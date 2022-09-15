using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectDirectionUI : UIBase
{
    [HideInInspector] public Ball addBall; // 추가할 공.. 여기서 가지고 있으면 날먹이 가능해여
    [SerializeField] private Sprite directionSprites;

    [HideInInspector] public int order;
    [HideInInspector] public BallControllUI ballControllUI;
    public MapLoadVideoPlayer mapLoadVideoPlayer;

    public bool isSelecting = false;

    public void Set(Ball addBall, BallControllUI ballControllUI, int order)
    {
        this.addBall = addBall;
        this.ballControllUI = ballControllUI;
        this.order = order;
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

                addBall.shootDir = (TileDirection)(index);
                gm.myBallList.Add(addBall);
                gm.lastBallList.Add(addBall);
                
                ballControllUI.SetDirection(addBall.shootDir);
                gm.BallUiSort();

                
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
}
