using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public List<int> stageIndexList;
    private bool isScreenOn = false;
    
    StageManager sm;
    public List<IntListBox> allContents = new List<IntListBox>();

    public override void Init()
    {
        GetCanvasGroup();
        sm = IsometricManager.Instance.GetManager<StageManager>();

        stageOnBtn.onClick.AddListener(() =>
        {
            ScreenOn(!isScreenOn);
            isScreenOn = !isScreenOn;
        });
        allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);
       
    }

    public void UpdateButtonListener(IntListBox myBox, int lastIndex)
    {
   
        int index = lastIndex;
        bool canEnter = false;
        if (sm.clearMapCount + 1 >= index)
        {
            canEnter = true;

        }
        myBox.SetLock(!canEnter);
        Button myButton = myBox.GetComponentInChildren<Button>();
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() =>
        {
            print(lastIndex);
            print($"idx:{index}, cc: {sm.clearMapCount}");
            if (canEnter)
            {
                sm.LoadStage(index);
                ScreenOn(false);
                isScreenOn = false;
            }
        });
    }

   

    public override void Load()
    {
        
    }

}
