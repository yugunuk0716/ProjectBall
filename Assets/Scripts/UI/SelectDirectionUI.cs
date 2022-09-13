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

    [HideInInspector] public BallControllUI ballControllUI;

    public void Set(Ball addBall, BallControllUI ballControllUI)
    {
        this.addBall = addBall;
        this.ballControllUI = ballControllUI;
    }



    public void Init(Action Callback)
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
        Button[] selectDirectionBtns = GetComponentsInChildren<Button>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
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
                Callback();
            });
        }
    }

    public void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;
    }
}
