using DG.Tweening;
using TMPro;
using UnityEngine;

public class IngameUI : UIBase
{
    [Header("Text")]
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI timer_text;



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
        if (textString.Equals("on"))
        {
            timer_text.DOFade(1f, 0.5f);
        }
        else if(textString.Equals("off"))
        {
            timer_text.DOFade(0f, 0.5f);
        }
        else
        {
            timer_text.text = textString;
            if (color != null)
            {
                timer_text.color = color ?? default(Color);
            }
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

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
