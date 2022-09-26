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
    private GameManager gm;
    private UIManager um;
    private BallControllUI currentBallUI;
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

        turoritalUI = um.canvas[3].GetComponent<TuroritalUI>();

        turoritalUI.TutoPanels.ForEach(x => tutoPanels.Add(x.GetComponent<CanvasGroup>()));
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
        currentBallUI = gm.ballUIList[ballCount];
        Canvas c;
        currentBallUI.gameObject.AddComponent<GraphicRaycaster>();
        currentBallUI.gameObject.AddComponent<Canvas>();
        c = currentBallUI.gameObject.GetComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 210;
        currentBallUI.directionSetBtn.onClick.AddListener(ChooseDir);
    }

    public void ChooseDir()
    {
        Destroy(currentBallUI.GetComponent<GraphicRaycaster>());
        Destroy(currentBallUI.GetComponent<Canvas>());
        tutoPanels[0].DOFade(0, 1f);
        tutoPanels[1].DOFade(1, 1f);
    }
}
