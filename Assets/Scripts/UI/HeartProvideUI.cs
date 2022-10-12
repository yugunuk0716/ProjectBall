using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartProvideUI : UIBase
{
    public Button checkButton;

    private AdPanel adPanel;

    public override void Init()
    {
        GetCanvasGroup();
        adPanel = IsometricManager.Instance.GetManager<UIManager>().FindUI("WatchAddPanel").GetComponent<AdPanel>();
        checkButton.onClick.AddListener(() =>
        {
            ScreenOn(false);
            adPanel.ScreenOn(false);
        });
    }

    public override void Load()
    {
        
    }

}
