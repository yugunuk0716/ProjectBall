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
    public float ratio = 0f;
    public float moveDist = 0f;

    public void Start()
    {
        float ratioX = (float)Screen.width / 1080;
        float ratioY = (float)Screen.height / 1920;

        ratio = (ratioX + ratioY) / 2;

        if (Screen.width > 1080 || Screen.height > 1920)
        {
            ratio /= 2;
        }
        titleBtns[0].transform.GetComponentInParent<HorizontalLayoutGroup>().spacing = 125 * ratio;
        titleLogo.rectTransform.anchoredPosition = new Vector2(0, 100 * ratio);

        ratio = Mathf.Clamp(ratio, 0.7f, 1f);


        moveDist = Screen.height / 19;
        titleLogo.transform.localScale = new Vector2(ratio, ratio);
        titleBtns.ForEach((x) => x.transform.localScale = new Vector2(ratio, ratio));


        TitleLogoMove();
        titleBtns[1].onClick.AddListener(ClickStartBtn);
        StartCoroutine(BtninteractableSet());
    }

    //titleLogo를 위아래로 왔다갔다 하게 하는 함수
    public void TitleLogoMove()
    {
        titleLogo.rectTransform.DOAnchorPosY(titleLogo.rectTransform.anchoredPosition.y + 100 * ratio, 1).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
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
