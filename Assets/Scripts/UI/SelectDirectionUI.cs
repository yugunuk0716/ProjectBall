using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectDirectionUI : UIBase
{
    [SerializeField] private Sprite directionSprites;

    [HideInInspector] public BallControllUI ballControllUI;
    private MapLoadVideoPlayer mapLoadVideoPlayer;

    public bool isSelecting = false;

    public void Set(BallControllUI ballControllUI)
    {
        this.ballControllUI = ballControllUI;
    }

    public override void Init()
    {
        GetCanvasGroup();

        mapLoadVideoPlayer = GetComponent<MapLoadVideoPlayer>();
        Button[] selectDirectionBtns = GetComponentsInChildren<Button>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.TakeMapLoadVideo = () => mapLoadVideoPlayer.TakeVideo();

        

        for (int i = 0; i< selectDirectionBtns.Length; i++) // 0번은 Nothing 들어갑니다.
        {
            int index = 1;

            for (int j = 0; j < i; j++)
                index <<= 1;
            Animator anim = selectDirectionBtns[i].GetComponentInChildren<Animator>();
            selectDirectionBtns[i].onClick.AddListener(() =>
            {
                anim.SetTrigger("OnClick");
                ScreenOn(false);

                int insertIndex = Mathf.Clamp(ballControllUI.order, 0, gm.myBallList.Count);
                ballControllUI.ball.shootDir = (TileDirection)(index);

                ballControllUI.SetDirection(ballControllUI.ball.shootDir);
            });
        }
    }

    public override void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        DOTween.To(() => canvasGroup.alpha, a => canvasGroup.alpha = a, on ? 1 : 0, 0.75f).SetUpdate(true);

        if (on)
        {
            mapLoadVideoPlayer.PlayVideo();
        }
    }

    public override void Load()
    {
        ScreenOn(false);
    }

    public override void Reset()
    {
        throw new NotImplementedException();
    }
}
