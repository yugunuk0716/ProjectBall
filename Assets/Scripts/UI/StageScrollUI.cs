using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public Button[] stageButtons;
    public int[] btnInteractCount;

    public override void Init()
    {
        GetCanvasGroup();
        stageButtons = GetComponentsInChildren<Button>();
        btnInteractCount = new int[stageButtons.Length];
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

        stageOnBtn.onClick.AddListener(() => { ScreenOn(true); });

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int temp = i;
            stageButtons[temp].onClick.AddListener(() =>
            {

                //if(btnInteractCount[temp] < 1)
                //{
                //    btnInteractCount[temp] += 1;
                //    return;
                //}

                //btnInteractCount[temp] = 0;
                print(int.Parse(stageButtons[temp].GetComponent<IntListBox>()._contentText.text));
                sm.LoadStage(Resources.Load<StageDataSO>($"Stage {int.Parse(stageButtons[temp].GetComponent<IntListBox>()._contentText.text) + 1}"));
                ScreenOn(false);
            });
        }
    }

    public override void Load()
    {
        
    }

}
