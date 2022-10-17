using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirFishLab.ScrollingList.Demo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScrollUI : UIBase
{
    public Button closeBtn;
    public StageInfoUI stageInfoPanel;
    public List<int> stageIndexList;
    private bool isScreenOn = false;

    UIBase inGameUI;
    UIBase settingPanel;
    StageManager sm;
    LifeManager lm;
    UIManager um;
    public List<IntListBox> allContents = new List<IntListBox>();

    public TextMeshProUGUI heartCountText;
    public TextMeshProUGUI heartCoolText;

    public override void Init()
    {
        GetCanvasGroup();
        sm = IsometricManager.Instance.GetManager<StageManager>();
        lm = IsometricManager.Instance.GetManager<LifeManager>();
        um = IsometricManager.Instance.GetManager<UIManager>();
        settingPanel = IsometricManager.Instance.GetManager<UIManager>().FindUI("SettingPopUp");
      
        closeBtn.onClick.AddListener(() => ScreenOn(false));
        allContents.ForEach(c => c.UpdateContents += UpdateButtonListener);

        IsometricManager.Instance.GetManager<GameManager>().OnClear += (x, y) =>
        {
            allContents.ForEach(c => c.UpdateContent());
        };
     
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
    }


    public void UpdateHeartText(int count, string timer, bool isADSkip)
    {
        if (isADSkip)
        {
            heartCountText.text = "∞";
            heartCoolText.text = "∞";
            return;
        }

        heartCountText.text = $"{count}/5";
        heartCoolText.text = timer;
    }

    public override void Load()
    {
        
    }


}
