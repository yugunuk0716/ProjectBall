using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetPointUI : PoolableMono, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public BallControllUI ballControllUI; // 내 위치 따라올 놈
    public override void Reset()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        transform.position = pos;
    }

    Transform beforeParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beforeParent = transform.parent;
        Debug.Log(transform.root.name);
        transform.SetParent(transform.root);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        List<TargetPointUI> targetPoints = gm.targetPointUIList;

        int insertIndex = 0;
        float minDist = 10000f;

        for(int i = 0; i < targetPoints.Count; i++)
        {
            float dist = Vector3.Distance(targetPoints[i].transform.localPosition, transform.localPosition);
            if (minDist > dist)
                minDist = dist;

            insertIndex = i;
        }

        bool isInsertRight = targetPoints[insertIndex].transform.localPosition.x < transform.localPosition.x;

        insertIndex = isInsertRight ? insertIndex + 1 : insertIndex;
        if (targetPoints.Count == 0) insertIndex = 1;


        transform.SetParent(beforeParent);
        transform.SetSiblingIndex(insertIndex);
    }
}
