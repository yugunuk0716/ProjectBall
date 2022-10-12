using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdPanel : UIBase
{
    public Button watchButton;
    public Button cancleButton;

    public override void Init()
    {
        GetCanvasGroup();
        cancleButton.onClick.AddListener(() => ScreenOn(false));
        watchButton.onClick.AddListener(() =>
        {
            //print("리워드 광고 요청");
            IsometricManager.Instance.RequestRewardAd.Invoke();
        });
    }

    public override void Load()
    {
        
    }

   
}
