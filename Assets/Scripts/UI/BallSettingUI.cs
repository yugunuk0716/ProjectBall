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
                shootBtn.onClick.AddListener(() => gm.Shoot()); // 확인 버튼 누르면 슛 버튼에 함수 구독
                gm.ballUIList.ForEach((x) => x.directionSetBtn.interactable = false);
                SwitchUI(false);
                StartCoroutine(MoveBallUis(gm.ballUIList));
            }
        });

        
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
        RollbackShootUI?.Invoke();
        SwitchUI(true);
    }

    IEnumerator MoveBallUis(List<BallControllUI> list)
    {
        yield return new WaitForSeconds(2f);

        GameManager.CanNotInteract = true;
        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // 걍 0번은 무시하고 가죠

        float duration = 0.3f;
        float minusDuration = duration / targetPoints.Length * 0.7f;

        yield return null;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 0.8f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }

        Sequence rollbackUISeq = DOTween.Sequence();
        rollbackUISeq.SetAutoKill(false);
        rollbackUISeq.Append(shootIcon.DOAnchorPosX(300, 0.6f).SetEase(Ease.InQuart));
        rollbackUISeq.Join(setIcon.DOAnchorPosX(500, 0.5f).SetEase(Ease.InQuart).SetDelay(0.2f));

        rollbackUISeq.Append(shootPanel.DOAnchorPos(new Vector3(100,60,0), 0.6f).SetEase(Ease.InQuart));
        rollbackUISeq.PrependInterval(0.3f);
        rollbackUISeq.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(targetPoint_ShootBtn.anchoredPosition, 0.5f).SetEase(Ease.OutCubic));
        rollbackUISeq.Join(shootBtn.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.LocalAxisAdd));
        rollbackUISeq.Join(shootBtn.transform.DOScale(new Vector3(2, 2, 2), 0.5f).OnComplete(() => GameManager.CanNotInteract = false));

        RollbackShootUI = () => rollbackUISeq.PlayBackwards();
        
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
