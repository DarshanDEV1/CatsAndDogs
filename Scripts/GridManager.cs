using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class GameStateCall : GridManager
{
    //Do all the operations that are required to do during gamestate
    public abstract void ButtonCheck(bool _game_State, bool _player_Turn, Image _button_Clicked_Image);
}
public class GameState : GameStateCall
{
    public GameState(bool _game_State, bool _player_Turn, Image _button_Clicked_Image)
    {
        ButtonCheck(_game_State, _player_Turn, _button_Clicked_Image);
    }
    public override void ButtonCheck(bool _game_State, bool _player_Turn, Image _button_Clicked_Image)
    {
        GameManager _game_Manager = FindObjectOfType<GameManager>();
        //throw new System.NotImplementedException();
        if (_game_State)
        {
            if(_player_Turn)
            {
                Sprite _sprite = _button_Clicked_Image.sprite;
                if (_sprite == null) return;
                if (_sprite.name == "Dog_0" && _game_Manager.playersScores[0] > 0 && _button_Clicked_Image.color != Color.red)
                {
                    Debug.Log("Dog Found...");
                    _game_Manager.playersScores[0]--;
                    _button_Clicked_Image.enabled = true;
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
                    _button_Clicked_Image.color = Color.red;
                }
            }
        }
    }
    ~GameState()
    {
        Debug.Log("Destroyed the instance...");
    }
}

public class GridManager : MonoBehaviour
{
    public Button buttonPrefab;
    public Sprite[] catNDog_Sprites;
    public GameManager gameManager;
    public Button[,] buttons;
    public Button submitButton;
    [SerializeField] GameObject startGamePanel;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        CreateGrid();
        CheckPlayers();
        ButtonClicksDetection();
        TurnBasedPlacement();
        gameManager.TurnTextSwitch();
    }
    void CreateGrid()
    {
        buttons = new Button[10, 5];
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                Button button = Instantiate(buttonPrefab, transform);
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
        Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
        if (gameManager.isClicked && gameManager.isGameStarted) return;

        if (gameManager.turn && gameManager.CheckTurn(1))
        {
            if (image.sprite != null) return;
            image.sprite = catNDog_Sprites[0]; // Player 1 places dog characters
            gameManager.playerCharacterPlacement[0]--;
        }
        else if (!gameManager.turn && gameManager.CheckTurn(2))
        {
            if (image.sprite != null) return;
            image.sprite = catNDog_Sprites[1]; // Player 2 places cat characters
            gameManager.playerCharacterPlacement[1]--;
        }
        if (!gameManager.isGameStarted)
            CheckPlayers();
        else
        {
            GameState _gameState = new GameState(gameManager.isGameStarted, gameManager.turn, image);
            if (!gameManager.isClicked) gameManager.isClicked = true;
        }
    }

    void CheckPlayers()
    {
        Sprite currentPlayerSprite = gameManager.turn ? catNDog_Sprites[0] : catNDog_Sprites[1];
        gameManager.CheckSubmitButtonInteractable();
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
                else if(sprite != currentPlayerSprite && image.color != Color.red)
                {
                    image.enabled = false;
                }
                else if(sprite == currentPlayerSprite && image.color == Color.red)
                {
                    image.enabled = true;
                }
            }
        }
    }

    void TurnBasedPlacement()
    {
        bool player1Turn = gameManager.turn;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (player1Turn)
                {
                    if (row >= 0 && row <= 4)
                    {
                        buttons[row, col].interactable = !gameManager.isGameStarted;
                    }
                    else
                    {
                        buttons[row, col].interactable = gameManager.isGameStarted;
                    }
                }
                else
                {
                    if (row >= 5 && row <= 10)
                    {
                        buttons[row, col].interactable = !gameManager.isGameStarted;
                    }
                    else
                    {
                        buttons[row, col].interactable = gameManager.isGameStarted;
                    }
                }
            }
        }
    }

    IEnumerator StartGame()
    {
        startGamePanel.SetActive(true);
        gameManager.isGameStarted = true;
        startGamePanel.GetComponent<Animator>().Play("StartAnimation");
        yield return new WaitForSeconds(2f);
        startGamePanel.SetActive(false);
    }

    void ButtonClicksDetection()
    {
        submitButton.onClick.AddListener(() =>
        {
            //Do something on clicked submit button
            if (gameManager.playerCharacterPlacement[0] > 0 || gameManager.playerCharacterPlacement[1] > 0)
            {
                gameManager.turn = !gameManager.turn;
                //CheckPlayers();
            }
            else
            {
                if (gameManager.playerCharacterPlacement[0] <= 0 && gameManager.playerCharacterPlacement[1] <= 0)
                {
                    //Start the game logic
                    StartCoroutine(StartGame());
                    gameManager.turn = !gameManager.turn;
                }
            }
            CheckPlayers();
            TurnBasedPlacement();
            gameManager.GameWon();
            gameManager.TurnTextSwitch();
            gameManager.isClicked = false;
        });
    }

    private void ComputerPlayer(int player)
    {
        //Write the code to create the AI player
    }
}
