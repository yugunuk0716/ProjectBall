using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;
using System;

public class StageManager : ManagerBase
{
    public int stageIndex = 1;
    
    private Dictionary<TileType, ObjectTile> dicPrefabs = new Dictionary<TileType, ObjectTile>();
    public List<ObjectTile> objectTileList = new List<ObjectTile>();
    public List<GameObject> stageObjList = new List<GameObject>();

    public Action<string> SetDebugText;
    public Action FadeDebugText;
    private GameObject beforeStageObj = null;

    public override void Init()
    {
        stageObjList = Resources.LoadAll<GameObject>("").ToList();

        for (int i = 0; i < stageObjList.Count; i++)
        {
            stageObjList[i] = Instantiate(stageObjList[i]);
            stageObjList[i].gameObject.SetActive(false);
        }

        //foreach ( var tile in objectTileList)
        //{
        //    PoolManager.Instance.CreatePool(tile);
        //
        //    dicPrefabs.Add(tile.myType, tile);
        //}

        //SetStage();
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
            gm.portalList = stageObjList[stageIndex - 1].GetComponentsInChildren<Teleporter>().ToList();
            gm.portalList.ForEach(portal => portal.FindPair());
            gm.ResetGameData();
        }
        else if(stageObjList.Count < stageIndex) // 12���� �ִµ� 13�ҷ����� �ϸ�
        {
            SetDebugText($"{stageObjList.Count} Stage is last");
        }
        else // 0 ������ �� ��ȣ �Է½�?
        {
            SetDebugText("Please enter over zero!");
        }
    }

    public void SetStageIndex(string stageIndexStr)
    {
        int.TryParse(stageIndexStr, out stageIndex);
    }

    public void SetStage()
    {
        //stageIndex�� ������ �ش��ϴ� ������ �����͸� �ҷ��� �� �����ϴ� �׷� �༮�� ����� ��������


    }

    public Quaternion GetTileRotation(TileDirection direction)
    {
        Quaternion quaternion = Quaternion.identity;

        // �ٵ� �̰� ������ up -> left -> Down -> right ������ �ٲ���� �ؿ�
        //quaternion = Quaternion.Euler(0, 90 * (int)direction - 90, 0); 

        /*
        switch (direction)
        {
            case TileDirection.UP:
                quaternion = Quaternion.Euler(0, 0, 0);
                break;
            case TileDirection.LEFT:
                quaternion = Quaternion.Euler(0, 90, 0);
                break;
            case TileDirection.DOWN:
                quaternion = Quaternion.Euler(0, 180, 0);
                break;
            case TileDirection.RIGHT:
                quaternion = Quaternion.Euler(0, 270, 0);
                break;
        }
        */

        return quaternion;
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
