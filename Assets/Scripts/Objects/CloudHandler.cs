using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudHandler : MonoBehaviour
{
   
    private List<Cloud> clouds = new List<Cloud>();
    
    public IEnumerator CloudMove()
    {
        for (int i = 0; i < 12; i++)
        {
            Cloud cloud = PoolManager.Instance.Pop("Cloud") as Cloud;
            ResetCloud(cloud);
            clouds.Add(cloud);
            yield return new WaitForSeconds(Random.Range(.5f, 3f));
        }
    }

    public void ResetCloud(Cloud cloud)
    {
        cloud.transform.position = new Vector3(10f, Random.Range(-12f, 8), 0);
        cloud.gameObject.SetActive(true);
        SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();
        sr.sprite = cloud.GetSprite();

        float sacle = Random.Range(.5f, 1.5f);
        float alpha;
        float speed;

        cloud.transform.localScale = new Vector3(sacle, sacle, 0);

        if (sacle <= .8f)
        {
            alpha = Random.Range(.7f, 1f);
            sr.sortingOrder = -(int)(100 + alpha);
            speed = Random.Range(10f, 15f);
        }
        else
        {
            alpha = Random.Range(.3f, .6f);
            sr.sortingOrder = (int)(20 + alpha);
            speed = Random.Range(5f, 10f);
        }

        sr.DOFade(alpha, .5f);

        cloud.transform.DOMove(new Vector3(-10, cloud.transform.position.y + 5), speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            ResetCloud(cloud);
        });
    }
    
}
