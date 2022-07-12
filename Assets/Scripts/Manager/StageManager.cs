using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public int stageIndex = 1;
    
    private Dictionary<TileType, ObjectTile> dicPrefabs = new Dictionary<TileType, ObjectTile>();
    public List<ObjectTile> objectTileList = new List<ObjectTile>();

    void Start()
    {
        instance = this;

        foreach ( var tile in objectTileList)
        {
            dicPrefabs.Add(tile.myType, tile);
        }

        SetStage();
    }


    public void SetStage()
    {
        //stageIndex�� ������ �ش��ϴ� ������ �����͸� �ҷ��� �� �����ϴ� �׷� �༮�� ����� ��������

        TextAsset textAsset = Resources.Load<TextAsset>($"StageData{stageIndex}");
        NodeClass stageData = JsonUtility.FromJson<NodeClass>(textAsset.text);

        foreach(var item in stageData.data)
        {
            ObjectTile tile = dicPrefabs[item.tile];
            Quaternion tileRotation = GetTileRotation(tile.myDirection);

            ObjectTile newTile = Instantiate(tile, new Vector2(item.x, item.y), tileRotation);
            newTile.name = item.name;
            if (item.tile.Equals(TileType.Goal))
            {
                GameManager.Instance.goalList.Add((Goal)tile); // �� üũ������ ����Ʈ�� �߰�.
            }
        }
    }

    public Quaternion GetTileRotation(TileDirection direction)
    {
        Quaternion quaternion = Quaternion.identity;
        switch(direction)
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

        return quaternion;
    }
}
