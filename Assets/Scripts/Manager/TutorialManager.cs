using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class TutorialManager : ManagerBase
{
    private List<CanvasGroup> tutoPanels = new List<CanvasGroup>();
    private TuroritalUI turoritalUI;
    private RectTransform arrowText;
    private GameManager gm;
    private UIManager um;
    private BallControllUI currentBallUI;
    private Canvas currentSelectedCanvas;
    private int ballCount = 0;
    //ManagerBase 구현하기
    public override void Init()
    {
        ballCount = 0;
    }


    public void Start()
    {
        um = IsometricManager.Instance.GetManager<UIManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        arrowText = GameObject.Find("Arrow").GetComponent<RectTransform>();


        turoritalUI = um.canvas[3].GetComponent<TuroritalUI>();

        turoritalUI.TutoPanels.ForEach(x => tutoPanels.Add(x.GetComponent<CanvasGroup>()));

        tutoPanels.ForEach(x => x.alpha = 0);

        ballCount = 0;
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
    }

    public IEnumerator StartTurotial()
    {
        um.canvas[3].interactable = true;
        um.canvas[3].blocksRaycasts = true;
        um.canvas[3].DOFade(1f, 1f);
        
        
        
        yield return null;
        SelectBall();
    }

    public void SelectBall()
    {
        tutoPanels.ForEach(x => x.alpha = 0);
        tutoPanels[0].alpha = 1;

        if (gm.ballUIList.Count <= ballCount)
        {
            Confirm();
        }
        else
        {
            arrowText.offsetMax = new Vector2(arrowText.offsetMax.x + 380 * ballCount, arrowText.offsetMax.y);
            currentBallUI = gm.ballUIList[ballCount];

            currentBallUI.gameObject.AddComponent<GraphicRaycaster>();
            currentBallUI.gameObject.AddComponent<Canvas>();
            currentSelectedCanvas = currentBallUI.gameObject.GetComponent<Canvas>();
            currentSelectedCanvas.overrideSorting = true;
            currentSelectedCanvas.sortingOrder = 210;
            currentBallUI.directionSetBtn.onClick.AddListener(ChooseDir);
        }
    }

    public void ChooseDir()
    {
        tutoPanels.ForEach(x => x.alpha = 0);
        tutoPanels[1].alpha = 1;
        
        Destroy(currentBallUI.GetComponent<GraphicRaycaster>());
        Destroy(currentBallUI.GetComponent<Canvas>());


        SelectDirectionUI selectDirectionUI = currentBallUI.transform.GetComponentInParent<BallSettingUI>().selectDirectionUI;
        selectDirectionUI.gameObject.AddComponent<GraphicRaycaster>();
        selectDirectionUI.gameObject.AddComponent<Canvas>();
        currentSelectedCanvas = selectDirectionUI.gameObject.GetComponent<Canvas>();
        currentSelectedCanvas.overrideSorting = true;
        currentSelectedCanvas.sortingOrder = 210;

        ballCount++;
        selectDirectionUI.selectDirectionBtns.ForEach(x => x.onClick.AddListener(SelectBall));

        tutoPanels[0].DOFade(0, 1f);
        tutoPanels[1].DOFade(1, 1f);
    }

    public void Confirm()
    {
        tutoPanels.ForEach(x => x.alpha = 0);
        tutoPanels[2].alpha = 1;

        
    }
}
