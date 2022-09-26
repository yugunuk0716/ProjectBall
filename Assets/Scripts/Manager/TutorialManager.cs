using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class TutorialManager : ManagerBase
{
    private List<Image> tutoPanels = new List<Image>();
    private TuroritalUI turoritalUI;
    private GameManager gm;
    private UIManager um;
    private int ballCount = 0;
    //ManagerBase 구현하기
    public override void Init()
    {
    }


    public void Start()
    {
        um = IsometricManager.Instance.GetManager<UIManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();

        turoritalUI = um.canvas[3].GetComponent<TuroritalUI>();

        tutoPanels = turoritalUI.TutoPanels.ToList();
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

        um.canvas[3].DOFade(1f, 1f);

        Canvas c;
        GameObject ingameUi = um.uis[3].gameObject;
        ingameUi.AddComponent<GraphicRaycaster>();
        ingameUi.AddComponent<Canvas>();
        c = ingameUi.GetComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 210;
        
        
        yield return null;
        SelectBall();
    }

    public void SelectBall()
    {

        gm.ballUIList.ForEach(x =>
        {
            x.directionSetBtn.onClick.AddListener(ChooseDir);
        });
    }

    public void ChooseDir()
    {
        
    }
}
