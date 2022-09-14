using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingPopUp : UIBase
{
    public Button bgmButton;
    public Button sfxButton;
    public Button cancleButton;

    private void Awake()
    {
        GetCanvasGroup();
    }

    public void Start()
    {
        bgmButton.onClick.AddListener(() => print("아직 브금 버튼에 이벤트가 비어있습니다"));
        sfxButton.onClick.AddListener(() => print("아직 사운드 이펙트 버튼에 이벤트가 비어있습니다"));
        cancleButton.onClick.AddListener(() =>
        {
            ScreenOn(false);
        });
       
    }

    public override void Load()
    {
        
    }

    public override void Init()
    {

    }
}
