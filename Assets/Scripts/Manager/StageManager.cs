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
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();
        IsometricManager.Instance.GetManager<UIManager>().Load();

        gm.myBallList.ForEach(b => PoolManager.Instance.Push(b));
        gm.aliveBallList.ForEach(b => PoolManager.Instance.Push(b));

        bool isSameStageLoaded = false;

        if (currentStageData == null) // 첫 로드
        {
            currentStageData = stageData;
        }
        else if (currentStageData.Equals(stageData)) // 현 스테이지랑 목표 스테이지랑 다르면
        {
            isSameStageLoaded = true;
        }
        else
        {
            currentStageData = stageData;
            gm.lastBallList.Clear();
        }

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

            if (isSameStageLoaded && gm.lastBallList.Count >= stageData.balls.Length)
            {
                for (int i = 0; i < stageData.balls.Length; i++)
                {
                    gm.MakeNewBallUI(gm.lastBallList[i], true);
                }

                gm.lastBallList = gm.lastBallList.GetRange(0, stageData.balls.Length);
            }
            else
            {
                for (int i = 0; i < stageData.balls.Length; i++)
                {
                    Ball ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
                    gm.MakeNewBallUI(ball, false);
                }
            }

            /* Action updateAction = null;

             updateAction = () =>
             {
                 if (Input.GetKeyDown(KeyCode.Backspace))
                 {
                     foreach (var item in sm.tileDatas)
                     {
                         sm.SetAnimationForMapLoading(item);
                     }
                     FunctionUpdater.Delete(updateAction);
                 }
             };
             FunctionUpdater.Create(updateAction);*/

            foreach (var item in sm.tileDatas)
            {
                sm.SetAnimationForMapLoading(item);
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
