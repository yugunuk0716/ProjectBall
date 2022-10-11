using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CantEnterPanel : UIBase
{
    Button btn;

    public override void Init()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            ScreenOn(false);
        });

        GetCanvasGroup();

    }


    public override void ScreenOn(bool on)
    {
        base.ScreenOn(on);
    }

    public override void Load()
    {
        
    }


}
