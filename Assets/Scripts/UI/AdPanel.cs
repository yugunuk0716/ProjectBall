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
    }

    public override void Load()
    {
        
    }

   
}
