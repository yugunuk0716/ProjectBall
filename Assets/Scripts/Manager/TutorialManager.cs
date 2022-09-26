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
    //ManagerBase 구현하기
    public override void Init()
    {
        turoritalUI = IsometricManager.Instance.GetManager<UIManager>().canvas[3].GetComponent<TuroritalUI>();
        
        tutoPanels = turoritalUI.TutoPanels.ToList();
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
    }

    public void StartTurotial()
    {
        IsometricManager.Instance.GetManager<UIManager>().canvas[3].DOFade(1f, 1f);
    }
}
