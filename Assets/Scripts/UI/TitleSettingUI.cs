
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingUI : UIBase
{
    public Button cancleButton;
    public Button bgmMuteBtn;
    public Button sfxMuteBtn;

    public TextMeshProUGUI heartCountText;
    public TextMeshProUGUI heartCoolText;

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


    public void UpdateHeartText(int count, string timer, bool isADSkip)
    {
        if (isADSkip)
        {
            heartCountText.text = $"∞/∞";
            heartCoolText.text = "∞:∞";
        }

        heartCountText.text = $"{count}/5";
        heartCoolText.text = timer;
    }

    public override void Load()
    {
        
    }

}
