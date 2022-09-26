using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngamePlayUIManager : UIBase
{
    [Header("Panel"), Space(10)] 
    [SerializeField] RectTransform settingPanel; // scroll value = 0
    [SerializeField] RectTransform shootPanel; // scroll value = 1

    [Header("Float"), Space(10)]
    [SerializeField] float swipeTime = 0.2f;

    [Header("UI About Ingame PlayUI"), Space(10)]
    [SerializeField] List<UIBase> playUIs = new List<UIBase>();
    private bool isSetPanelActive = true;

    private Vector3 big = new Vector3(1.2f, 1.2f, 1.2f);

    Sequence seq;

    private void SwitchUI(bool moveLeft, bool isForLoad) // 왼쪽으로 가나? 로딩할때 함수가 실행되는가?
    {
        if (isForLoad && isSetPanelActive) return;

        if (!moveLeft) // 슛 패널 켜기
        {
            MoveUI(shootPanel, settingPanel);
        }
        else        // 세팅 패널 켜기
        {
            MoveUI(settingPanel, shootPanel);
        }
    }

    public void MoveUI(RectTransform activedPanel, RectTransform activePanel)
    {
        GameManager.CanNotInteract = true;

        int targetPos = isSetPanelActive ? -Screen.width : Screen.width;
        int posX = activedPanel == settingPanel ? 100 : 0;
        seq = DOTween.Sequence();
        seq.Append(activedPanel.DOAnchorPosX(targetPos, 0.6f).SetEase(Ease.OutCubic));
        seq.Join(activePanel.GetComponent<RectTransform>().DOAnchorPosX(posX, 0.6f).SetDelay(0.2f).SetEase(Ease.OutBack).
            OnComplete(() =>
            {
                isSetPanelActive = !isSetPanelActive;
                GameManager.CanNotInteract = false;
            }));
    }

    public void BtnCloseUp(Image image1, Image image2)
    {
        image1.transform.DOScale(Vector3.one, swipeTime);
        image2.transform.DOScale(big, swipeTime);

        image1.transform.SetSiblingIndex(0);
        image2.transform.SetSiblingIndex(1);

        image1.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        image2.color = Color.white;
    }

    public override void Init()
    {
        playUIs.ForEach((x) => x.Init());
        playUIs.ForEach((x) =>
        {
            if(x is BallSettingUI)
            {
                BallSettingUI ballSettingUI = x.GetComponent<BallSettingUI>();
                ballSettingUI.SwitchUI = (x) => SwitchUI(isSetPanelActive, x);
            }
        });
    }

    public override void Load()
    {
        playUIs.ForEach((x) => x.Load());
    }

    public override void Reset()
    {
        throw new NotImplementedException();
    }
}
