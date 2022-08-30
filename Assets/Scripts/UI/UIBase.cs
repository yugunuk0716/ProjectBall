using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class UIBase : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    protected void GetCanvasGroup() => canvasGroup = gameObject.GetComponent<CanvasGroup>();

    public void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;
    }

    public abstract void Init();
    public abstract void Load();
}
