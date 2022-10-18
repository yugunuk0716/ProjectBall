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

    [Header("RetryBtn")]
    public Button retryBtn;

    private Vector3 big = new Vector3(1.2f, 1.2f, 1.2f);

    Sequence seq;
    float width;

    private GameManager gm;

    IEnumerator CoInit()
    {
        yield return null;
        width = transform.root.GetComponent<RectTransform>().sizeDelta.x;

        if (Screen.width > width)
        {
            width = Screen.width;
        }

        GetCanvasGroup();
        playUIs.ForEach((x) => x.Init());
        playUIs.ForEach((x) =>
        {
            if (x is BallSettingUI)
            {
                BallSettingUI ballSettingUI = x.GetComponent<BallSettingUI>();
                ballSettingUI.SwitchUI = (x) => SwitchUI(isSetPanelActive, x);
            }
        });

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        LifeManager lm = IsometricManager.Instance.GetManager<LifeManager>();
        UIManager um = IsometricManager.Instance.GetManager<UIManager>();

        retryBtn.onClick.AddListener(() =>
        {
            if (GameManager.canInteract && !sm.isMapLoading)
            {
                if (!lm.CanEnterStage())
                {
                    print("광고보기");
                    um.FindUI("WatchAddPanel").ScreenOn(true);
                    return;
                }

                lm.EnterStage();
                sm.LoadStage(sm.stageIndex);
            }
        });
    }

    private void Start()
    {
        IsometricManager.Instance.GetManager<StageManager>().retryBtn = retryBtn.gameObject;
    }

    public override void Init()
    {
        StartCoroutine(CoInit());
    }

    public override void Load()
    {
        playUIs.ForEach((x) => x.Load());

    }

    private void SwitchUI(bool moveLeft, bool isForLoad)
    {
        if (isForLoad && isSetPanelActive)
        {
            GameManager.canInteract = true;
            return;
        }

        if (!moveLeft)
        {
            MoveUI(shootPanel, settingPanel);
        }
        else     
        {
            MoveUI(settingPanel, shootPanel);
        }
    }

    public void MoveUI(RectTransform activedPanel, RectTransform activePanel)
    {
        GameManager.canInteract = false;
        if (gm == null)
        {
            gm = IsometricManager.Instance.GetManager<GameManager>();
        }

        int targetPos = isSetPanelActive ? (int)(-width) : (int)(width);
        int posX = activedPanel == settingPanel ? 100 : 0;
        seq = DOTween.Sequence();
        seq.Append(activedPanel.DOAnchorPosX(targetPos, 0.5f).SetEase(Ease.OutCubic));
        seq.Join(activePanel.DOAnchorPosX(posX, 0.4f).SetDelay(0.15f).SetEase(Ease.OutCubic).
            OnComplete(() =>
            {
                isSetPanelActive = !isSetPanelActive;
                gm.ballUIList.ForEach((x) => x.SetInteractValues(isSetPanelActive));
                if(isSetPanelActive)
                {
                    GameManager.canInteract = true;
                }
            }));

        seq.AppendInterval(2.5f).OnComplete(() =>
        {
            retryBtn.gameObject.SetActive(!isSetPanelActive);
        });
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

}
