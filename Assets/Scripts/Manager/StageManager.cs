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
        //stageIndex�� ������ �ش��ϴ� ������ �����͸� �ҷ��� �� �����ϴ� �׷� �༮�� ����� ��������

        //�ӽ÷δٰ�

        foreach (ObjectTile tile in objectTiles)
        {
            if (tile.myType.Equals(TileType.Goal))
            {
                GameManager.Instance.goalList.Add((Goal)tile);
            }
        }
    }

   
}
