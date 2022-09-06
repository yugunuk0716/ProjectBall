using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BallAfterImage : MonoBehaviour
{
    Sprite sprite;
    GameObject parentObj;

    SpriteRenderer[] renderers = new SpriteRenderer[20];
    SpriteRenderer[] renderers2 = new SpriteRenderer[5];

    bool bSettingCompleted = false;

    private void OnEnable()
    {
        if(bSettingCompleted)
        {
            StartCoroutine(MoveAfterImageEffect(renderers2));
            StartCoroutine(RotateAfterImageEffect(renderers));
        }
    }

    private void OnDisable()
    {
        if (bSettingCompleted)
        {
            StopCoroutine(MoveAfterImageEffect(renderers2));
            StopCoroutine(RotateAfterImageEffect(renderers));
        }
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        parentObj = new GameObject("Test");
        parentObj.transform.position = this.transform.position;

        for (int i = 0; i < renderers.Length; i++)
            SetSprite(i, renderers);

        for (int i = 0; i < renderers2.Length; i++)
            SetSprite(i, renderers2);

        StartCoroutine(CallMyRotateAfterImages(renderers));

        bSettingCompleted = true;
    }

    void SetSprite(int index, SpriteRenderer[] targetArray)
    {
        GameObject obj = new GameObject($"ImgObj{index}");
        obj.transform.SetParent(parentObj.transform);
        obj.transform.localScale = transform.localScale;
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.clear;
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

    IEnumerator CallMyRotateAfterImages(SpriteRenderer[] renderers)
    {
        while(true)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].transform.position = this.transform.position;
            yield return new WaitForSeconds(0.05f);
        }
    }
    
}
