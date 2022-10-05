using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class SelectDirectionUI : UIBase
{
    [HideInInspector] public BallControllUI ballControllUI;
    private MapLoadVideoPlayer mapLoadVideoPlayer;

    [HideInInspector]
    public List<Button> selectDirectionBtns;

    public bool isSelecting = false;

    Image myImage;

    float width, height;

    public void Set(BallControllUI ballControllUI)
    {
        this.ballControllUI = ballControllUI;
    }

    public override void Init()
    {
        GetCanvasGroup();
        myImage = GetComponent<Image>();
        width = myImage.rectTransform.rect.width;
        height = myImage.rectTransform.rect.height;

        mapLoadVideoPlayer = GetComponent<MapLoadVideoPlayer>();
        selectDirectionBtns = GetComponentsInChildren<Button>().ToList();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.TakeMapLoadVideo = () => mapLoadVideoPlayer.TakeVideo();

        

        for (int i = 0; i< selectDirectionBtns.Count; i++) // 0번은 Nothing 들어갑니다.
        {
            int index = 1;

            for (int j = 0; j < i; j++)
                index <<= 1;
            Animator anim = selectDirectionBtns[i].GetComponentInChildren<Animator>();
            selectDirectionBtns[i].onClick.AddListener(() =>
            {
                isSelecting = false;
                gm.curSetBallCount++;
                anim.SetTrigger("OnClick");
                ScreenOn(false);

                int insertIndex = Mathf.Clamp(ballControllUI.order, 0, gm.maxBallCount);
                ballControllUI.ball.shootDir = (TileDirection)(index);

                ballControllUI.SetDirection(ballControllUI.ball.shootDir);
            });
        }
    }

    public override void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        isSelecting = on;
        DOTween.To(() => canvasGroup.alpha, a => canvasGroup.alpha = a, on ? 1 : 0, 0.75f).SetUpdate(true);

        myImage.rectTransform.DOSizeDelta(new Vector2(width, on ? height : 0), 0.75f);
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
