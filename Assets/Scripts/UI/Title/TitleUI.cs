using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TitleUI : MonoBehaviour
{
    [SerializeField] CanvasGroup ingameCanvasGroup;
    [SerializeField] CanvasGroup titleCanvasGroup;
    [SerializeField] Image titleLogo;
    public List<Button> titleBtns;
    public float ratioY = 0f;
    public float moveDist = 0f;

    private LifeManager lm;
    private UIManager um;
    TileHelpUI tileHelp;
    public void Start()
    {
        ratioY = (float)Screen.height / 1920;
        moveDist = Screen.height / 19;

        TitleLogoMove();
        titleBtns[1].onClick.AddListener(ClickStartBtn);
        StartCoroutine(BtninteractableSet());
        um = IsometricManager.Instance.GetManager<UIManager>();
        lm = IsometricManager.Instance.GetManager<LifeManager>();
       
        titleBtns[0].onClick.AddListener(() =>
        {
            if (Input.touchCount > 1) return;
                
            um.FindUI("TitleSettingPopUp").ScreenOn(true);
        });

         
    }


    public void TitleLogoMove()
    {
        titleLogo.rectTransform.DOAnchorPosY(titleLogo.rectTransform.anchoredPosition.y + 100 * ratioY, 1).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    public void ClickStartBtn()
    {
        if (Input.touchCount > 1) return;

        if (!lm.CanEnterStage())
        {
            print("광고보기");
            um.FindUI("WatchAddPanel").ScreenOn(true);
            return;
        }
        if(tileHelp == null)
        {
            tileHelp = IsometricManager.Instance.GetManager<UIManager>().FindUI("HelpPanel").GetComponent<TileHelpUI>();
        }

        lm.EnterStage();
        tileHelp.MoveUI(false);

        titleBtns[1].interactable = false;
        
        titleCanvasGroup.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            titleBtns[1].interactable = true;
        });
        ingameCanvasGroup.DOFade(1, 1f).SetUpdate(true).OnComplete(() =>
        {
            ingameCanvasGroup.interactable = true;
            ingameCanvasGroup.blocksRaycasts = true;
        });
    }

    public IEnumerator BtninteractableSet()
    {
        titleBtns.ForEach(x => x.interactable = false);
        yield return new WaitForSeconds(2f);
        titleBtns.ForEach(x => x.interactable = true);
    }
}
