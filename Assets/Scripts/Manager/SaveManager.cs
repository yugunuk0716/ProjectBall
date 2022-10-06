using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using DG.Tweening;
using System;
using System.Linq;

public class SaveManager : ManagerBase
{
    [SerializeField, Header("Tile map")] public Tilemap mainMap;
    [SerializeField, Header("Animation Tilemap")] public Tilemap animationMap;
    [SerializeField, Header("Sheet Range")] public string range;
    [SerializeField, Header("Sheet Type")] public string sheet;


    public List<TileData> tileDatas = new List<TileData>();

    const string URL = "https://docs.google.com/spreadsheets/d/1ikRYpziG0g-MmSjAE14hvrXo_hWKSsuWlAj6cpPD9pY/export?format=tsv";

    private string lastString = string.Empty;
    private int portalIndex = 0;
    private string lineDir;
    private string targetString;
    private Color changeColor = new Color();
    private AnimatedTile riseAnimatedTile;

    public void LoadMapSpreadsheets(Action callback)
    {
        IsometricManager.Instance.GetManager<GameManager>().tileDict.Clear();
        string data;
        StartCoroutine(Getdata());

        IEnumerator Getdata()
        {
            UnityWebRequest www = UnityWebRequest.Get(URL + "&gid=" + sheet + "&range=" + range);

            yield return www.SendWebRequest();


            if (www.result.Equals(UnityWebRequest.Result.Success))
            {
                IsometricManager.Instance.GetManager<UIManager>().FindUI("NetworkPanel").ScreenOn(false);
            }
            else
            {
                IsometricManager.Instance.GetManager<UIManager>().FindUI("NetworkPanel").ScreenOn(true);
                yield break;
            }


           

            ClearTileMap();
            tileDatas.Clear();
            data = www.downloadHandler.text;

            string[] row = data.Split('\n');
            int rowSize = row.Length;
            int columnSize = row[0].Split('\t').Length;


            for (int i = 0; i < rowSize; i++)
            {
                string[] column = row[i].Split('\t');



                for (int j = 0; j < columnSize; j++)
                {
                    changeColor = Color.white;
                    lineDir = null;
                    targetString = null;
                    bool isTransitionTile = column[j].Contains("*");
                    if (isTransitionTile)
                    {
                        column[j] = column[j][1..];
                    }

                    bool isColoredTile = column[j].Contains("#");
                    if (isColoredTile)
                    {
                        string[] str = column[j].Split('#');
                        string colorCode;
                        column[j] = str[0];
                        colorCode = "#" + str[1];
                        ColorUtility.TryParseHtmlString(colorCode, out changeColor);
                    }


                    bool isLine = column[j].Contains("!");
                    if (isLine)
                    {
                        string[] str = column[j].Split('!');
                        column[j] = str[0];
                        lineDir = str[1];
                    }


                    bool isBtnTarget = column[j].Contains("~");
                    if (isBtnTarget)
                    {
                        string[] str = column[j].Split('~');
                        column[j] = str[0];
                        targetString = str[1];
                    }


                    Tile tile = ParseTile(column[j]);

                    Vector3Int pos = new Vector3Int(1 + j, 6 - i);
                    TileType type;

                    type = GetType(tile);

                    TileData data = new TileData();

                    data.animatedTile = riseAnimatedTile;
                    data.realTile = tile;
                    data.pos = pos;
                    data.type = type;
                    data.isTransitionTile = isTransitionTile;
                    data.rowSize = rowSize;
                    data.lineDir = lineDir;
                    data.targetstring = targetString;
                    data.lastString = lastString;

                    tileDatas.Add(data);
                }
            }
            yield return null;
            callback();
        }
    }

    public void SetAnimationForMapLoading(TileData data)
    {
        Tilemap map = mainMap;
        Tilemap animMap = animationMap;

        animMap.SetTile(data.pos, data.animatedTile);
        animMap.SetAnimationFrame(data.pos, 0);
        data.animatedTile.m_AnimationStartFrame = 0;

        Action updateAction = null;
        Vector3Int pos = data.pos;
        AnimatedTile animatedTile = data.animatedTile;
        Tile realTile = data.realTile;

        updateAction = () =>
        {
            string currentName = animMap.GetSprite(pos).name;

            if (currentName.Equals(animatedTile.m_AnimatedSprites[animatedTile.m_AnimatedSprites.Length - 1].name))
            {
                animMap.SetTile(pos, null);
                map.SetTile(pos, realTile);
                SettingObjectTiles(data);
                FunctionUpdater.Delete(updateAction);
            }



        };
        FunctionUpdater.Create(updateAction);
    }

