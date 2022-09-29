using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TileHelpUI : MonoBehaviour
{
    [SerializeField] Button onBtn;
    [SerializeField] RectTransform scrollView; // this

    bool isViewing = false;
    bool isMoving = false;

    float width;
    private void Awake()
    {
        float ratio = 1f;
        if (Screen.width < 1080)
        {
            ratio = 1080f / (float)Screen.width;
        }
        width = Screen.width * ratio;

        scrollView.anchoredPosition = new Vector3(width, scrollView.anchoredPosition.y, 0);

        onBtn.onClick.AddListener(() =>
        {
            if(isMoving)
            {
                return;
            }

            isMoving = true;
            scrollView.DOAnchorPosX(isViewing ? width : 0, 1f).OnComplete(() =>
            {
                isMoving = false;
            });
            isViewing = !isViewing;
        });
    }
}
