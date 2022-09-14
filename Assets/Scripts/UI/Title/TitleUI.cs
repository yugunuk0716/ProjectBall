using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public TitleSettingPopUp settingPopUp;

    private void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene("NewGame"));
        optionButton.onClick.AddListener(() => settingPopUp.ScreenOn(true));
    }
}
