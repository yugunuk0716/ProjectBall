using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : UIBase
{
    public Button descButton;
    public Button closeButton;
    public Button leftBtn;
    public Button rightBtn;

    SwipeUI helpSwipeUI;

    public override void Init()
    {
        GetCanvasGroup();
        helpSwipeUI = GetComponentInChildren<SwipeUI>();

        descButton.onClick.AddListener(() =>
        {
            ScreenOn(true);
            print("on");
        });

        closeButton.onClick.AddListener(() =>
        {
            ScreenOn(false);
            print("off");
        });

        leftBtn.onClick.AddListener(() => helpSwipeUI.UpdateSwipe(true, false));
        rightBtn.onClick.AddListener(() => helpSwipeUI.UpdateSwipe(true, true));
    }

    public override void Load()
    {

    }


}
