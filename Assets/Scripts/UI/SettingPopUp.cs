using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopUp : UIBase
{
    public Button menuButton;
    public Button resumeButton;
    public Button homeButton;
    public Button quitButton;

    public Button sfxButton;
    public Button bgmButton;

    public override void Init()
    {
        GetCanvasGroup();
        menuButton.onClick.AddListener(() => ScreenOn(true));
        resumeButton.onClick.AddListener(() => ScreenOn(false));
        homeButton.onClick.AddListener(() => { SceneManager.LoadScene("Title"); }); // 버튼에 씬이동하게 만들기
        quitButton.onClick.AddListener(() => Application.Quit());

        sfxButton.onClick.AddListener(() =>
        {
            SoundManager.isSFXMute = !SoundManager.isSFXMute;
            sfxButton.GetComponent<Image>().color = SoundManager.isSFXMute ? Color.gray : Color.white;
        });

        bgmButton.onClick.AddListener(() =>
        {
            SoundManager.isBGMMute = !SoundManager.isBGMMute;
            bgmButton.GetComponent<Image>().color = SoundManager.isBGMMute ? Color.gray : Color.white;
        });
    }

    public override void Load()
    {
        
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
