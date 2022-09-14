using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDirectionUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [HideInInspector] public Ball addBall; // 추가할 공.. 여기서 가지고 있으면 날먹이 가능해여
    [SerializeField] private Sprite directionSprites;

    [HideInInspector] public int order;

    [HideInInspector] public BallControllUI ballControllUI;

    MapLoadVideoPlayer mapLoadVideoPlayer;

    public void Set(Ball addBall, BallControllUI ballControllUI, int order)
    {
        this.addBall = addBall;
        this.ballControllUI = ballControllUI;
        this.order = order;
    }



    public void Init(Action Callback)
    {
        mapLoadVideoPlayer = GetComponent<MapLoadVideoPlayer>();
        mapLoadVideoPlayer.FindCam();
        canvasGroup = GetComponent<CanvasGroup>(); 
        Button[] selectDirectionBtns = GetComponentsInChildren<Button>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.TakeMapLoadVideo = () => mapLoadVideoPlayer.TakeVideo();
        for (int i = 0; i< selectDirectionBtns.Length; i++) // 0번은 Nothing 들어갑니다.
        {
            int index = 1;

            for (int j = 0; j < i; j++)
                index <<= 1;

            selectDirectionBtns[i].onClick.AddListener(() =>
            {
                addBall.shootDir = (TileDirection)(index);
                gm.myBallList.Add(addBall);
                gm.lastBallList.Add(addBall);
                ScreenOn(false);
                ballControllUI.SetDirection(addBall.shootDir);

                gm.BallUiSort();

                Callback();


            });
        }
    }

    public void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;

        if(on)
        {
            mapLoadVideoPlayer.PlayVideo();
        }
    }
}
