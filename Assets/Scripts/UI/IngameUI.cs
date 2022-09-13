using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngameUI : UIBase
{
    [Header("Text")]
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;

    [Space(10)]
    [Header("Content")]
    public Transform ballContent;
    [SerializeField] private Transform targetPointContent;

    [Space(10)]
    [Header("Prefab")]
    [SerializeField] BallControllUI ballControllUIPrefab;
    [SerializeField] GameObject targetPointObjPrefab;

    [SerializeField] private SelectDirectionUI selectDirectionUI;
    bool isSelectingDirection = false;

    [Space(10)]
    [Header("Button")]
    [SerializeField] private Button ballSettingConfirmBtn;
    [SerializeField] private Button shootBtn;

    [Space(10)]
    [Header("Panel")]
    [SerializeField] private RectTransform shootPanel;


    IEnumerator MoveBalls(List<BallControllUI> list)
    {
        Transform[] targetPoints = targetPointContent.GetComponentsInChildren<Transform>(); // 걍 0번은 무시하고 가죠

        float duration = 0.3f;
        float minusDuration = duration / targetPoints.Length / 2 ;


        for (int i = 0; i< list.Count; i++)
        {
            list[i].transform.SetParent(targetPoints[i + 1]);
            list[i].transform.DOMove(targetPoints[i + 1].position, 1f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(duration);
            duration -= minusDuration;
        }
    }

    int order = 0;

    public override void Init()
    {
        GetCanvasGroup();
        selectDirectionUI.Init(() => isSelectingDirection = false);

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

        RectTransform ballSettingUIRectTrm = ballContent.parent.parent.GetComponent<RectTransform>();


        ballSettingConfirmBtn.onClick.AddListener(() =>
        {
            if(gm.myBallList.Count < gm.maxBallCount || 0 >= gm.myBallList.Count)
            {
                return;
            }

            gm.isShooting = true;
            

            Sequence seq = DOTween.Sequence();
            seq.SetAutoKill(false);
            seq.Append(ballSettingUIRectTrm.DOAnchorPosX(ballSettingUIRectTrm.anchoredPosition.x - ballSettingUIRectTrm.rect.size.x * 1.5f, 1f).SetEase(Ease.InBack));
            seq.Append(shootPanel.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).SetDelay(0.3f).SetEase(Ease.OutBack)).OnComplete(() => StartCoroutine(MoveBalls(gm.ballUIList)));

            sm.ReuseUI = () => seq.PlayBackwards();

        });
        shootBtn.onClick.AddListener(() => gm.Shoot());

        gm.MakeNewBallUI += (ball, isAutoSet) =>
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
                if (isSelectingDirection || gm.isShooting)
                {
                    return;
                }

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
                    isSelectingDirection = true;
                }

                isAdded = !isAdded;
            });
        };

        gm.SetTimerText += (string textString, Color? color) => SetTimerText(textString, color);

        sm.SetDebugText += (string textString) => SetDebugText(textString);
        sm.FadeDebugText += () => FadeDebugText();
    }

    public void SetTimerText(string textString, Color? color = null)
    {
        timer_text.text = textString;
        if (color != null)
        {
            timer_text.color = color ?? default(Color);
        }
    }

    public void SetDebugText(string textString)
    {
        debugText.text = textString;
    }

    public void FadeDebugText()
    {
        debugText.DOComplete();
        debugText.color = new Color(1, 0.5f, 0.5f, 1);
        debugText.DOFade(0, 2);
    }

    public override void Load()
    {
        selectDirectionUI.ScreenOn(false);
        order = 0;
        isSelectingDirection = false;
        MakeTargetPoints();
    }

    public void MakeTargetPoints()
    {
        for(int i = 0; i< Resources.Load<StageDataSO>($"Stage {IsometricManager.Instance.GetManager<StageManager>().stageIndex}").balls.Length; i++)
        {
            Instantiate(targetPointObjPrefab, targetPointContent);
        }
    }
}
