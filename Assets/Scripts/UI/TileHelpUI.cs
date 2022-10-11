using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class TileHelpUI : UIBase
{
    [SerializeField] Button onBtn;
    [SerializeField] RectTransform scrollView; // this

    private Dictionary<TileType, TileDescUI> descUIDict;

    bool isViewing = false;
    bool isMoving = false;

    public List<TileDescUI> descUIList = new List<TileDescUI>();

    private Transform mainMap;

    public void SetTexts(List<ObjectTile> tiles)
    {
        foreach (var item in descUIDict.Values)
        {
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);
            }
        }

        foreach (ObjectTile tile in tiles)
        {
            if (descUIDict.ContainsKey(tile.myType))
            {
                descUIDict[tile.myType].gameObject.SetActive(true);
            }
        }
    }

    float width;

    public override void Init()
    {
        width = 1080 * ((float)Screen.width / Screen.height / 0.56f);

        GetCanvasGroup();
        mainMap = GameObject.Find("MainMap").transform;
        SetDict();

        scrollView.anchoredPosition = new Vector3(width, scrollView.anchoredPosition.y, 0);

        onBtn.onClick.AddListener(() =>
        {
            if (isMoving)
            {
                return;
            }

            isMoving = true;
            scrollView.DOAnchorPosX(isViewing ? width : 0, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                isMoving = false;
            });
            isViewing = !isViewing;

            onBtn.DOComplete();
            onBtn.transform.DORotate(new Vector3(0,0, onBtn.transform.localEulerAngles.z + 180), 0.2f);
        });
    }

    public void SetDict()
    {
        descUIDict = new Dictionary<TileType, TileDescUI>();
        foreach (var item in descUIList)
        {
            descUIDict[item.tileType] = item;
        }
    }

    public override void Load()
    {
        List<ObjectTile> tiles = mainMap.GetComponentsInChildren<ObjectTile>().Distinct(new ObjectTileComparer()).ToList();
        SetTexts(tiles);
    }
}
