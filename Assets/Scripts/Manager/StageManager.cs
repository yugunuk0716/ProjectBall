using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;

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
            ClearAllBalls();
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

    public void ClearAllBalls()
    {
        PoolManager.Instance.gameObject.GetComponentsInChildren<Ball>().ToList().ForEach(x => x.gameObject.SetActive(false));
    }

    public void LoadStage()
    {
        beforeStageObj?.SetActive(false);
        beforeStageObj = stageObjList[stageIndex - 1];
        GameManager.Instance.shooter = stageObjList[stageIndex - 1].GetComponentInChildren<ShooterTile>();

        if (stageObjList.Count >= stageIndex)
        {
            debugText.text = $"{stageIndex} Stage Loaded";
            stageObjList[stageIndex - 1].SetActive(true);
            GameManager.Instance.goalList = stageObjList[stageIndex - 1].GetComponentsInChildren<Goal>().ToList();
        }
        else
        {
            debugText.text = $"{stageIndex} Stage Not Exist";
        }

        debugText.DOComplete();
        debugText.color = new Color(1, 0.5f, 0.5f, 1);
        debugText.DOFade(0, 2);

        GameManager.Instance.ResetGameData();
    }

    public void SetStageIndex(string stageIndexStr)
    {
        int.TryParse(stageIndexStr, out stageIndex);
        stageIndex = Mathf.Clamp(stageIndex, 1, stageObjList.Count);
    }

    public void SetStage()
    {
        //stageIndex를 가지고 해당하는 파일의 데이터를 불러와 맵 생성하는 그런 녀석을 만들어 볼꺼에요


    }

    public Quaternion GetTileRotation(TileDirection direction)
    {
        Quaternion quaternion = Quaternion.identity;

        // 근데 이거 쓰려면 up -> left -> Down -> right 순으로 바꿔줘야 해용
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
}
