using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StageInfoUI : UIBase
{
    public TextMeshProUGUI stageIndexText;
    public Button enterButton;
    public Button cancleButton;
    public Image[] starImages;

    private StageManager sm;
    private LifeManager lm;


    public override void Init()
    {
       
    }

    public override void Load()
    {
        
    }



    // Start is called before the first frame update
    public void ScreenOn(bool on, int stageIndex, StageScrollUI ssUI)
    {
        if(canvasGroup == null)
        {
            GetCanvasGroup();
            cancleButton.onClick.AddListener(() => ScreenOn(false));
            sm = IsometricManager.Instance.GetManager<StageManager>();
        }
        for (int i = 0; i < 3; i++)
        {
            starImages[i].gameObject.SetActive(false);
        }

        enterButton.onClick.RemoveAllListeners();
        enterButton.onClick.AddListener(() =>
        {
            if (lm == null) 
            {
                lm = IsometricManager.Instance.GetManager<LifeManager>();
            }

            if (!lm.CanEnterStage()) 
            {
                return;
            }

            sm.LoadStage(stageIndex);
            ScreenOn(false);
            ssUI.ScreenOn(false);

        });
        stageIndexText.SetText(stageIndex.ToString());
        int starCount = sm.GetStar(stageIndex - 1);

        for (int i = 0; i < starCount; i++)
        {
            starImages[i].gameObject.SetActive(true);
        }

        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        DOTween.To(() => canvasGroup.alpha, a => canvasGroup.alpha = a, on ? 1 : 0, 0.5f).SetUpdate(true);
    }

  
}
