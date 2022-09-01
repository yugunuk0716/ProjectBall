using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StageManager : ManagerBase
{
    public int stageIndex = 1;
    
    public List<ObjectTile> objectTileList = new List<ObjectTile>();
    public List<GameObject> stageObjList = new List<GameObject>();

    public Action<string> SetDebugText;
    public Action FadeDebugText;
    public Action<Ball[]> InitBallControllUIs;

    private string currentMapRange;

    public override void Init()
    {
        stageObjList = Resources.LoadAll<GameObject>("Maps").ToList();
        Transform gridObj = GameObject.Find("Isometric Palette").transform;

        for (int i = 0; i < stageObjList.Count; i++)
        {
            stageObjList[i] = Instantiate(stageObjList[i], gridObj);
            stageObjList[i].gameObject.SetActive(false);
        }

        LoadStage(IsometricManager.Instance.GetManager<GameManager>().mapRangeStrArray[0]);
    }

    public void LoadStage(string mapRange)
    {
        if(currentMapRange == null)
        {
            currentMapRange = mapRange;
        }
        else 
        {
            if (!currentMapRange.Equals(mapRange))
            {
                currentMapRange = null;
                IsometricManager.Instance.GetManager<GameManager>().lastBallList.Clear();
            }
        }

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        IsometricManager.Instance.GetManager<UIManager>().Load();

        if (gm.mapRangeStrArray.Length >= stageIndex && stageIndex > 0)
        {
            SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();
            sm.range = mapRange;
            sm.LoadMapSpreadsheets(() =>
            {
                gm.ResetData();
                gm.goalList = sm.mainMap.GetComponentsInChildren<Goal>().ToList();
                gm.goalList.ForEach(x => x.ResetFlag(false));
                gm.portalList = sm.mainMap.GetComponentsInChildren<Teleporter>().ToList();
                gm.portalList.ForEach(portal => portal.FindPair());

                InitBallControllUIs(Resources.Load<StageDataSO>($"Stage {stageIndex}").balls);
                IsometricManager.Instance.UpdateState(eUpdateState.Load);
            });
        }
        else if(gm.mapRangeStrArray.Length < stageIndex) // 12까지 있는데 13불러오려 하면
        {
            SetDebugText($"{gm.mapRangeStrArray.Length} Stage is last");
        }
        else // 0 이하의 맵 번호 입력시?
        {
            SetDebugText("Please enter over zero!");
        }

        FadeDebugText();
    }

    public void SetStageIndex(string stageIndexStr)
    {
        int.TryParse(stageIndexStr, out stageIndex);
    }

    public override void UpdateState(eUpdateState state)
    {
        switch (state)
        {
            case eUpdateState.Init:
                Init();
                break;
        }
    }

    public override void Load()
    {
        
    }
}
