using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPopUp : UIBase
{
    public Button menuButton;
    public Button stageUIButton;
    public Button resumeButton;
    public Button homeButton;
    public Button quitButton;
    public Button sideButton;

    public Button sfxButton;
    public Button bgmButton;

    private UIManager uimanager;
    private StageManager stageManager;
    private bool isActive = false;

    public override void Init()
    {
        uimanager = IsometricManager.Instance.GetManager<UIManager>();
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        stageManager = IsometricManager.Instance.GetManager<StageManager>();
        GetCanvasGroup();
        menuButton.onClick.AddListener(() =>
        {
            if(GameManager.canInteract && !stageManager.isMapLoading)
            {
                ScreenOn(true);
            }
        });
        resumeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            ScreenOn(false);
        });

        TileHelpUI tileHelp = uimanager.FindUI("HelpPanel").GetComponent<TileHelpUI>();
        homeButton.onClick.AddListener(() => 
        {
            Time.timeScale = 1;
            ScreenOn(false);
            
            uimanager.canvas[0].gameObject.SetActive(true);
            uimanager.canvas[0].DOFade(1, 0.5f).SetUpdate(true);
            
            uimanager.canvas[1].DOFade(0, 0.5f).SetUpdate(true);

            tileHelp.MoveUI(false);

        });

        sideButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            ScreenOn(false);
        });

        stageUIButton.onClick.AddListener(() =>
        {
            uimanager.FindUI("StageNumberPanel").ScreenOn(true);
        });

        quitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Application.Quit();
        });

        sfxButton.onClick.AddListener(() =>
        {
            sm.Mute(AudioType.SFX);
            sfxButton.GetComponent<Image>().color = SoundManager.isSFXMute ? Color.gray : Color.white;
        });

        bgmButton.onClick.AddListener(() =>
        {
            sm.Mute(AudioType.BGM);
            bgmButton.GetComponent<Image>().color = SoundManager.isBGMMute ? Color.gray : Color.white;
        });

        FunctionUpdater.Create(CheckBack);
    }

    public override void Load()
    {
    }

    public override void ScreenOn(bool on)
    { 
        base.ScreenOn(on);
        isActive = on;

        if (true == on)
        {
            Time.timeScale = 0;
        }
    }

    public void CheckBack()
    {

        if (isActive)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    if (canvasGroup.interactable)
                    {
                        ScreenOn(false);
                    }
                    else
                    {
                        ScreenOn(true);
                    }
                }
            }
        }
        
    }
    
}
