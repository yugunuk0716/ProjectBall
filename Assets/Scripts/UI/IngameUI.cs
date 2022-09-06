using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : UIBase
{
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;

    public Transform[] parentTrms; // 0 은 생성 위치, 1은 추가하면 이동할 위치
    [SerializeField] BallControllUI ballControllUIPrefab;

    public SelectDirectionUI selectDirectionUI;
    public bool isSelectingDirection = false;

    public override void Init()
    {
        GetCanvasGroup();

        selectDirectionUI.Init(() => isSelectingDirection = false);

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        gm.MakeNewBallUI += (ball, isAutoSet) =>
        {
            BallControllUI newBallControllUI = Instantiate(ballControllUIPrefab, parentTrms[0]);
            newBallControllUI.SetBallSprites(ball.uiSprite);
            bool isAdded = false;


            Button btn = newBallControllUI.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                if (isSelectingDirection) return;

                if (isAdded) // 다시 돌아오려는
                {
                    newBallControllUI.transform.SetParent(parentTrms[0]);
                    gm.myBallList.Remove(ball);
                    gm.ballUIList.Remove(newBallControllUI.gameObject);
                    newBallControllUI.SetDirection(TileDirection.RIGHTDOWN, false);
                }
                else // 추가 하려는
                {
                    newBallControllUI.transform.SetParent(parentTrms[1]);
                    gm.ballUIList.Add(newBallControllUI.gameObject);
                    selectDirectionUI.addBall = ball;
                    selectDirectionUI.ballControllUI = newBallControllUI;
                    selectDirectionUI.ScreenOn(true);
                    isSelectingDirection = true;
                }

                isAdded = !isAdded;

            });


            if (isAutoSet)
            {
                Debug.Log("오토 세팅");
                try
                {
                    Debug.Log("1");
                    isAdded = true;
                    newBallControllUI.transform.SetParent(parentTrms[1]);
                    gm.ballUIList.Add(newBallControllUI.gameObject);
                    gm.myBallList.Add(ball);
                    gm.lastBallList.Add(ball);
                    Debug.Log("2");
                }
                catch
                {
                    Debug.Log("3");
                }
            }
            else
            {
                Debug.Log("세팅 안함 초기화");
            }
        };


        try
        {
            gm.SetTimerText += (string textString, Color? color) => SetTimerText(textString, color);

            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            sm.SetDebugText += (string textString) => SetDebugText(textString);
            sm.FadeDebugText += () => FadeDebugText();

            sm.ClearBallUis += () =>
            {
                for (int i = 0; i < parentTrms.Length; i++)
                {
                    parentTrms[i].GetComponentsInChildren<Button>().ToList().ForEach((x) => Destroy(x.gameObject));
                }
            };
        }
        catch
        {
            Debug.Log("ㅇㅇ");
        }
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
        ClearAllBalls();
        isSelectingDirection = false;
    }

    public void ClearAllBalls()
    {
        PoolManager.Instance.gameObject.GetComponentsInChildren<Ball>().ToList().ForEach(x => x.gameObject.SetActive(false));
    }
}
