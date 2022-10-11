using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CantEnterPanel : UIBase
{
    Button btn;
    [SerializeField] Image bgImage;

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
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;

        float y = on ? 1110 : 0;
        bgImage.DOComplete();
        bgImage.rectTransform.DOSizeDelta(new Vector2(680, y), 1f);
    }

    public override void Load()
    {
        
    }


}
