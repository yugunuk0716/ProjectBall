using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkPopUp : UIBase
{
    public Button iseeBtn;


    public override void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        DOTween.To(() => canvasGroup.alpha, a => canvasGroup.alpha = a, on ? 1 : 0, 1f).SetUpdate(true);
    }

    public override void Init()
    {
        GetCanvasGroup();
        iseeBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        FunctionUpdater.Create(Checknetwork);
    }

    public void Checknetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && !canvasGroup.interactable)
        {
            ScreenOn(true);
        }
    }

    public override void Load()
    {
     
    }
}
