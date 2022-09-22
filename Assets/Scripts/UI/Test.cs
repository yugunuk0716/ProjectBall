using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform beforeParent;
    RectTransform rt;

    private void Awake()
    {
        beforeParent = transform.parent;
        rt = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        rt.anchoredPosition = pos;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        List<Test> swapperList = gm.swapperList;

        GameObject obj = eventData.hovered.Find((x) => x.name.Contains("BallControllUI"));
        int insertIndex = 0;



        if (obj != null)
        {
            Debug.Log("1");
            insertIndex = obj.GetComponent<BallControllUI>().order;
        }
        else
        {
            Debug.Log("2");
            insertIndex = 0;
        }

        if (swapperList.Count > 0)
        {
            bool isInsertRight = swapperList[insertIndex].transform.localPosition.x < transform.localPosition.x;
            insertIndex = isInsertRight ? insertIndex + 1 : insertIndex;
            insertIndex = Mathf.Clamp(insertIndex, 0, swapperList.Count);
        }

        Debug.Log($"{obj == null}, {insertIndex}");

        transform.SetParent(beforeParent);
        transform.SetSiblingIndex(insertIndex);
    }
}
