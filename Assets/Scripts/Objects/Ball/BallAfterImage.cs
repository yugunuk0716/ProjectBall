using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BallAfterImage : MonoBehaviour
{
    SpriteRenderer _sr;
    GameObject parentObj;

    SpriteRenderer[] renderers = new SpriteRenderer[20];
    SpriteRenderer[] renderers2 = new SpriteRenderer[5];

    bool bSettingCompleted = false;

    private void OnEnable()
    {
        Debug.Log("켜짐");
        if (bSettingCompleted)
        {
            StartCoroutine(MoveAfterImageEffect(renderers2));
            StartCoroutine(RotateAfterImageEffect(renderers));
            StartCoroutine(CallMyAfterImages(renderers));
            StartCoroutine(CallMyAfterImages(renderers2));
        }
    }

    private void OnDisable()
    {
        if (bSettingCompleted)
        {
            StopCoroutine(MoveAfterImageEffect(renderers2));
            StopCoroutine(RotateAfterImageEffect(renderers));
            StopCoroutine(CallMyAfterImages(renderers));
            StopCoroutine(CallMyAfterImages(renderers2));
        }
    }

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        
        parentObj = new GameObject("Test");
        parentObj.transform.position = this.transform.position;

        for (int i = 0; i < renderers.Length; i++) SetSprite(i, renderers);
        for (int i = 0; i < renderers2.Length; i++) SetSprite(i, renderers2);

        //OnEnable에서 처음엔 인식 못하게 막아뒀음
        StartCoroutine(MoveAfterImageEffect(renderers2));
        StartCoroutine(RotateAfterImageEffect(renderers));
        StartCoroutine(CallMyAfterImages(renderers));
        StartCoroutine(CallMyAfterImages(renderers2));

        bSettingCompleted = true;
    }

    void SetSprite(int index, SpriteRenderer[] targetArray)
    {
        GameObject obj = new GameObject($"ImgObj{index}");
        obj.transform.SetParent(parentObj.transform);
        obj.transform.localScale = transform.localScale;
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = _sr.sprite;
        sr.color = _sr.color;
        targetArray[index] = sr;
    }

    IEnumerator MoveAfterImageEffect(SpriteRenderer[] renderers)
    {
        while (true)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].transform.rotation = transform.rotation;
                renderers[i].transform.position = transform.position;
                renderers[i].color = Color.white;
                renderers[i].DOFade(0, 1f);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    IEnumerator RotateAfterImageEffect(SpriteRenderer[] renderers)
    {
        while(true)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].transform.rotation = transform.rotation;
                renderers[i].color = Color.white;
                renderers[i].DOFade(0, 0.7f);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    IEnumerator CallMyAfterImages(SpriteRenderer[] renderers)
    {
        while(true)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].transform.position = this.transform.position;
            yield return new WaitForSeconds(0.05f);
        }
    }
    
}
