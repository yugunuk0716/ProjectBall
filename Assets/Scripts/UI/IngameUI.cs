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

    /*
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

        //이거 나중에 고치삼 ㅇㅋ? ㅇㅋ?ㅋㅇ?ㅋㅇㅋ?ㅇㅋ?ㅇㅋ?ㅇㅋ?ㅇ
        yield return new WaitForSeconds(1f);
        GameManager.CanNotInteract = false;
    }
    */

    public override void Init()
    {
        GetCanvasGroup();

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

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

    }
}
