using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    [SerializeField,Header("�ش� Ÿ�ϸ�")] private Tilemap mainMap;
    [SerializeField, Header("���������Ʈ ����")] private string range;
    [SerializeField, Header("Json ������")] private List<LevelData> datas = new List<LevelData>();
    

    const string URL = "https://docs.google.com/spreadsheets/d/1ikRYpziG0g-MmSjAE14hvrXo_hWKSsuWlAj6cpPD9pY/export?format=tsv&gid=80333382&range=";

    public void SaveMap()
    {
        BoundsInt bounds = mainMap.cellBounds;

        LevelDatas levelData = new LevelDatas();

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {

                TileBase temp = mainMap.GetTile(new Vector3Int(x, y, 0));

                if (temp != null)
                {
                    LevelData data = new LevelData();
                    data.tile = temp;
                    data.pos = new Vector3Int(x, y, 0);
                    data.type = GetType(temp);

                    levelData.datas.Add(data);
                }

            }
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/testSave.json", json);
    }

    public void LoadMapJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/testSave.json");
        datas = new List<LevelData>(JsonUtility.FromJson<LevelDatas>(json).datas);

        ClearTileMap();

        for (int i = 0; i < datas.Count; i++)
        {
            mainMap.SetTile(datas[i].pos, datas[i].tile);

            if (datas[i].type.Equals(TileType.None))
            {

            }
            else
            {
                GameObject a = PoolManager.Instance.Pop(datas[i].type.ToString()).gameObject;

                a.transform.position = mainMap.CellToWorld(
                    new Vector3Int(datas[i].pos.x + 1, datas[i].pos.y + 1, 0));
                a.transform.parent = mainMap.transform;
                a.SetActive(true);

            }
        }
    }

    public void LoadMapSpreadsheets()
    {
        string data;
        StartCoroutine(Getdata());

        IEnumerator Getdata()
        {
            UnityWebRequest www = UnityWebRequest.Get(URL + range);
            yield return www.SendWebRequest();
            ClearTileMap();
            data = www.downloadHandler.text;
            //print(data);

            string[] row = data.Split('\n');
            int rowSize = row.Length;
            int columnSize = row[0].Split('\t').Length;
            //print(rowSize);

            for (int i = 0; i < rowSize; i++)
            {
                string[] column = row[i].Split('\t');
                for (int j = 0; j < columnSize; j++)
                {
                    //print(column[j]);

                    TileBase tile = ParseTile(column[j]);
                    Vector3Int pos = new Vector3Int(1 + j, 6 - i, 0);
                    TileType type;

                    mainMap.SetTile(pos, tile);
                    print(tile.name);
                    type = GetType(tile);


                    if (type.Equals(TileType.None))
                    {
                    }
                    else
                    {
                        GameObject a = PoolManager.Instance.Pop(type.ToString()).gameObject;

                        a.transform.position = mainMap.CellToWorld(
                            new Vector3Int(pos.x + 1, pos.y + 1, 0));
                        a.transform.parent = mainMap.transform;
                        a.SetActive(true);
                    }
                    yield return null;
                }
            }
        }
    }

    public TileBase ParseTile(string data)
    {
        TileBase tile;
        data = data.ToUpper().Trim();

        TileColors color;

        switch (data)
        {
            case "D":
                color = TileColors.Yellow;
                break;

            case "\\":
            case "/":
                color = TileColors.Green;
                break;

            case "L":
                color = TileColors.Red;
                break;

            case "��":
            case "��":
            case "��":
            case "��":
                color = TileColors.Blue;
                break;

            case "S":
                color = TileColors.White;
                break;

            case "O":
                color = TileColors.Orange;
                break;

            default:
                if (data.Contains("TP"))
                {
                    color = TileColors.Purple;
                }
                else
                {
                    color = TileColors.Black;
                    print(data);
                }
                break;
        }
        tile = Resources.Load<TileBase>($"IsometricTileAssets/1{color}");
        tile.name = color.ToString();
        return tile;
    }

    public TileType GetType(TileBase tile)
    {
        TileColors tileColor;

        for (int i = 0; i < 8; i++)
        {
            tileColor = (TileColors)i;
            if (tile.name.Contains(tileColor.ToString()))
            {
                switch (tileColor)
                {
                    case TileColors.Black:
                        return TileType.None;
                    case TileColors.Blue:
                        return TileType.DirectionChanger;
                    case TileColors.Green:
                        return TileType.Reflect;
                    case TileColors.Orange:
                        return TileType.None;
                    case TileColors.Purple:
                        return TileType.Teleporter;
                    case TileColors.Red:
                        return TileType.Goal;
                    case TileColors.White:
                        return TileType.Slow;
                    case TileColors.Yellow:
                        return TileType.JumpPad;
                }
            }
        }

        return TileType.None;
    }

    public void ClearTileMap()
    {
        mainMap.ClearAllTiles();
        var child = mainMap.GetComponentsInChildren<Transform>();
        foreach(var c in child)
        {
            if(c != mainMap.transform)
            {
                Destroy(c.gameObject);
            }
        }

    }
}

[System.Serializable]
public class LevelDatas
{
    public List<LevelData> datas = new List<LevelData>();
}


[System.Serializable]
public class LevelData
{
    public TileBase tile;
    public Vector3Int pos;
    public TileType type;
}

[System.Serializable]
public enum TileColors
{
    Black,
    Blue,
    Green,
    Orange,
    Purple,
    Red,
    White,
    Yellow
}


