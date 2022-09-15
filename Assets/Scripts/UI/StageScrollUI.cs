using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public Button[] stageButtons;

    private bool isScreenOn = false;

    public List<IntListBox> allContents = new List<IntListBox>();

    public override void Init()
    {
        GetCanvasGroup();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        stageButtons = GetComponentsInChildren<Button>();
        allContents = GetComponentsInChildren<IntListBox>().ToList();
        IsometricManager.Instance.GetManager<GameManager>().UpdateUIContents += () => { allContents.ForEach(c => c.UpdateContent()); };

        stageOnBtn.onClick.AddListener(() => { ScreenOn(!isScreenOn); isScreenOn = !isScreenOn; });

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int temp = i;
            stageButtons[temp].onClick.AddListener(() =>
            {
                print($"temp {temp}, length {stageButtons.Length}");
                int index = int.Parse(stageButtons[temp].GetComponentInParent<IntListBox>()._contentText.text);
                print($"idx:{index}, cc: {sm.clearMapCount}");
                if (sm.clearMapCount + 1 >= index)
                {
                    sm.LoadStage(index);
                    ScreenOn(false);
                    isScreenOn = false;
                }
            });
        }
    }

    public override void Load()
    {
        
    }

}
