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

    TileHelpUI tileHelp;

    private GameManager gm;
    private SaveManager sm;
    private LifeManager lm;
    private UIManager um;
    private bool isFirstLoad = true;
    public bool isMapLoading = false;

    [HideInInspector] public GameObject retryBtn;

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

        gm = IsometricManager.Instance.GetManager<GameManager>();
        sm = IsometricManager.Instance.GetManager<SaveManager>();
        lm = IsometricManager.Instance.GetComponent<LifeManager>();
        um = IsometricManager.Instance.GetComponent<UIManager>();

        LoadStage(stageIndex);


    }
    public override void Load() { }


    public void LoadStage(int stageIndex)
    {
        if (retryBtn)
        {
            retryBtn.gameObject.SetActive(false);
        }

        if (tileHelp == null)
        {
            tileHelp = IsometricManager.Instance.GetManager<UIManager>().FindUI("HelpPanel").GetComponent<TileHelpUI>();
        }

        if (!isMapLoading)
        {
            isMapLoading = true;

            gm.StopGame();
            tileHelp.MoveUI(false);
            GameManager.canInteract = false;
            gm.usableBallList.ForEach((x) =>
            {
                x.SetDisable();
            });

            bool isSameStageLoaded = false;

            int realIndex = stageIndex - 1;
            gm.SetStageText(stageIndex);

            if (currentStageData == null) // first load
            {
                currentStageData = stageDataList[realIndex];
            }
            else
            {
                if (currentStageData.Equals(stageDataList[realIndex])) // if curstage and target stage not same
                {
                    isSameStageLoaded = true;
                }
                else
                {
                    currentStageData = stageDataList[realIndex];
                }
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
    }

    IEnumerator WaitUntilObjectTileCreated(Action callBack)
    {
        yield return new WaitForSeconds(1.5f);
        isMapLoading = false;
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
        if (leftTime >= threeStarTime)
        {
            return 3;
        }
        else if (leftTime >= twoStarTime)
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
        if (stageIndex == 0)
        {
            stageIndex = 1;
        }
        PlayerPrefs.SetInt("LastStage", stageIndex);

    }

}
