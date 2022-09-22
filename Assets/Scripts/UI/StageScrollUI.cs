using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public StageInfoUI stageInfoPanel;
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
            if(!GameManager.CanNotInteract)
            {
                ScreenOn(!isScreenOn);
                isScreenOn = !isScreenOn;
                allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);
            }

        });
        allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);

        IsometricManager.Instance.GetManager<GameManager>().OnClear += (x) => { allContents.ForEach(c => c.UpdateContent()); };

      //  ScreenOn(true);
        //isScreenOn = true;
     
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
            print($"cc: {sm.clearMapCount + 1}  idx: {index}");
            if (canEnter)
            {
                stageInfoPanel.ScreenOn(true, lastIndex, this);
                sm.stageIndex = index;
                isScreenOn = false;
            }
        });
    }

    public override void ScreenOn(bool on)
    {
        base.ScreenOn(on);
        Time.timeScale = on ? 0 : 1;
    }

    public void UpdateHaptic()
    {
        Vibration.Vibrate(5);
    }

    public override void Load()
    {
        
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
