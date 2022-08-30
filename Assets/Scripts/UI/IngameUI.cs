using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : UIBase
{
    public TMP_InputField stageIndexInputField;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;
    public Button moveStageBtn;

    public Transform[] parentTrms; // 0 은 생성 위치, 1은 추가하면 이동할 위치
    [SerializeField] BallControllUI ballControllUIPrefab;

    public SelectDirectionUI selectDirectionUI;

    public bool isSelectingDirection = false;

    [SerializeField] private Sprite[] abilitySprites;


    public override void Init()
    {
        Debug.Log("InitCall");

        GetCanvasGroup();

        selectDirectionUI.Init(() =>
        {
            isSelectingDirection = false;
        });
        
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.SetTimerText += (string textString, Color? color) => SetTimerText(textString, color);

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        sm.SetDebugText += (string textString) => SetDebugText(textString);
        sm.FadeDebugText += () => FadeDebugText();
        sm.InitBallControllUIs += (Ball[] balls) =>
        {
            gm.maxBallCount = balls.Length;

            for(int i = 0; i < parentTrms.Length; i++) parentTrms[i].GetComponentsInChildren<Button>().ToList().ForEach((x) => Destroy(x.gameObject));

            for(int i = 0; i< balls.Length; i++)
            {
                Ball ball = null;
                if (balls[i].ballState != BallState.None)
                {
                    ball = PoolManager.Instance.Pop($"{balls[i].ballState}{balls[i].collisionTileType}") as Ball;
                }
                else
                {
                    ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
                }

                BallControllUI newBallControllUI = Instantiate(ballControllUIPrefab, parentTrms[0]);
                newBallControllUI.SetBallSprites(ball.uiSprite, abilitySprites[(int)ball.ballState]);
                bool isAdded = false;

                newBallControllUI.GetComponent<Button>().onClick.AddListener(() =>
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
            }
        };

        moveStageBtn.onClick.AddListener(() =>
        {
            sm.LoadStage(gm.mapRangeStrArray[sm.stageIndex - 1]);
        });
        stageIndexInputField.onValueChanged.AddListener(sm.SetStageIndex);
    }
    
    public void SetTimerText(string textString, Color? color = null)
    {
        timer_text.text = textString;
        if(color != null)
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
