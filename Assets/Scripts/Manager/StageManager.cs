using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Linq;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public int stageIndex = 1;
    
    private Dictionary<TileType, ObjectTile> dicPrefabs = new Dictionary<TileType, ObjectTile>();
    public List<ObjectTile> objectTileList = new List<ObjectTile>();

    public Button moveStageBtn;
    public TMP_InputField stageIndexInputField;
    public List<GameObject> stageObjList = new List<GameObject>();
    public TextMeshProUGUI debugText;

    private GameObject beforeStageObj = null;

    void Start()
    {
        instance = this;

        moveStageBtn.onClick.AddListener(() =>
        {
            LoadStage();
        });
        stageIndexInputField.onValueChanged.AddListener(SetStageIndex);
        //foreach ( var tile in objectTileList)
        //{
        //    PoolManager.Instance.CreatePool(tile);
        //
        //    dicPrefabs.Add(tile.myType, tile);
        //}

        //SetStage();
    }

    public void LoadStage()
    {
        beforeStageObj?.SetActive(false);
        debugText.gameObject.SetActive(true);

        if (stageObjList.Count >= stageIndex)
        {
            debugText.text = $"{stageIndex} 스테이지 로드";
            stageObjList[stageIndex - 1].SetActive(true);
            GameManager.Instance.goalList = stageObjList[stageIndex - 1].GetComponentsInChildren<Goal>().ToList();
        }
        else
        {
            debugText.text = $"{stageIndex}는 존재하지 않습니다";
        }
    }

    public void SetStageIndex(string stageIndexStr)
    {
        int.TryParse(stageIndexStr, out stageIndex);
    }

    public void SetStage()
    {
        //stageIndex를 가지고 해당하는 파일의 데이터를 불러와 맵 생성하는 그런 녀석을 만들어 볼꺼에요

        TextAsset textAsset = Resources.Load<TextAsset>($"StageData{stageIndex}");
        NodeClass stageData = JsonUtility.FromJson<NodeClass>(textAsset.text);

        foreach(var item in stageData.data)
        {
            //ObjectTile tile = dicPrefabs[item.tile];
            //Quaternion tileRotation = GetTileRotation(tile.myDirection);
            //
            //ObjectTile newTile = Instantiate(tile, new Vector2(item.x, item.y), tileRotation);
            //newTile.name = item.name;
            //if (item.tile.Equals(TileType.Goal))
            //{
            //    GameManager.Instance.goalList.Add((Goal)tile); // 골 체크용으로 리스트에 추가.
            //}
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
