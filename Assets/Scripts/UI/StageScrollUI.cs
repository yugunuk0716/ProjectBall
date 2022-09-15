using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public Button[] stageButtons;

    public override void Init()
    {
        GetCanvasGroup();
        stageButtons = GetComponentsInChildren<Button>();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

        stageOnBtn.onClick.AddListener(() => { ScreenOn(true); });

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int temp = i;
            stageButtons[temp].onClick.AddListener(() =>
            {
                sm.LoadStage(Resources.Load<StageDataSO>($"Stage {int.Parse(stageButtons[temp].GetComponent<IntListBox>()._contentText.text) + 1}"));
                ScreenOn(false);
            });
        }
    }

    public override void Load()
    {
        
    }

}
