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


    public override void Init()
    {
        IsometricManager.Instance.GetManager<GameManager>().SetTimerText += (string textString, Color? color) => SetTimerText(textString, color);

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        sm.SetDebugText += (string textString) => SetDebugText(textString);
        sm.FadeDebugText += () => FadeDebugText();

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
        timer_text.color = color ?? default(Color);
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


}
