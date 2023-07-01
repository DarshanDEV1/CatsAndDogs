using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool turn = true;
    public bool isGameStarted = false;
    public bool isClicked = false;
    public int[] playerCharacterPlacement = new int[2] { 3, 3 };
    [SerializeField] GridManager gridController;
    public int[]playersScores = new int[2] { 3, 3 };
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text playerTurnText;
    [SerializeField] Button backButton;
    [SerializeField] Button restartButton;

    private void Start()
    {
        CheckSubmitButtonInteractable();
        GameWon();
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StartScene");
        });
    }

    public bool CheckTurn(int player)
    {
        if (player == 1)
        {
            if (playerCharacterPlacement[0] > 0)
            {
                return true;
            }
        }
        else
        {
            if (playerCharacterPlacement[1] > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckSubmitButtonInteractable()
    {
        gridController = FindObjectOfType<GridManager>();
        if (turn && !isGameStarted)
        {
            if (playerCharacterPlacement[0] > 0)
            {
                gridController.submitButton.interactable = false;
            }
            else
            {
                gridController.submitButton.interactable = true;
            }
        }
        else if (!turn && !isGameStarted)
        {
            if (playerCharacterPlacement[1] > 0)
            {
                gridController.submitButton.interactable = false;
            }
            else
            {
                gridController.submitButton.interactable = true;
            }
        }
    }

    public void GameWon()
    {
        if(isGameStarted && playersScores[0] <= 0)
        {
            //Game Win Panel For Player One
            winPanel.SetActive(true);
            TMP_Text _text = winPanel.transform.GetChild(0).GetComponent<TMP_Text>();
            _text.text = "The winner is player one...";
        }
        else if(isGameStarted && playersScores[1] <= 0)
        {
            //Game Win Panel For Player Two
            winPanel.SetActive(true);
            TMP_Text _text = winPanel.transform.GetChild(0).GetComponent<TMP_Text>();
            _text.text = "The winner is player two...";
        }
        else
        {
            winPanel.SetActive(false);
        }
    }

    public void TurnTextSwitch()
    {
        if(turn)
        {
            playerTurnText.text = "Turn : Player 1";
        }
        else
        {
            playerTurnText.text = "Turn : Player 2";
        }
    }
}
