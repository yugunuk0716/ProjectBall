using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;

public class IngameUI : UIBase
{
    public TMP_InputField stageIndexInputField;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;
    public Button moveStageBtn;

    public Transform[] parentTrms; // 0 은 생성 위치, 1은 추가하면 이동할 위치
    [SerializeField] GameObject ballControllUIPrefab;

    public SelectDirectionUI selectDirectionUI;

    public override void Init()
    {
        GetCanvasGroup();
        selectDirectionUI.Init();
        
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
                Ball ball = balls[i];

                GameObject newBallControllUI = Instantiate(ballControllUIPrefab, parentTrms[0]);
                bool isAdded = false;

                newBallControllUI.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if(isAdded) // 다시 돌아오려는
                    {
                        newBallControllUI.transform.SetParent(parentTrms[0]);
                        gm.myBallList.Remove(ball);
                        gm.ballUIList.Remove(newBallControllUI);
                    }
                    else // 추가 하려는
                    {
                        newBallControllUI.transform.SetParent(parentTrms[1]);
                        gm.ballUIList.Add(newBallControllUI);
                        selectDirectionUI.addBall = ball;
                        selectDirectionUI.ScreenOn(true);
                    }

                    isAdded = !isAdded;
                });
            }
        };

        moveStageBtn.onClick.AddListener(() =>
        {
            sm.LoadStage();
            sm.ClearAllBalls();
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
        timer_text.text = textString;
    }

    public void FadeDebugText()
    {
        debugText.DOComplete();
        debugText.color = new Color(1, 0.5f, 0.5f, 1);
        debugText.DOFade(0, 2);
    }


}
