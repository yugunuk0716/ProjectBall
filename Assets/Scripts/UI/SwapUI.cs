using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapUI : MonoBehaviour
{
    public BallControllUI ballControllUI { get; set; } = null;

    float width = 0f;
    float ratio = 1f;
    private void Start()
    {

        width = Screen.width;

        if (Screen.width < 1080)
        {
            ratio = 1080f / (float)Screen.width;
        }
        Debug.Log(ratio);
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
            Debug.Log(obj.name);
            BallControllUI ui = obj.GetComponentInParent<BallControllUI>(); // 버튼 부모가 UI임
            Debug.Log($"{Input.mousePosition.x + 95 / ratio}     /     {(ui.rt.anchoredPosition.x + width - (1000 / ratio))}");
            insertIndex = Input.mousePosition.x + 95 / ratio < ui.rt.anchoredPosition.x + (width - 1000 / ratio) / 2 ? ui.order : ui.order + 1;
        }
        else
        {
            insertIndex = Input.mousePosition.x + 95 / ratio < (width - 1000/ ratio) / 2 + gm.ballUIList.Count - 1  * 190 ? 0 : 10;
        }

        insertIndex = Mathf.Clamp(insertIndex, 0, gm.ballUIList.Count - 1);
        Debug.Log(insertIndex);

        for (int i = insertIndex; i < gm.ballUIList.Count; i++)
        {
            gm.ballUIList[i].order++;
        }

        ballControllUI.order = insertIndex;

        gm.BallUiSort();
        ballControllUI.transform.SetSiblingIndex(insertIndex);
        ballControllUI.directionImg.transform.DOScaleX(1, 0.45f);
                                                               
        ballControllUI.rt.DOSizeDelta(new Vector2(190, 190), 0.45f).OnComplete(() =>
        {
            gm.ballUIList.ForEach((x) => x.SetInteractValues(true));
        });

        ballControllUI = null;
        gameObject.SetActive(false);
    }
}
