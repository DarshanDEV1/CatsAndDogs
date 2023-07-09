using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DT_UI
{
    [System.Serializable]
    public class UIManager : MonoBehaviour
    {
        [Header("Button References")]
        [SerializeField] private List<UiButton> uiButton = new List<UiButton>();
        private Dictionary<string, Button> uiButtonDictionary = new Dictionary<string, Button>();

        [Header("InputField References")]
        [SerializeField] private List<UiInputField> uiInputField = new List<UiInputField>();
        private Dictionary<string, TMP_InputField> uiInputFieldDictionary = new Dictionary<string, TMP_InputField>();

        private void Awake()
        {
            UpdateButtonDictionary();
            UpdateInputFieldDictionary();
        }

        private void UpdateButtonDictionary()
        {
            uiButtonDictionary.Clear();
            foreach (UiButton uiReference in uiButton)
            {
                if (uiReference.button != null && !uiButtonDictionary.ContainsKey(uiReference.key))
                {
                    uiButtonDictionary.Add(uiReference.key, uiReference.button);
                }
            }
        }

        private void UpdateInputFieldDictionary()
        {
            uiInputFieldDictionary.Clear();
            foreach (UiInputField uiReference in uiInputField)
            {
                if (uiReference.inputField != null && !uiInputFieldDictionary.ContainsKey(uiReference.key))
                {
                    uiInputFieldDictionary.Add(uiReference.key, uiReference.inputField);
                }
            }
        }

        public Button GetButton(string key)
        {
            if (uiButtonDictionary.ContainsKey(key))
            {
                return uiButtonDictionary[key];
            }
            return null;
        }
        public TMP_InputField GetInputField(string key)
        {
            if (uiInputFieldDictionary.ContainsKey(key))
            {
                return uiInputFieldDictionary[key];
            }
            return null;
        }
    }

    [System.Serializable]
    public struct UiButton
    {
        public string key;
        public Button button;
    }

    [System.Serializable]
    public struct UiInputField
    {
        public string key;
        public TMP_InputField inputField;
    }
}