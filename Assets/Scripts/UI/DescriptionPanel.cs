using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : UIBase
{
    public Button descButton;
    public Button closeButton;

    public override void Init()
    {
        GetCanvasGroup();

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
    }

    public override void Load()
    {

    }

    public override void Reset()
    {

    }

}
