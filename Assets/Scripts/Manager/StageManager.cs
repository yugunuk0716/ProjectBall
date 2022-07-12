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
        //stageIndex를 가지고 해당하는 파일의 데이터를 불러와 맵 생성하는 그런 녀석을 만들어 볼꺼에요

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
                GameManager.Instance.goalList.Add((Goal)tile); // 골 체크용으로 리스트에 추가.
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
