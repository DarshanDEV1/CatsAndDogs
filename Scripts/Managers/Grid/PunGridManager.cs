using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon;
using Photon.Pun;
using Photon.Realtime;

public abstract class PunGameStateCall : GridManager
{
    //Do all the operations that are required to do during gamestate
    public abstract void ButtonCheck(bool _game_State, bool _player_Turn, Image _button_Clicked_Image);
}
public class PunGameState : PunGameStateCall
{
    public PunGameState(bool _game_State, bool _player_Turn, Image _button_Clicked_Image)
    {
        ButtonCheck(_game_State, _player_Turn, _button_Clicked_Image);
    }
    public override void ButtonCheck(bool _game_State, bool _player_Turn, Image _button_Clicked_Image)
    {
        PunGameManager _game_Manager = FindObjectOfType<PunGameManager>();
        //throw new System.NotImplementedException();
        if (_game_State)
        {
            if (_player_Turn)
            {
                Sprite _sprite = _button_Clicked_Image.sprite;
                if (_sprite == null) return;
                if (_sprite.name == "Dog_0" && _game_Manager.playersScores[0] > 0 && _button_Clicked_Image.color != Color.red)
                {
                    Debug.Log("Dog Found...");
                    _game_Manager.playersScores[0]--;
                    _button_Clicked_Image.enabled = true;
                    if (!_game_Manager.isClicked) _game_Manager.isClicked = true;
                    _button_Clicked_Image.color = Color.red;
                }
            }
            else
            {
                Sprite _sprite = _button_Clicked_Image.sprite;
                if (_sprite == null) return;
                if (_sprite.name == "Cat_0" && _game_Manager.playersScores[1] > 0 && _button_Clicked_Image.color != Color.red)
                {
                    Debug.Log("Cat Found...");
                    _game_Manager.playersScores[1]--;
                    _button_Clicked_Image.enabled = true;
                    if (!_game_Manager.isClicked) _game_Manager.isClicked = true;
                    _button_Clicked_Image.color = Color.red;
                }
            }
        }
    }
    ~PunGameState()
    {
        Debug.Log("Destroyed the instance...");
    }
}

public class PunGridManager : MonoBehaviour
{
    public Button buttonPrefab;
    public Sprite[] catNDog_Sprites;
    public PunGameManager punGameManager;
    public Button[,] buttons;
    public Button submitButton;
    [SerializeField] GameObject startGamePanel;
    public string gameMode;

    private void Start()
    {
        gameMode = PlayerPrefs.GetString("GameMode");
        Debug.Log(gameMode);
        punGameManager = FindObjectOfType<PunGameManager>();
        CreateGrid();
        CheckPlayers();
        ButtonClicksDetection();
        TurnBasedPlacement();
        punGameManager.TurnTextSwitch();
    }
    void CreateGrid()
    {
        buttons = new Button[10, 5];
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                Button button = PhotonNetwork.Instantiate(buttonPrefab.name, transform.position, Quaternion.identity).GetComponent<Button>();
                button.transform.SetParent(transform);
                button.transform.localScale = Vector3.one;

                buttons[row, col] = button;

                int buttonRow = row;
                int buttonCol = col;
                button.name = "BTN: " + buttonRow.ToString() + " " + buttonCol.ToString();
                button.onClick.AddListener(() =>
                {
                    ButtonClicked(buttonRow, buttonCol);
                });
            }
        }
    }

    void ButtonClicked(int row, int col)
    {
        if (gameMode == "OPVP")
        {
            Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
            if (punGameManager.isClicked && punGameManager.isGameStarted) return;

            if (punGameManager.turn && punGameManager.CheckTurn(1))
            {
                if (image.sprite != null) return;
                image.sprite = catNDog_Sprites[0]; // Player 1 places dog characters
                punGameManager.playerCharacterPlacement[0]--;
            }
            else if (!punGameManager.turn && punGameManager.CheckTurn(2))
            {
                if (image.sprite != null) return;
                image.sprite = catNDog_Sprites[1]; // Player 2 places cat characters
                punGameManager.playerCharacterPlacement[1]--;
            }
            if (!punGameManager.isGameStarted)
                CheckPlayers();
            else
            {
                PunGameState _gameState = new PunGameState(punGameManager.isGameStarted, punGameManager.turn, image);
                if (!punGameManager.isClicked && image.color != Color.red) punGameManager.isClicked = true;
            }
        }
    }

    void CheckPlayers()
    {
        Sprite currentPlayerSprite = punGameManager.turn ? catNDog_Sprites[0] : catNDog_Sprites[1];
        punGameManager.CheckSubmitButtonInteractable();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                Sprite sprite = image.sprite;

                if (sprite == currentPlayerSprite && image.color != Color.red)
                {
                    image.enabled = true;
                }
                else if (sprite != currentPlayerSprite && image.color != Color.red)
                {
                    image.enabled = false;
                }
                else if (sprite == currentPlayerSprite && image.color == Color.red)
                {
                    image.enabled = true;
                }
            }
        }
    }

    void TurnBasedPlacement()
    {
        bool player1Turn = punGameManager.turn;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (player1Turn)
                {
                    if (row >= 0 && row <= 4)
                    {
                        buttons[row, col].interactable = !punGameManager.isGameStarted;
                    }
                    else
                    {
                        buttons[row, col].interactable = punGameManager.isGameStarted;
                    }
                }
                else
                {
                    if (row >= 5 && row <= 10)
                    {
                        buttons[row, col].interactable = !punGameManager.isGameStarted;
                    }
                    else
                    {
                        buttons[row, col].interactable = punGameManager.isGameStarted;
                    }
                }
            }
        }
    }

    IEnumerator StartGame()
    {
        startGamePanel.SetActive(true);
        punGameManager.isGameStarted = true;
        startGamePanel.GetComponent<Animator>().Play("StartAnimation");
        yield return new WaitForSeconds(2f);
        startGamePanel.SetActive(false);
    }

    void ButtonClicksDetection()
    {
        submitButton.onClick.AddListener(() =>
        {
            //Do something on clicked submit button
            if (punGameManager.playerCharacterPlacement[0] > 0 || punGameManager.playerCharacterPlacement[1] > 0)
            {
                punGameManager.turn = !punGameManager.turn;
            }
            else
            {
                if (punGameManager.playerCharacterPlacement[0] <= 0 && punGameManager.playerCharacterPlacement[1] <= 0)
                {
                    //Start the game logic
                    StartCoroutine(StartGame());
                    punGameManager.turn = !punGameManager.turn;
                }
            }
            GameUpdate();
        });
    }
    private bool IsEmpty(int row, int col)
    {
        Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
        return image.sprite == null;
    }
    public void GameUpdate()
    {
        CheckPlayers();// To hide the players
        TurnBasedPlacement();// To make the buttons interactable or non interactable
        punGameManager.GameWon();
        punGameManager.TurnTextSwitch();
        punGameManager.isClicked = false;
    }
}
