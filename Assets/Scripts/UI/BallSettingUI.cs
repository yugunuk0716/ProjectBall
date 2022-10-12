using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSettingUI : UIBase
{
    [Space(10)]
    [Header("Content")]
    public Transform ballContent;
    [SerializeField] private Transform targetPointContent;

    [Header("Panel")]
    public SelectDirectionUI selectDirectionUI;
    [SerializeField] RectTransform shootPanel; // ball panel at shoot Time

    [Header("Button")]
    public Button confirmBtn;
    [SerializeField] Button shootBtn;
    [SerializeField] SwapUI swapUi;

    public Action<bool> SwitchUI;  // seting to shoot.

    private UIManager um;

    float width = 0f;
    float heightDevideFive;

    IEnumerator CoInit()
    {
        yield return null;
        width = transform.root.GetComponent<RectTransform>().sizeDelta.x;

        if (Screen.width > width)
        {
            width = Screen.width;
        }

        heightDevideFive = 350;

        shootPanel.anchoredPosition = new Vector3(width, shootPanel.anchoredPosition.y, 0);

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        um = IsometricManager.Instance.GetManager<UIManager>();
        gm.ActiveGameOverPanel += (x) => shootBtn.interactable = false;
        gm.MakeNewBallUI = (ball, isAutoSet, index) =>
        {
            BallControllUI ballUI = GameObjectPoolManager.Instance.GetGameObject("UIs/BallControllUI", GameObjectPoolManager.Instance.transform).GetComponent<BallControllUI>();
            ballUI.transform.SetParent(ballContent);
            ballUI.transform.localPosition = new Vector3(0, 0, 0);
            ballUI.transform.localScale = Vector3.one;
            ballUI.order = index;
            ballUI.swapUI = swapUi;
            ballUI.ball = ball;

            gm.ballUIList.Add(ballUI);

            bool isAdded = false;

            if (isAutoSet)
            {
                isAdded = true;
                gm.curSetBallCount++;
                ballUI.SetDirection(ballUI.ball.shootDir);
            }

            ballUI.directionSetBtn.onClick.RemoveAllListeners();
            ballUI.directionSetBtn.onClick.AddListener(() =>
            {
                if (Input.touchCount > 1) return;

                if (isAdded) // return
                {
                    ballUI.SetDirection(TileDirection.RIGHTDOWN, false);
                    gm.curSetBallCount--;
                }
                else // add
                {
                    if (selectDirectionUI.ballControllUI == null)
                    {
                        selectDirectionUI.Set(ballUI);
                        selectDirectionUI.ScreenOn(true);
                    }
                }

                isAdded = !isAdded;
            });
        };
        confirmBtn.onClick.AddListener(() =>
        {
            if (!GameManager.canInteract || selectDirectionUI.isSelecting) return;



            if (Input.touchCount > 1) return;

            if (gm.maxBallCount != gm.curSetBallCount)
            {
                um.FindUI("CantEnterPanel").ScreenOn(true);
                return;
            }

            gm.ballUIList.ForEach((x) =>
            {
                x.directionSetBtn.interactable = false;
                x.directionSetBtn.image.raycastTarget = false;
            });
            StartCoroutine(MoveBallUis(gm.ballUIList));
        });
        shootBtn.onClick.AddListener(() => gm.Shoot()); // press confirm button, add listner shoot;
    }

    public override void Init()
    {
        StartCoroutine(CoInit());
    }

    public override void Load()
    {
        
        TargetPointUI[] arr = targetPointContent.GetComponentsInChildren<TargetPointUI>();
        for(int i = 0; i < arr.Length; i++)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(arr[i].gameObject);
        }

        MakeTargetPoints();
        shootBtn.interactable = false;
        GameManager.canInteract = false;

        Sequence rollbackUISeq = DOTween.Sequence();
        rollbackUISeq.SetAutoKill(false);
        rollbackUISeq.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPosY(-heightDevideFive, 0.5f).SetEase(Ease.OutCubic));
        rollbackUISeq.Join(shootBtn.transform.DORotate(new Vector3(0, 0, -720), 0.5f, RotateMode.LocalAxisAdd));
        rollbackUISeq.Join(shootBtn.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            SwitchUI(true);
            GameManager.canInteract = true;
        }));

    }

    IEnumerator MoveBallUis(List<BallControllUI> list)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.lastBallList.Clear();

        gm.ballUIList.Sort((x, y) => x.order.CompareTo(y.order));
        gm.ballUIList.ForEach((x) =>
        {
            x.isTutoOrShooting = true;
            gm.lastBallList.Add(x.ball.shootDir);
        });

        shootBtn.interactable = false;
        SwitchUI(false);
        yield return new WaitForSeconds(0.8f);

        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // skip zero

        float duration = 0.16f;
        float minusDuration = duration / targetPoints.Length / 2;

        yield return null;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 0.35f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }

        yield return new WaitForSeconds(0.2f);

        List<BallControllUI> ballUiList = gm.ballUIList;
        ballUiList.ForEach((x) => x.SetInteractValues(false));
        foreach(var item in ballUiList)
        {
            TargetPointUI tp = item.transform.parent.GetComponent<TargetPointUI>();
            item.transform.SetParent(item.transform.parent.parent);
            if(tp != null)
            {
                GameObjectPoolManager.Instance.UnusedGameObject(tp.gameObject);
            }
        }

        GameManager.canInteract = false;
        Sequence changeUISeq2 = DOTween.Sequence();
        changeUISeq2.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPosY(heightDevideFive, 0.8f).SetEase(Ease.OutCubic));
        changeUISeq2.Join(shootBtn.transform.DORotate(new Vector3(0, 0, 720), 0.8f, RotateMode.LocalAxisAdd));
        changeUISeq2.Join(shootBtn.transform.DOScale(new Vector3(1.5f, 1.5f, 0.8f), 0.8f).OnComplete(() =>
        {
            shootBtn.interactable = true;
            GameManager.canInteract = true;
        }));
    }

    public void MakeTargetPoints()
    {
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        int count = sm.stageDataList[sm.stageIndex - 1].balls.Length;

        for (int i = 0; i < count; i++)
        {
            TargetPointUI obj = GameObjectPoolManager.Instance.GetGameObject("UIs/TargetPointUI", GameObjectPoolManager.Instance.transform).GetComponent<TargetPointUI>();
            obj.transform.SetParent(targetPointContent);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = Vector3.one;
        }
    }

}