    private void SettingObjectTiles(TileData data)
    {
        ObjectTile a = GameObjectPoolManager.Instance.GetGameObject($"Tiles/{data.type.ToString()}", GameObjectPoolManager.Instance.transform).GetComponent<ObjectTile>();
        int index = 0;

        if (data.isTransitionTile)
        {
            Debug.Log($"{a.name}");

            a.StartTransition();
        }

        if(data.targetstring != null)
        {
           
            string[] str = data.targetstring.Split('!');
            data.targetstring = str[0];
            
        }



        if (data.lineDir != null)
        {
            
            if (int.TryParse(data.lineDir[0].ToString(), out index))
            {
                data.lineDir = data.lineDir[1..];
            }
            Line line = GameObjectPoolManager.Instance.GetGameObject("Tiles/Line", GameObjectPoolManager.Instance.transform).GetComponent<Line>();




            switch (data.lineDir)
            {
                case "┃":
                    line.SetLineDir(true, false, false, true);
                    break;
                case "━":
                    line.SetLineDir(false, true, true, false);
                    break;
                case "┏":
                    line.SetLineDir(false, true, false, true);
                    break;
                case "┓":
                    line.SetLineDir(false, false, true, true);
                    break;
                case "┗":
                    line.SetLineDir(true, true);
                    break;
                case "┛":
                    line.SetLineDir(true, false, true, false);
                    break;
                case "┣":
                    line.SetLineDir(true, true, false, true);
                    break;
                case "┫":
                    line.SetLineDir(true, false, true, true);
                    break;
                case "┻":
                    line.SetLineDir(true, true, true, false);
                    break;
                case "┳":
                    line.SetLineDir(false, true, true, true);
                    break;
                case "╋":
                    line.SetLineDir(true, true, true, true);
                    break;
                case "▼":
                    line.SetLineDir(false, false, false, true);
                    break;
                case "▲":
                    line.SetLineDir(true, false, false, false);
                    break;
                case "◀":
                    line.SetLineDir(false, false, true, false);
                    break;
                case "▶":
                    line.SetLineDir(false, true, false, false);
                    break;


            }


            SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            sr.DOFade(1, .7f).SetEase(Ease.Linear);

            line.transform.position = mainMap.CellToWorld(new Vector3Int(data.pos.x, data.pos.y, 0));
            line.transform.position += new Vector3(0, 1.25f, 0);

            line.transform.DOMoveY(line.transform.position.y - 1, .7f).SetEase(Ease.InQuart);
            line.transform.parent = mainMap.transform;
            line.gameObject.SetActive(true);

            lineDir = null;
        }


        if (data.type.Equals(TileType.None))
        {
        }
        else
        {
            if (data.type.Equals(TileType.Teleporter))
            {
                Teleporter tp = a.GetComponent<Teleporter>();
                tp.portalIndex = portalIndex;
            }

            a.btnIndex = index;

            a.btnString = data.targetstring;
            a.dataString = data.lastString;

            a.transform.position = mainMap.CellToWorld(new Vector3Int(data.pos.x + 1, data.pos.y + 1, 0));
            a.transform.position += new Vector3(0, 1, 0);

            a.SetDirection();

            a.transform.parent = mainMap.transform;

            a.gameObject.SetActive(true);

            SpriteRenderer sr = a.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = a.GetComponentInChildren<SpriteRenderer>();
            }
            
            if (sr == null)
            {
                Debug.Log($"{sr == null} / {a?.name}");
            }


            float alpha = sr.color.a;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            sr.DOFade(alpha, .7f).SetEase(Ease.Linear);
            a.transform.DOMoveY(a.transform.position.y - 1, .7f).SetEase(Ease.InQuart);
        }


        Vector2 worldPoint = mainMap.CellToWorld(data.pos);
        a.worldPos = new Vector2(worldPoint.x, worldPoint.y + 0.25f);
        data.pos = new Vector3Int(data.pos.x - 1, data.pos.y - 6 + data.rowSize - 1);
        a.gridPos = data.pos;
        a.keyPos = new Vector2(data.pos.x, data.pos.y);
        IsometricManager.Instance.GetManager<GameManager>().tileDict.Add(new Vector2(data.pos.x, data.pos.y), a);
    }


    public Tile ParseTile(string data)
    {
        Tile tile;
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
            case "C":
                color = TileColors.Any;
                break;

            case "O":
                color = TileColors.Orange;
                break;

            case "W":
                color = TileColors.Gray;
                break;
            case "B":
                color = TileColors.Deepblue;
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
        tile = Resources.Load<Tile>($"IsometricTileAssets/1{color}");
        riseAnimatedTile = Resources.Load<AnimatedTile>($"IsometricTileAssets/AnimatedTile/1{color}");
        if (riseAnimatedTile == null)
        {
            riseAnimatedTile = Resources.Load<AnimatedTile>($"IsometricTileAssets/AnimatedTile/1White");
        }
        tile.name = color.ToString();
        if (tile.name.Equals("Any"))
        {
            tile.color = changeColor;
        }
        else
        {
        }
        return tile;
    }

    public TileType GetType(TileBase tile)
    {
        TileColors tileColor;

        for (int i = 0; i < Enum.GetValues(typeof(TileColors)).Length; i++)
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
                        return TileType.Flag;
                    case TileColors.White:
                        return TileType.Slow;
                    case TileColors.Yellow:
                        return TileType.JumpPad;
                    case TileColors.Gray:
                        return TileType.Thorn;
                    case TileColors.Deepblue:
                        return TileType.ButtonTile;
                }
            }
        }

        return TileType.None;
    }

    public void ClearTileMap()
    {
        List<ObjectTile> tiles = mainMap.GetComponentsInChildren<ObjectTile>().ToList();
        tiles.ForEach((x) =>
        {
            if(!x.name.Contains("ShooterTile"))
            {
                x.SetDisable();
            }
        });
        GameObjectPoolManager.Instance.GetComponentsInChildren<ObjectTile>().ToList().ForEach((x) => x.SetDisable());

        mainMap.ClearAllTiles();
    }

    public override void Init()
    {
        mainMap = GameObject.Find("MainMap").GetComponent<Tilemap>();
        animationMap = GameObject.Find("AnimatedMap").GetComponent<Tilemap>();
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

    public override void Load()
    {

    }
}

[System.Serializable]
public class TileData
{
    public AnimatedTile animatedTile;
    public Tile realTile;
    public Vector3Int pos;
    public TileType type;
    public bool isTransitionTile;
    public int rowSize;
    public string lastString;
    public string lineDir;
    public string targetstring;
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
    Yellow,
    Any,
    Gray,
    Deepblue
}


