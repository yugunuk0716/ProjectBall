using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUp : UIBase
{
    public Button menuButton;
    public Button resumeButton;
    public Button optionButton;
    public Button homeButton;
    public Button quitButton;

    public Button sfxButton;
    public Button bgmButton;

    public override void Init()
    {
        GetCanvasGroup();
        menuButton.onClick.AddListener(() => ScreenOn(true));
        resumeButton.onClick.AddListener(() => ScreenOn(false));
        homeButton.onClick.AddListener(() => { }); // 버튼에 씬이동하게 만들기
        quitButton.onClick.AddListener(() => Application.Quit());
    }

    public override void Load()
    {
        
    }

   
}
