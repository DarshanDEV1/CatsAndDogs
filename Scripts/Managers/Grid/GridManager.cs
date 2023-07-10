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
    public string gameMode;

    private void Start()
    {
        gameMode = PlayerPrefs.GetString("GameMode");
        Debug.Log(gameMode);
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
        if(gameMode == "PVP") //  This condition works only when the game mode is player versus player
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
                if (!gameManager.isClicked && image.color != Color.red) gameManager.isClicked = true;
            }
        }
        else if(gameMode == "PVC")
        {
            //Add The AI
            if(gameManager.character == 1)
            {
                //In case of cat selected
                if(gameManager.turn)
                {
                    //Player
                    Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                    if (gameManager.isClicked && gameManager.isGameStarted) return;

                    if (gameManager.CheckTurn(1))
                    {
                        if (image.sprite != null) return;
                        image.sprite = catNDog_Sprites[0]; // Player places cat characters
                        gameManager.playerCharacterPlacement[0]--;
                    }

                    if (!gameManager.isGameStarted)
                        CheckPlayers();
                    else
                    {
                        GameState _gameState = new GameState(gameManager.isGameStarted, gameManager.turn, image);
                        if (!gameManager.isClicked && image.color != Color.red) gameManager.isClicked = true;
                    }
                }
                else
                {
                    //Computer
                    //ComputerPlayer(2);
                }
            }
            else if(gameManager.character == 2)
            {
                //In case of dog selected
                if (gameManager.turn)
                {
                    //Computer
                    //ComputerPlayer(1);
                }
                else
                {
                    //Player
                    Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                    if (gameManager.isClicked && gameManager.isGameStarted) return;

                    if (gameManager.CheckTurn(2))
                    {
                        if (image.sprite != null) return;
                        image.sprite = catNDog_Sprites[1]; // Player places cat characters
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
            }
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
            GameUpdate();
            ComputerCall();
        });
    }
    private bool IsEmpty(int row, int col)
    {
        Image image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
        return image.sprite == null;
    }
    private void ComputerPlayer(int playerNumber)
    {
        Image image;
        if (playerNumber == 1)
        {
            // Logic for computer player assuming player one's position

            // Beginning stage
            if (!gameManager.isGameStarted && gameManager.turn)
            {
                // Place three characters
                for (int i = 0; i < 3; i++)
                {
                    int row = Random.Range(0, 5);
                    int col = Random.Range(0, 5);
                    while (!IsEmpty(row, col))
                    {
                        row = Random.Range(0, 5);
                        col = Random.Range(0, 5);
                    }
                    image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                    if (gameManager.turn && gameManager.CheckTurn(1))
                    {
                        //if (image.sprite != null) continue;
                        image.sprite = catNDog_Sprites[0]; // Player 2 places dog characters
                        gameManager.playerCharacterPlacement[0]--;
                        if (!gameManager.isGameStarted)
                            CheckPlayers();
                    }
                }

                // Simulate clicking the submit button
                StartCoroutine(SimulateSubmitButtonClick());

                // Update game state and turn
                gameManager.turn = !gameManager.turn;
                GameUpdate();
                CheckPlayers();
            }

            //GamePlay stage
            if(gameManager.isGameStarted && gameManager.turn)
            {
                //Do Something here to simulate the button guess
                int row = Random.Range(6, 10);
                int col = Random.Range(0, 5);

                image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                Debug.Log(buttons[row, col].name);
                gameManager.isClicked = true;
                GameState _gameState = new GameState(gameManager.isGameStarted, gameManager.turn, image);
                gameManager.turn = !gameManager.turn;
                GameUpdate();
            }
        }
        else if (playerNumber == 2)
        {
            // Logic for computer player assuming player two's position
            // Beginning stage
            if (!gameManager.isGameStarted && !gameManager.turn)
            {
                // Place three characters
                for (int i = 0; i < 3; i++)
                {
                    int row = Random.Range(6, 10);
                    int col = Random.Range(0, 5);
                    while (!IsEmpty(row, col))
                    {
                        row = Random.Range(6, 10);
                        col = Random.Range(0, 5);
                    }
                    image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                    if (!gameManager.turn && gameManager.CheckTurn(2))
                    {
                        //if (image.sprite != null) continue;
                        image.sprite = catNDog_Sprites[1]; // Player 2 places dog characters
                        gameManager.playerCharacterPlacement[1]--;
                        if (!gameManager.isGameStarted)
                            CheckPlayers();
                    }
                }

                // Simulate clicking the submit button
                StartCoroutine(SimulateSubmitButtonClick());

                // Update game state and turn
                gameManager.turn = !gameManager.turn;
                GameUpdate();
            }

            //GamePlay stage
            if (gameManager.isGameStarted && !gameManager.turn)
            {
                //Do Something here to simulate the button guess
                int row = Random.Range(0, 5);
                int col = Random.Range(0, 5);

                image = buttons[row, col].transform.GetChild(0).GetComponent<Image>();
                Debug.Log(buttons[row, col].name);
                gameManager.isClicked = true;
                GameState _gameState = new GameState(gameManager.isGameStarted, gameManager.turn, image);
                gameManager.turn = !gameManager.turn;
                GameUpdate();
            }
        }
    }

    private IEnumerator SimulateSubmitButtonClick()
    {
        yield return new WaitForSeconds(1f); // Adjust the delay as needed

        // Simulate clicking the submit button
        submitButton.onClick.Invoke();
    }

    public void ComputerCall()
    {
        if (gameMode == "PVC")
        {
            //Do something
            if (gameManager.character == 2 && gameManager.turn)
            {
                ComputerPlayer(1);
            }
            else if (gameManager.character == 1 && !gameManager.turn)
            {
                ComputerPlayer(2);
            }
        }
    }

    public void GameUpdate()
    {
        CheckPlayers();// To hide the players
        TurnBasedPlacement();// To make the buttons interactable or non interactable
        gameManager.GameWon();
        gameManager.TurnTextSwitch();
        gameManager.isClicked = false;
    }
}
