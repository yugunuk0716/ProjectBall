using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cradit : MonoBehaviour
{
    [SerializeField]
    private RectTransform cradits;

    public void OnEnable()
    {
        cradits.DOAnchorPos3DY(3000, 40f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
