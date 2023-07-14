using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using DT_UI;

public class StartManager : MonoBehaviourPunCallbacks
{
    #region VARIABLES

    [SerializeField] GameObject multiplayerConnectingPanel;
    [SerializeField] UIManager _uiManager;

    KeyValue keyvalue = new KeyValue();

    #endregion

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        PlayerPrefs.GetString("GameMode", "Default");
        _uiManager.GetButton(keyvalue.PlayerVsPlayer).onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("GameMode", "PVP");
            LoadScene("PvP_Scene");
        });
        _uiManager.GetButton(keyvalue.PlayerVsComputer).onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("GameMode", "PVC");
            LoadScene("PvComp_Scene");
        });
        _uiManager.GetButton(keyvalue.OnlinePlayerVsPlayer).onClick.AddListener(() =>
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
