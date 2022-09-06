using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : ManagerBase
{
    public int stageIndex = 1;
    public int clearMapCount = 0;

    public List<ObjectTile> objectTileList = new List<ObjectTile>();
    public List<GameObject> stageObjList = new List<GameObject>();

    public Action<string> SetDebugText;
    public Action FadeDebugText;
    public Action<Ball[]> InitBallControllUIs;
    public Action ClearBallUis;

    private StageDataSO currentStageData;

    public override void Init()
    {
        clearMapCount = PlayerPrefs.GetInt("ClearMapsCount", 0);
        stageObjList = Resources.LoadAll<GameObject>("Maps").ToList();
        Transform gridObj = GameObject.Find("Isometric Palette").transform;

        for (int i = 0; i < stageObjList.Count; i++)
        {
            stageObjList[i] = Instantiate(stageObjList[i], gridObj);
            stageObjList[i].gameObject.SetActive(false);
        }

        LoadStage(Resources.Load<StageDataSO>($"Stage {stageIndex}"));
    }

    public void LoadStage(StageDataSO stageData)
    {
        bool isSameStageLoaded = false; 

        if (currentStageData == null)
        {
            currentStageData = stageData;
        }

        if (!currentStageData.Equals(stageData))
        {
            currentStageData = null;
            IsometricManager.Instance.GetManager<GameManager>().lastBallList.Clear();
        }
        else
        {
            isSameStageLoaded = true;
        }
        IsometricManager.Instance.GetManager<UIManager>().Load();

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();
        sm.range = stageData.range;
        sm.sheet = ((int)stageData.eSheet).ToString();

        sm.LoadMapSpreadsheets(() =>
        {
            gm.ResetData();
            gm.goalList = sm.mainMap.GetComponentsInChildren<Goal>().ToList();
            gm.goalList.ForEach(x => x.ResetFlag(false));

            gm.portalList = sm.mainMap.GetComponentsInChildren<Teleporter>().ToList();
            gm.portalList.ForEach(portal => portal.FindPair());

            gm.limitTime = stageData.countDown;
            gm.maxBallCount = stageData.balls.Length;
            ClearBallUis();

            if(false) // isSameStageLoaded && gm.lastBallList.Count >= stageData.balls.Length
            {
                for(int i = 0; i < gm.lastBallList.Count; i++)
                {
                    gm.MakeNewBallUI(gm.lastBallList[i], true);
                }
            }
            else
            {
                for (int i = 0; i < stageData.balls.Length; i++)
                {
                    Ball ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
                    gm.MakeNewBallUI(ball, false);
                }
            }
            IsometricManager.Instance.UpdateState(eUpdateState.Load);
        });


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
