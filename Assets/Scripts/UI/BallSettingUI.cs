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
    [SerializeField] SelectDirectionUI selectDirectionUI;
    [SerializeField] RectTransform shootPanel; // 공 발사할 때 볼 패널!

    [Header("Button")]
    public Button confirmBtn;
    [SerializeField] Button shootBtn;
    [SerializeField] RectTransform targetPoint_ShootBtn;
    [SerializeField] SwapUI swapUi;

    public Action<bool> SwitchUI;  // 세팅에서 슛으로.

    public override void Init()
    {
        float ratio = 1f;

        if(Screen.width < 1080)
        {
            ratio = 1080f / (float)Screen.width;
        }

        shootPanel.anchoredPosition = new Vector3(Screen.width * ratio, shootPanel.anchoredPosition.y, 0);

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        gm.MakeNewBallUI = (ball, isAutoSet, index) =>
        {
            BallControllUI ballUI = PoolManager.Instance.Pop("BallControllUI") as BallControllUI;
            ballUI.transform.SetParent(ballContent);
            ballUI.transform.localPosition = new Vector3(0,0, 0); 
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
                if (isAdded) // 다시 돌아오려는
                {
                    ballUI.SetDirection(TileDirection.RIGHTDOWN, false);
                    gm.curSetBallCount--;
                }
                else // 추가 하려는
                {
                    selectDirectionUI.Set(ballUI);
                    selectDirectionUI.ScreenOn(true);
                    gm.curSetBallCount++;
                }

                isAdded = !isAdded;
            });
        };

        confirmBtn.onClick.AddListener(() =>
        {
            if (gm.maxBallCount != gm.curSetBallCount) return;

            gm.ballUIList.ForEach((x) =>
            {
                x.directionSetBtn.interactable = false;
                x.directionSetBtn.image.raycastTarget = false;
            });
            StartCoroutine(MoveBallUis(gm.ballUIList));
        });

        shootBtn.onClick.AddListener(() => gm.Shoot()); // 확인 버튼 누르면 슛 버튼에 함수 구독
    }

    public override void Load()
    {
        //혹시 슛 패널로 이동 안했으면 남아있을거니까!
        TargetPointUI[] arr = targetPointContent.GetComponentsInChildren<TargetPointUI>();
        for(int i = 1; i < arr.Length; i++)
        {
            PoolManager.Instance.Push(arr[i]);
        }

        MakeTargetPoints();
        GameManager.CanNotInteract = true;

        Sequence rollbackUISeq = DOTween.Sequence();
        rollbackUISeq.SetAutoKill(false);
        rollbackUISeq.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector3(-100, 100, 0), 0.5f).SetEase(Ease.OutCubic));
        rollbackUISeq.Join(shootBtn.transform.DORotate(new Vector3(0, 0, -360), 0.5f, RotateMode.LocalAxisAdd));
        rollbackUISeq.Join(shootBtn.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            SwitchUI(true);
            GameManager.CanNotInteract = false;
        }));

    }

    IEnumerator MoveBallUis(List<BallControllUI> list)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.ballUIList.Sort((x, y) => x.order.CompareTo(y.order));
        gm.ballUIList.ForEach((x) => gm.myBallList.Add(x.ball));
        gm.lastBallList = gm.myBallList;

        GameManager.CanNotInteract = true;
        shootBtn.interactable = false;

        SwitchUI(false);
        yield return new WaitForSeconds(1.3f);

        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // 걍 0번은 무시하고 가죠

        float duration = 0.2f;
        float minusDuration = duration / targetPoints.Length / 2;

        yield return null;

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(i);
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 0.4f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }

        yield return new WaitForSeconds(0.2f);

        List<BallControllUI> ballUiList = gm.ballUIList;
        foreach(var item in ballUiList)
        {
            TargetPointUI tp = item.transform.parent.GetComponent<TargetPointUI>();
            item.transform.SetParent(item.transform.parent.parent);
            PoolManager.Instance.Push(tp);
        }

        Sequence changeUISeq2 = DOTween.Sequence();
        changeUISeq2.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(targetPoint_ShootBtn.anchoredPosition, 0.8f).SetEase(Ease.OutCubic));
        changeUISeq2.Join(shootBtn.transform.DORotate(new Vector3(0, 0, 360), 0.8f, RotateMode.LocalAxisAdd));
        changeUISeq2.Join(shootBtn.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.8f).OnComplete(() =>
        {
            GameManager.CanNotInteract = false;
            shootBtn.interactable = true;
        }));
    }

    public void MakeTargetPoints()
    {
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        int count = sm.stageDataList[sm.stageIndex - 1].balls.Length;

        for (int i = 0; i < count; i++)
        {
            TargetPointUI obj = PoolManager.Instance.Pop("TargetPointUI") as TargetPointUI;
            obj.transform.SetParent(targetPointContent);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = Vector3.one;
        }
    }

    public override void Reset()
    {
        throw new NotImplementedException();
    }
}
