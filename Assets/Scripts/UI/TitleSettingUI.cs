using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingUI : UIBase
{
    public Button cancleButton;
    public Button bgmMuteBtn;
    public Button sfxMuteBtn;

    public override void Init()
    {
        GetCanvasGroup();
        cancleButton.onClick.AddListener(() => ScreenOn(false));
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();

        bgmMuteBtn.onClick.AddListener(() => {
            sm.Mute(AudioType.BGM);
            bgmMuteBtn.GetComponent<Image>().color = SoundManager.isBGMMute ? Color.gray : Color.white;
        });
        sfxMuteBtn.onClick.AddListener(() => {
            sm.Mute(AudioType.SFX);
            sfxMuteBtn.GetComponent<Image>().color = SoundManager.isSFXMute ? Color.gray : Color.white;
        });
    }

    public override void Load()
    {
        
    }

}
