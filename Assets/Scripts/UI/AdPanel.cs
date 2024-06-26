using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdPanel : UIBase
{
    public Button watchButton;
    public Button cancleButton;

    public Image bgImage;
    public Image watchIcon;
    bool isOpenning = false;

    public override void Init()
    {
        GetCanvasGroup();
        cancleButton.onClick.AddListener(() => ScreenOn(false));
    }

    public override void Load()
    {
        
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
        if (on)
        {
            bgImage.rectTransform.DOComplete();
            bgImage.rectTransform.sizeDelta = new Vector2(880, 0);
        }


        canvasGroup.DOFade(on ? 1 : 0, 0.5f);

        Sequence seq = DOTween.Sequence();
        seq.Append(bgImage.rectTransform.DOSizeDelta(new Vector2(880, y), 1f).SetEase(Ease.InOutBack)).OnComplete(() =>
        {
            isOpenning = false;
        });

        seq.Join(watchButton.image.DOFade(on ? 1 : 0, 0.4f).SetDelay(0.6f));
        seq.Join(watchIcon.DOFade(on ? 0.7f : 0, 0.4f));
            
    }


}
