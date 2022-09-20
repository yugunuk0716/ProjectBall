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

    int order = 0;

    [Header("Panel")]
    [SerializeField] private SelectDirectionUI selectDirectionUI;
    [SerializeField] private RectTransform shootPanel; // 공 발사할 때 볼 패널!

    [HideInInspector] public RectTransform setIcon;
    [HideInInspector] public RectTransform shootIcon;

    [Header("Button")]
    public Button confirmBtn;
    [SerializeField] Button shootBtn;

    public Action<bool> SwitchUI; // 세팅에서 슛으로.
    public Action RollbackShootUI; // 변화했던 UI 돌려놓기.

    [SerializeField] RectTransform targetPoint_ShootBtn;

    public override void Init()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.MakeNewBallUI = (ball, isAutoSet) =>
        {
            BallControllUI newBallControllUI = PoolManager.Instance.Pop("BallControllUI") as BallControllUI;
            newBallControllUI.transform.SetParent(ballContent);
            // 어차피 Horizontal LayoutGroup이 처리함, Z값 이상해서 0으로 처리해봄
            newBallControllUI.transform.localPosition = new Vector3(0,0, 0); 
            newBallControllUI.transform.localScale = Vector3.one;
            newBallControllUI.SetBallSprites(ball.uiSprite);
            gm.ballUIList.Add(newBallControllUI);
            newBallControllUI.order = 1000; // 정렬 안되도록

            bool isAdded = false;

            if (isAutoSet)
            {
                order++;
                isAdded = true;
                gm.myBallList.Add(ball);
                newBallControllUI.SetDirection(ball.shootDir);
            }

            

            newBallControllUI.directionSetBtn.onClick.AddListener(() =>
            {
                if (isAdded) // 다시 돌아오려는
                {
                    order--;

                    newBallControllUI.order = 1000;
                    newBallControllUI.SetDirection(TileDirection.RIGHTDOWN, false);

                    gm.myBallList.Remove(ball);
                    gm.BallUiSort();
                }
                else // 추가 하려는
                {
                    order++;
                    newBallControllUI.order = order;
                    selectDirectionUI.Set(ball, newBallControllUI, order);
                    selectDirectionUI.ScreenOn(true);
                }

                isAdded = !isAdded;
            });
        };

        confirmBtn.onClick.AddListener(() =>
        {
            if (false == GameManager.CanNotInteract && gm.myBallList.Count >= gm.maxBallCount)
            {

                gm.ballUIList.ForEach((x) => x.directionSetBtn.interactable = false);
                StartCoroutine(MoveBallUis(gm.ballUIList));
            }
        });

        shootBtn.onClick.AddListener(() => gm.Shoot()); // 확인 버튼 누르면 슛 버튼에 함수 구독
    }

    public override void Load()
    {
        order = 0;
        TargetPointUI[] arr = targetPointContent.GetComponentsInChildren<TargetPointUI>();
        for(int i = 1; i < arr.Length; i++)
        {
            PoolManager.Instance.Push(arr[i]);
        }

        MakeTargetPoints();
        GameManager.CanNotInteract = true;
        Sequence rollbackUISeq = DOTween.Sequence();
        rollbackUISeq.SetAutoKill(false);
        rollbackUISeq.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector3(-150, 170, 0), 0.5f).SetEase(Ease.OutCubic));
        rollbackUISeq.Join(shootBtn.transform.DORotate(new Vector3(0, 0, -720), 0.5f, RotateMode.LocalAxisAdd));
        rollbackUISeq.Join(shootBtn.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f).OnComplete(() => SwitchUI(true)));

        rollbackUISeq.Append(setIcon.DOAnchorPosX(-250, 0.6f).SetEase(Ease.InQuart));
        rollbackUISeq.Join(shootIcon.DOAnchorPosX(-50, 0.6f).SetEase(Ease.InQuart).SetDelay(0.2f).OnComplete(() => GameManager.CanNotInteract = false));
    }

    IEnumerator MoveBallUis(List<BallControllUI> list)
    {
        GameManager.CanNotInteract = true;
        shootBtn.interactable = false;

        Sequence changeUISeq = DOTween.Sequence();
        changeUISeq.Append(shootIcon.DOAnchorPosX(300, 0.4f).SetEase(Ease.InQuart));
        changeUISeq.Join(setIcon.DOAnchorPosX(500, 0.3f).SetEase(Ease.InQuart).SetDelay(0.1f).OnComplete(() => SwitchUI(false)));

        yield return new WaitForSeconds(1.3f);

        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // 걍 0번은 무시하고 가죠

        float duration = 0.2f;
        float minusDuration = duration / targetPoints.Length / 2;

        yield return null;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 0.4f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }

        yield return new WaitForSeconds(0.2f);

        Sequence changeUISeq2 = DOTween.Sequence();
        changeUISeq2.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(targetPoint_ShootBtn.anchoredPosition, 0.8f).SetEase(Ease.OutCubic));
        changeUISeq2.Join(shootBtn.transform.DORotate(new Vector3(0, 0, 720), 0.8f, RotateMode.LocalAxisAdd));
        changeUISeq2.Join(shootBtn.transform.DOScale(new Vector3(2, 2, 2), 0.8f).OnComplete(() =>
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
