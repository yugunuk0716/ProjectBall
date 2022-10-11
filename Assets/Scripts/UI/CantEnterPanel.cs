using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantEnterPanel : UIBase
{
    public override void Init()
    {
        GetCanvasGroup();

    }


    public override void ScreenOn(bool on)
    {
        if (on)
        {
            Invoke("SetFalse", 3f);
        }

        base.ScreenOn(on);
    }

    private void SetFalse()
    {
        ScreenOn(false);
    }


    public override void Load()
    {

    }


}
