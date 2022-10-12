using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartProvideUI : UIBase
{
    public Button checkButton;

    public override void Init()
    {
        GetCanvasGroup();

        checkButton.onClick.AddListener(() => ScreenOn(false));
    }

    public override void Load()
    {
        
    }

}
