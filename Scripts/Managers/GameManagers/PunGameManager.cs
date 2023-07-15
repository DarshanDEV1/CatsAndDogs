using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

public class PunGameManager : MonoBehaviour
{
    public bool turn = true;
    public bool isGameStarted = false;
    public bool isClicked = false;
    public int[] playerCharacterPlacement = new int[2] { 3, 3 };
    [SerializeField] PunGridManager gridController;
    public int[] playersScores = new int[2] { 3, 3 };
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text playerTurnText;
    [SerializeField] Button backButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button[] chooseCharacter;
    public int character;
    [SerializeField] GameObject characterPanel;
    [SerializeField] GameObject player;

    private void Awake()
    {
        PhotonNetwork.Instantiate(player.name, transform.position, Quaternion.identity);
    }

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
            PhotonNetwork.LoadLevel("StartScene");
        });
        chooseCharacter[0].onClick.AddListener(() =>
        {
            //Do something
            character = 1;
            characterPanel.SetActive(false);
        });
        chooseCharacter[1].onClick.AddListener(() =>
        {
            //Do something
            character = 2;
            characterPanel.SetActive(false);
            gridController.GameUpdate();
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
        gridController = FindObjectOfType<PunGridManager>();
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
        if (isGameStarted && playersScores[0] <= 0)
        {
            //Game Win Panel For Player One
            winPanel.SetActive(true);
            TMP_Text _text = winPanel.transform.GetChild(0).GetComponent<TMP_Text>();
            _text.text = "The winner is player one...";
        }
        else if (isGameStarted && playersScores[1] <= 0)
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
        if (turn)
        {
            playerTurnText.text = "Turn : Player 1";
        }
        else
        {
            playerTurnText.text = "Turn : Player 2";
        }
    }
}
