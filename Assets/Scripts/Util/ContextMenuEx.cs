using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuEx : MonoBehaviour
{
    public RectTransform targetRectTr;
    public Camera uiCamera;
    public RectTransform menuUITr;

    private Vector2 screenPoint;
    private void Start()
    {
        targetRectTr = GetComponent<RectTransform>();
        uiCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
            menuUITr.localPosition = screenPoint;
        }
    }
}