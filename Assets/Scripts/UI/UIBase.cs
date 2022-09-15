using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class UIBase : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    
    protected void GetCanvasGroup() => canvasGroup = gameObject.GetComponent<CanvasGroup>();

    public virtual void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        DOTween.To(() => canvasGroup.alpha, a => canvasGroup.alpha = a, on ? 1 : 0, 0.5f).SetUpdate(true);
    }

    public abstract void Init();
    public abstract void Load();
}
