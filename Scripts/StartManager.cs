using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

public class StartManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button playerVPlayer;
    [SerializeField] Button playerVComputer;
    [SerializeField] Button onlinePlayerVPlayer;
    [SerializeField] GameObject multiplayerConnectingPanel;

    private void Start()
    {
        PlayerPrefs.GetString("GameMode", "Default");
        playerVPlayer.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("GameMode", "PVP");
            LoadScene("PvP_Scene");
        });
        playerVComputer.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("GameMode", "PVC");
            LoadScene("PvComp_Scene");
        });
        onlinePlayerVPlayer.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("GameMode", "OPVP");
            PhotonNetwork.ConnectUsingSettings();
            multiplayerConnectingPanel.SetActive(true);
            //LoadScene("Online_PvP_Scene");
        });
    }

    private void LoadScene(string _scene_Name)
    {
        SceneManager.LoadScene(_scene_Name);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("DemoRoom", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Online_PvP_Scene");
    }
}
