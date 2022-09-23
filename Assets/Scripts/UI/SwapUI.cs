using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public BallControllUI ballControllUI { get; set; } = null;

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        GameObject obj = eventData.hovered.Find((x) => x.name.Contains("BallControllUI"));
        int insertIndex = 0;

        if (obj != null)
        {
            BallControllUI ui = obj.GetComponent<BallControllUI>();
            insertIndex = obj.GetComponent<BallControllUI>().order;
            Debug.Log($"Test, {ui.rt.anchoredPosition.x + (Screen.width - 1000) / 2} / {transform.position.x}");
        }
        else
        {
            float distFirst = Vector2.Distance(gm.ballUIList[0].rt.anchoredPosition, ballControllUI.rt.anchoredPosition);
            float distLast  = Vector2.Distance(gm.ballUIList[gm.ballUIList.Count -1].rt.anchoredPosition, ballControllUI.rt.anchoredPosition);
            insertIndex = distFirst < distLast ? 0 : gm.ballUIList.Count - 1;
        }

        Debug.Log($"{insertIndex}");

        ballControllUI.transform.SetSiblingIndex(insertIndex);
        ballControllUI.transform.DOScaleX(1, 0.2f);
        ballControllUI.order = insertIndex;
        ballControllUI = null;
        
        for (int i = insertIndex + 1; i < gm.ballUIList.Count; i++)
        {
            gm.ballUIList[i].order++;
        }
        gm.BallUiSort();

       
        gameObject.SetActive(false);
    }
}
