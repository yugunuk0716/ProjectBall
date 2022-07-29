using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class IngameUI : UIBase
{
    public TMP_InputField stageIndexInputField;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;
    public Button moveStageBtn;

    public Transform[] parentTrms; // 0 은 생성 위치, 1은 추가하면 이동할 위치
    [SerializeField] BallControllUI ballControllUIPrefab;    
    

    public override void Init()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.SetTimerText += (string textString, Color? color) => SetTimerText(textString, color);

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        sm.SetDebugText += (string textString) => SetDebugText(textString);
        sm.FadeDebugText += () => FadeDebugText();
        sm.InitBallControllUIs += (Ball[] Balls) =>
        {
            List<BallControllUI> btnList = new List<BallControllUI>();

            foreach(Ball item in Balls)
            {
                BallControllUI newBallControllUI = Instantiate(ballControllUIPrefab, parentTrms[0]);

                newBallControllUI.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if(newBallControllUI.isAdded) // 다시 돌아오려는
                    {
                        newBallControllUI.transform.parent = parentTrms[0];
                        gm.myBallList.Remove(newBallControllUI.myBall);
                        
                    }
                    else // 추가 하려는
                    {
                        newBallControllUI.transform.parent = parentTrms[1];
                        gm.myBallList.Add(newBallControllUI.myBall);
                    }
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
