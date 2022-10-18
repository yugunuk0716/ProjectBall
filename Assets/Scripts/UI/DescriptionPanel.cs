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
        helpSwipeUI.parentCvsGroup = this.canvasGroup;

        descButton.onClick.AddListener(() =>
        {
            if (Input.touchCount > 1) return;

            ScreenOn(true);
        });

        closeButton.onClick.AddListener(() =>
        {
            ScreenOn(false);
            print("off");
        });

        leftBtn.onClick.AddListener(() =>
        {
            if(!helpSwipeUI.isSwipeMode)
            {
                helpSwipeUI.UpdateSwipe(true, false);
            }
        });
        rightBtn.onClick.AddListener(() =>
        {
            if (!helpSwipeUI.isSwipeMode)
            {
                helpSwipeUI.UpdateSwipe(true, true);
            }
        });
    }

    public override void Load()
    {

    }


}
