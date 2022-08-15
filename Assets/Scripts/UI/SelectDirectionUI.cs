using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDirectionUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [HideInInspector] public Ball addBall; // 추가할 공.. 여기서 가지고 있으면 날먹이 가능해여

    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
        Button[] selectDirectionBtns = GetComponentsInChildren<Button>();

        for (int i = 0; i< selectDirectionBtns.Length; i++) // 0번은 Nothing 들어갑니다.
        {
            int index = 1;

            for (int j = 0; j < i; j++)
                index <<= 1;

            selectDirectionBtns[i].onClick.AddListener(() =>
            {
                addBall.shootDir = (TileDirection)(index);
                IsometricManager.Instance.GetManager<GameManager>().myBallList.Add(addBall);
                ScreenOn(false);
            });
        }
    }

    public void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;
    }
}
