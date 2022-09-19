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
    bool canEnter = false;
    
    int index;
    StageManager sm;
    public List<IntListBox> allContents = new List<IntListBox>();

    public override void Init()
    {
        GetCanvasGroup();
        sm = IsometricManager.Instance.GetManager<StageManager>();
        stageButtons = GetComponentsInChildren<Button>();
        allContents = GetComponentsInChildren<IntListBox>().ToList();
        IsometricManager.Instance.GetManager<GameManager>().UpdateUIContents += () => { allContents.ForEach(c => c.UpdateContent()); };

        stageOnBtn.onClick.AddListener(() =>
        {
            ScreenOn(!isScreenOn);
            isScreenOn = !isScreenOn;
            ASD();
        });

       
    }

    public void ASD()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int temp = i;
            IntListBox box = stageButtons[temp].GetComponentInParent<IntListBox>();
            index = 0;
            bool a = int.TryParse(box._contentText.text, out index);
            print(box._contentText.text);
            bool canEnter = false;
            if (sm.clearMapCount + 1 >= index)
            {
                canEnter = true;

            }
            box.SetLock(!canEnter);
            stageButtons[temp].onClick.AddListener(() =>
            {
                print($"temp {temp}, length {stageButtons.Length}");
                print($"idx:{index}, cc: {sm.clearMapCount}");
                if (canEnter)
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
