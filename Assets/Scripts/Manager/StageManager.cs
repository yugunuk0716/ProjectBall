using System;
using System.Collections;
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
    public Action ReuseUI;

    private StageDataSO currentStageData;

    public override void Init()
    {
        ClearBallUis += () => IsometricManager.Instance.GetManager<GameManager>().ballUIList.ForEach((x) => Destroy(x.gameObject));
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


        gm.myBallList.ForEach(b => PoolManager.Instance.Push(b));
        gm.aliveBallList.ForEach(b => PoolManager.Instance.Push(b));

        SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();
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
            ClearBallUis();
            ClearActiveBalls();
            ReuseUI?.Invoke();

            gm.TakeMapLoadVideo();
            foreach (var item in sm.tileDatas)
            {
                sm.SetAnimationForMapLoading(item);
            }

            StartCoroutine(WaitUntilObjectTileCreated(() =>
            {
                IsometricManager.Instance.UpdateState(eUpdateState.Load);
                gm.ResetData(stageData, isSameStageLoaded);
            }));
        });

        FadeDebugText();
    }

    IEnumerator WaitUntilObjectTileCreated(Action callBack)
    {
        yield return new WaitForSeconds(1f);
        callBack();

    }

    private void ClearActiveBalls()
    {
        PoolManager.Instance.gameObject.GetComponentsInChildren<Ball>().ToList().ForEach(x => x.gameObject.SetActive(false));
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
