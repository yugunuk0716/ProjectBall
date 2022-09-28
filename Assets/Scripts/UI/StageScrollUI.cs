using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList.Demo;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button stageOnBtn;
    public Button closeBtn;
    public StageInfoUI stageInfoPanel;
    public List<int> stageIndexList;
    private bool isScreenOn = false;

    UIBase inGameUI;
    UIBase settingPanel;
    StageManager sm;
    public List<IntListBox> allContents = new List<IntListBox>();

    public override void Init()
    {
        GetCanvasGroup();
        sm = IsometricManager.Instance.GetManager<StageManager>();
        settingPanel = IsometricManager.Instance.GetManager<UIManager>().FindUI("SettingPopUp");
      
        closeBtn.onClick.AddListener(() => ScreenOn(false));
        allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);

        IsometricManager.Instance.GetManager<GameManager>().OnClear += (x) =>
        {
            allContents.ForEach(c => c.UpdateContent());
           
        };

 
     
    }

    public void SetButton()
    {
       
        if (!GameManager.CanNotInteract)
        {
            ScreenOn(true);
            //isScreenOn = !isScreenOn;
            allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);
           
        }
    }


    public void CheckBackButton()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ScreenOn(false);
            }
        }
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
            //print($"cc: {sm.clearMapCount + 1}  idx: {index}");
            if (canEnter)
            {
                stageInfoPanel.ScreenOn(true, lastIndex, this);
                sm.stageIndex = index;
                isScreenOn = false;
                settingPanel.ScreenOn(false);
            }
        });
    }

    public override void ScreenOn(bool on)
    {
        if (inGameUI == null)
        {
            inGameUI = IsometricManager.Instance.GetManager<UIManager>().FindUI("InGamePlayUIManager");
        }
        inGameUI.ScreenOn(!on);
        base.ScreenOn(on);
        Time.timeScale = on ? 0 : 1;
    }

    public void UpdateHaptic()
    {
        //Vibration.Vibrate(5);
    }

    public override void Load()
    {
        
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
