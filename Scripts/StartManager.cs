using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] Button playerVPlayer;
    [SerializeField] Button playerVComputer;
    [SerializeField] Button onlinePlayerVPlayer;

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
            LoadScene("Online_PvP_Scene");
        });
    }

    private void LoadScene(string _scene_Name)
    {
        SceneManager.LoadScene(_scene_Name);
    }
}
