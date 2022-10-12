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

    public override void Init()
    {
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

        float y = on ? 1110 : 0;
        if(on)
        {
            bgImage.rectTransform.DOComplete(); 
            bgImage.rectTransform.sizeDelta = new Vector2(880, 0);
        }


        canvasGroup.DOFade(on ? 1 : 0, 0.5f);
        bgImage.rectTransform.DOSizeDelta(new Vector2(880, y), 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            isOpenning = false;
        });
    }

    public override void Load()
    {
        
    }


}
