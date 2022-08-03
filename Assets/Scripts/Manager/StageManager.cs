using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StageManager : ManagerBase
{
    public int stageIndex = 1;
    
    private Dictionary<TileType, ObjectTile> dicPrefabs = new Dictionary<TileType, ObjectTile>();
    public List<ObjectTile> objectTileList = new List<ObjectTile>();
    public List<GameObject> stageObjList = new List<GameObject>();

    public Action<string> SetDebugText;
    public Action FadeDebugText;
    public Action<Ball[]> InitBallControllUIs;

    private GameObject beforeStageObj = null;

    public override void Init()
    {
        stageObjList = Resources.LoadAll<GameObject>("Maps").ToList();
        Transform gridObj = GameObject.Find("Isometric Palette").transform;

        for (int i = 0; i < stageObjList.Count; i++)
        {
            stageObjList[i] = Instantiate(stageObjList[i], gridObj);
            stageObjList[i].gameObject.SetActive(false);
        }
    }

    public void ClearAllBalls()
    {
        PoolManager.Instance.gameObject.GetComponentsInChildren<Ball>().ToList().ForEach(x => x.gameObject.SetActive(false));
    }

    public void LoadStage()
    {
        if (stageObjList.Count >= stageIndex && stageIndex > 0)
        {
            GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

            beforeStageObj?.SetActive(false);
            beforeStageObj = stageObjList[stageIndex - 1];
            gm.shooter = stageObjList[stageIndex - 1].GetComponentInChildren<ShooterTile>();

            SetDebugText($"{stageIndex} Stage Loaded");
            stageObjList[stageIndex - 1].SetActive(true);

            gm.goalList = stageObjList[stageIndex - 1].GetComponentsInChildren<Goal>().ToList();
            gm.goalList.ForEach(x => x.ResetFlag());
            gm.portalList = stageObjList[stageIndex - 1].GetComponentsInChildren<Teleporter>().ToList();
            gm.portalList.ForEach(portal => portal.FindPair());

            // 대충 여기서 공 데이터 받아와야겠당
            InitBallControllUIs(Resources.Load<StageDataSO>($"Stage {stageIndex}").balls);

        }
        else if(stageObjList.Count < stageIndex) // 12까지 있는데 13불러오려 하면
        {
            SetDebugText($"{stageObjList.Count} Stage is last");
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
}
