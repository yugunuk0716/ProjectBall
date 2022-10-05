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

    public Dictionary<int, int> stageProgressDictionary = new Dictionary<int, int>();

    private const float threeStarTime = 1.5f;
    private const float twoStarTime = 1f;

    public List<StageDataSO> stageDataList = new List<StageDataSO>();
    private StageDataSO currentStageData;

    public override void Init()
    {
        clearMapCount = PlayerPrefs.GetInt("ClearMapsCount", 0);

        for (int i = 0; i < clearMapCount; i++)
        {
            stageProgressDictionary.Add(i, GetStar(i));
        }

        stageObjList = Resources.LoadAll<GameObject>("Maps").ToList();
        Transform gridObj = GameObject.Find("Isometric Palette").transform;

        for (int i = 0; i < stageObjList.Count; i++)
        {
            stageObjList[i] = Instantiate(stageObjList[i], gridObj);
            stageObjList[i].gameObject.SetActive(false);
        }

        stageDataList = Resources.LoadAll<StageDataSO>("StageDatas").ToList();

        stageDataList.Sort((x, y) => x.stageIndex.CompareTo(y.stageIndex));

        stageIndex = PlayerPrefs.GetInt("LastStage", 1);
        LoadStage(stageIndex);

    }
    public override void Load() { }


    public void LoadStage(int stageIndex)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();
        gm.StopGame();

        gm.canInteract = false;
        gm.usableBallList.ForEach((x) =>
        {
            GameObjectPoolManager.Instance.UnusedGameObject(x.gameObject);
        });


        bool isSameStageLoaded = false;

        int realIndex = stageIndex - 1;
        gm.SetStageText(stageIndex);

        if (currentStageData == null) // 첫 로드
        {
            currentStageData = stageDataList[realIndex];
        }
        else if (currentStageData.Equals(stageDataList[realIndex])) // 현 스테이지랑 목표 스테이지랑 다르면
        {
            isSameStageLoaded = true;
        }
        else
        {
            currentStageData = stageDataList[realIndex];
        }

        sm.range = stageDataList[realIndex].range;
        sm.sheet = ((int)stageDataList[realIndex].eSheet).ToString();

        sm.LoadMapSpreadsheets(() =>
        {
            gm.TakeMapLoadVideo();
            foreach (var item in sm.tileDatas)
            {
                sm.SetAnimationForMapLoading(item);
            }


            StartCoroutine(WaitUntilObjectTileCreated(() =>
            {
                IsometricManager.Instance.UpdateState(eUpdateState.Load);
                gm.ResetData(stageDataList[realIndex], isSameStageLoaded);
            }));
        });

        FadeDebugText();
    }

    IEnumerator WaitUntilObjectTileCreated(Action callBack)
    {
        yield return new WaitForSeconds(1.5f);
        callBack();
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



    public int CalcStar(float leftTime)
    {
        if(leftTime > threeStarTime)
        {
            return 3;
        }
        else if(leftTime > twoStarTime)
        {
            return 2;
        }

        return 1;
    }

    public int GetStar(int targetStageIndex)
    {
        return PlayerPrefs.GetInt($"{targetStageIndex}Stage", 0);
    }

    public void SaveStar(int curStageIndex, int starCount)
    {
        //print(curStageIndex);
        PlayerPrefs.SetInt($"{curStageIndex}Stage", starCount);
    }

    private void OnApplicationQuit()
    {
        if(stageIndex == 0)
        {
            stageIndex = 1;
        }
        PlayerPrefs.SetInt("LastStage", stageIndex);
    }

}
