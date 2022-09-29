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

    public void Start()
    {
        ratioY = (float)Screen.height / 1920;
        moveDist = Screen.height / 19;

        TitleLogoMove();
        titleBtns[1].onClick.AddListener(ClickStartBtn);
        StartCoroutine(BtninteractableSet());
        UIManager um = IsometricManager.Instance.GetManager<UIManager>();
        titleBtns[0].onClick.AddListener(() => um.FindUI("TitleSettingPopUp").ScreenOn(true));
    }

    //titleLogo를 위아래로 왔다갔다 하게 하는 함수
    public void TitleLogoMove()
    {
        titleLogo.rectTransform.DOAnchorPosY(titleLogo.rectTransform.anchoredPosition.y + 100 * ratioY, 1).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    public void ClickStartBtn()
    {
        titleBtns[1].interactable = false;
        //TitleUi CanvasGroup Alpha값을 0으로 만들어서 사라지게 하고 InGameUI CanvasGroup Alpha값을 1로 만들어서 나타나게 함
        titleCanvasGroup.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            titleBtns[1].interactable = true;
        });
        ingameCanvasGroup.DOFade(1, 1f).SetUpdate(true);
    }

    public IEnumerator BtninteractableSet()
    {
        titleBtns.ForEach(x => x.interactable = false);
        yield return new WaitForSeconds(2f);
        titleBtns.ForEach(x => x.interactable = true);
    }
}
