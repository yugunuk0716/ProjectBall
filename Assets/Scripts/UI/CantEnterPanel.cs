using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CantEnterPanel : UIBase
{
    Button btn;
    [SerializeField] Image bgImage;

    bool isOpenning = false;

    //float ratioY, ratioX;

    public override void Init()
    {
        //ratioY = (float)Screen.height / 1920;
        //ratioX = (float)Screen.width / 1080;

        bgImage.rectTransform.sizeDelta = new Vector2(880, 0);
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            ScreenOn(false);
        });

        GetCanvasGroup();

    }


    public override void ScreenOn(bool on)
    {
        if (isOpenning)
        {
            return;
        }
        else
        {
            isOpenning = true;
        }

        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;

        float y = on ? 1110 : 0;
        if(on)
        {
            bgImage.rectTransform.DOComplete(); 
            bgImage.rectTransform.sizeDelta = new Vector2(880, 0);
        }
        bgImage.rectTransform.DOSizeDelta(new Vector2(880, y), 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            isOpenning = false;
        });
    }

    public override void Load()
    {
        
    }


}
