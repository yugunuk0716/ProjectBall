using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudHandler : MonoBehaviour
{
    private List<Cloud> clouds = new List<Cloud>();
    
    public void CloudMove()
    {
        for (int i = 0; i < 7; i++)
        {
            Cloud cloud = PoolManager.Instance.Pop("Cloud") as Cloud;
            ResetCloud(cloud);
            clouds.Add(cloud);
        }
    }

    public void ResetCloud(Cloud cloud)
    {
        cloud.transform.position = new Vector3(10f, Random.Range(-12f, 10f), 0);
        cloud.gameObject.SetActive(true);
        SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();
        sr.DOFade(Random.Range(.5f, 1), .5f);

        cloud.transform.DOMove(new Vector3(-10, cloud.transform.position.y + 5), Random.Range(3f,10f)).SetEase(Ease.Linear).OnComplete(() =>
        {
            ResetCloud(cloud);
        });
    }
    
}
