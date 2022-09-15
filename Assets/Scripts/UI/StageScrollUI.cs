using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public Button[] stageButtons;

    private bool isScreenOn = false;

    public override void Init()
    {
        GetCanvasGroup();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        stageButtons = GetComponentsInChildren<Button>();

        stageOnBtn.onClick.AddListener(() => { ScreenOn(!isScreenOn); isScreenOn = !isScreenOn; });

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int temp = i;
            stageButtons[temp].onClick.AddListener(() =>
            {
                sm.LoadStage(int.Parse(stageButtons[temp].GetComponent<IntListBox>()._contentText.text) + 1);
                ScreenOn(false);
                isScreenOn = false;
            });
        }
    }

    public override void Load()
    {
        
    }

}
