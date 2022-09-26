using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapUI : MonoBehaviour
{
    public BallControllUI ballControllUI { get; set; } = null;

    float width = 0f;

    private void Start()
    {
        width = Screen.width;
    }

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        GameObject obj = eventData.hovered.Find((x) => x.transform.CompareTag("BallControllUI"));

        int insertIndex = 0;

        if (obj != null)
        {
            BallControllUI ui = obj.GetComponentInParent<BallControllUI>(); // 버튼 부모가 UI임
            insertIndex = Input.mousePosition.x < ui.rt.anchoredPosition.x + (width - 1000) / 2 ? ui.order : ui.order + 1;
        }
        else
        {
            insertIndex = Input.mousePosition.x < (width - 1000) / 2 + gm.ballUIList.Count - 1  * 190 ? 0 : gm.ballUIList.Count - 1;
        }

        insertIndex = Mathf.Clamp(insertIndex, 0, gm.ballUIList.Count - 1);

        ballControllUI.order = insertIndex;
        for (int i = insertIndex + 1; i < gm.ballUIList.Count; i++)
        {
            gm.ballUIList[i].order++;
        }

        gm.BallUiSort();

        gm.ballUIList.ForEach((x) => x.SetInteractValues(true));

        ballControllUI.transform.SetSiblingIndex(insertIndex);

        ballControllUI.directionImg.transform.DOScaleX(1, 0.3f);
        ballControllUI.rt.DOSizeDelta(new Vector2(190, 190), 0.3f).OnComplete(() =>
        {
            gm.ballUIList.ForEach((x) => x.SetInteractValues(true));

            ballControllUI = null;
            gameObject.SetActive(false);
        });
    }
}
