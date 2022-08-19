using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class SaveManager : ManagerBase
{
    [SerializeField,Header("해당 타일맵")] public Tilemap mainMap;
    [SerializeField, Header("스프레드시트 범위")] public string range;
    [SerializeField, Header("Json 데이터")] private List<LevelData> datas = new List<LevelData>();
    

    const string URL = "https://docs.google.com/spreadsheets/d/1ikRYpziG0g-MmSjAE14hvrXo_hWKSsuWlAj6cpPD9pY/export?format=tsv&gid=80333382&range=";

    private string lastString = string.Empty;
    private int portalIndex = 0;

    public void LoadMapSpreadsheets(Action callback) // 맵 데이터 초기화 콜백
    {
        IsometricManager.Instance.GetManager<GameManager>().tileDict.Clear();
        string data;
        StartCoroutine(Getdata());

        IEnumerator Getdata()
        {
            UnityWebRequest www = UnityWebRequest.Get(URL + range);
            yield return www.SendWebRequest();
            ClearTileMap();
            data = www.downloadHandler.text;

            string[] row = data.Split('\n');
            int rowSize = row.Length;
            int columnSize = row[0].Split('\t').Length;

            for (int i = 0; i < rowSize; i++)
            {
                string[] column = row[i].Split('\t');

                

                for (int j = 0; j < columnSize; j++)
                {
                    bool isTransitionTile = column[j].Contains("*");
                    if (isTransitionTile)
                    {
                        column[j] = column[j].Substring(1);
                    }

                    TileBase tile = ParseTile(column[j]);
                    Vector3Int pos = new Vector3Int(1 + j, 6 - i, 0);
                    TileType type;

                    mainMap.SetTile(pos, tile);
                    type = GetType(tile);

                    ObjectTile a = PoolManager.Instance.Pop(type.ToString()) as ObjectTile;
                    if (isTransitionTile)
                    {
                        a.StartTransition();
                    }

                    if (type.Equals(TileType.None))
                    {
                    }
                    else
                    {
                        if (type.Equals(TileType.Teleporter))
                        {
                            Teleporter tp = a.GetComponent<Teleporter>();
                            tp.portalIndex = portalIndex;
                        }

                        //스프라이트 갈아끼고 아래 변수들 다 설정해줘야댐
                        a.dataString = lastString;
                        a.SetDirection();
                       
                        a.transform.position = mainMap.CellToWorld(
                            new Vector3Int(pos.x + 1, pos.y + 1, 0));
                        a.transform.parent = mainMap.transform;
                        a.gameObject.SetActive(true);
                    }
                    Vector2 worldPoint = mainMap.CellToWorld(pos);
                    a.worldPos = new Vector2(worldPoint.x,worldPoint.y + 0.25f);
                    pos = new Vector3Int(pos.x - 1, pos.y - 6 + rowSize - 1);
                    a.gridPos = pos;
                    a.keyPos = new Vector2(pos.x, pos.y);
                    IsometricManager.Instance.GetManager<GameManager>().tileDict.Add(new Vector2(pos.x, pos.y), a);
                    yield return null;
                }
            }

            callback();
        }
    }

    public TileBase ParseTile(string data)
    {
        TileBase tile;
        data = data.ToUpper().Trim();

        TileColors color;
        lastString = data;

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

            case "→":
            case "←":
            case "↑":
            case "↓":
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

                    portalIndex = int.Parse(data.Substring(3));
                    color = TileColors.Purple;
                }
                else
                {
                    color = TileColors.Black;
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

    public override void Init()
    {
        mainMap = GameObject.Find("MainMap").GetComponent<Tilemap>();
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


