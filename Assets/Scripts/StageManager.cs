using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int stageIndex = 1;
    
    public List<ObjectTile> objectTiles = new List<ObjectTile>();



    void Start()
    {
        SetStage();
    }


    public void SetStage()
    {
        //stageIndex를 가지고 해당하는 파일의 데이터를 불러와 맵 생성하는 그런 녀석을 만들어 볼꺼에요

        //임시로다가

        foreach (ObjectTile tile in objectTiles)
        {
            if (tile.myType.Equals(TileType.Goal))
            {
                GameManager.Instance.goalList.Add((Goal)tile);
            }
        }
    }

   
}
