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

    [Space(10)]
    [Header("Prefab")]
    [SerializeField] BallControllUI ballControllUIPrefab;
    [SerializeField] GameObject targetPointObjPrefab;

    [Header("Panel")]
    [SerializeField] private SelectDirectionUI selectDirectionUI;
    [SerializeField] private RectTransform shootPanel; // 공 발사할 때 볼 패널!

    [HideInInspector] public RectTransform setOnBtn;
    [HideInInspector] public RectTransform shootOnBtn;

    [Header("Button")]
    public Button confirmBtn;
    [SerializeField] Button shootBtn;

    public Action SwitchUI; // 세팅에서 슛으로.
    public Action RollbackShootUI; // 변화했던 UI 돌려놓기.

    [SerializeField] RectTransform targetPoint_ShootBtn;

    public override void Init()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.MakeNewBallUI = (ball, isAutoSet) =>
        {
            BallControllUI newBallControllUI = Instantiate(ballControllUIPrefab, ballContent);
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
                SwitchUI();
                StartCoroutine(MoveBallUis(gm.ballUIList));
            }
        });

        shootBtn.onClick.AddListener(() => gm.Shoot());
    }

    public override void Load()
    {
        order = 0;
        Transform []arr = targetPointContent.GetComponentsInChildren<Transform>();
        for(int i = 1; i < arr.Length; i++)
        {
            Destroy(arr[i].gameObject);
        }

        MakeTargetPoints();
        RollbackShootUI?.Invoke();
    }

    IEnumerator MoveBallUis(List<BallControllUI> list)
    {
        yield return new WaitForSeconds(2f);

        GameManager.CanNotInteract = true;
        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // 걍 0번은 무시하고 가죠

        float duration = 0.3f;
        float minusDuration = duration / targetPoints.Length / 2;

        yield return null;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 1f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }

        yield return new WaitForSeconds(0.3f);

        Sequence rollbackUISeq = DOTween.Sequence();
        rollbackUISeq.SetAutoKill(false);
        rollbackUISeq.Append(shootOnBtn.DOAnchorPosX(300, 0.6f).SetEase(Ease.InQuart));
        rollbackUISeq.Join(setOnBtn.DOAnchorPosX(500, 0.5f).SetEase(Ease.InQuart).SetDelay(0.2f));

        rollbackUISeq.Append(shootPanel.DOAnchorPosY(-50, 0.6f).SetEase(Ease.InQuart));

        rollbackUISeq.Append(shootBtn.GetComponent<RectTransform>().DOAnchorPos(targetPoint_ShootBtn.anchoredPosition, 0.7f).SetEase(Ease.OutCubic));
        rollbackUISeq.Join(shootBtn.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.LocalAxisAdd));
        rollbackUISeq.Join(shootBtn.transform.DOScale(new Vector3(2, 2, 2), 1f).OnComplete(() => GameManager.CanNotInteract = false));

        RollbackShootUI = () => rollbackUISeq.PlayBackwards();
        
    }

    public void MakeTargetPoints()
    {
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        int count = sm.stageDataList[sm.stageIndex - 1].balls.Length;

        for (int i = 0; i < count; i++)
        {
            Instantiate(targetPointObjPrefab, targetPointContent);
        }
    }

}
