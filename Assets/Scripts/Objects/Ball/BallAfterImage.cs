using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BallAfterImage : MonoBehaviour
{
    Sprite sprite;

    GameObject parentObj;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        SpriteRenderer[] renderers = new SpriteRenderer[20];

        parentObj = new GameObject("Test");

        for (int i = 0; i < renderers.Length; i++)
        {
            SetSprite(i, renderers);
        }

        StartCoroutine(RotateAfterImageEffect(renderers));
        StartCoroutine(CallMyRotateAfterImages(renderers));

        SpriteRenderer[] renderers2 = new SpriteRenderer[5];

        for (int i = 0; i < renderers2.Length; i++)
        {
            SetSprite(i, renderers2);
        }

        StartCoroutine(MoveAfterImageEffect(renderers2));
    }

    void SetSprite(int index, SpriteRenderer[] targetArray)
    {
        GameObject obj = new GameObject($"ImgObj{index}");
        obj.transform.SetParent(parentObj.transform);
        obj.transform.localScale = transform.localScale;
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
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
                yield return new WaitForSeconds(0.5f);
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
                renderers[i].DOFade(0, 2f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator CallMyRotateAfterImages(SpriteRenderer[] renderers)
    {
        while(true)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].transform.position = this.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    void Update()
    {
        this.transform.Rotate(Vector3.forward * 80 * Time.deltaTime);
    }
}
