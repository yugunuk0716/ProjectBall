using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapLoadVideoPlayer : MonoBehaviour
{
    public Texture2D[] texes = new Texture2D[200];
    // 나중에 미리 생성해두고 그걸 초기화하는 느낌으로 ㄱㄱ

    public RawImage img;
    public RenderTexture rt;

    private void Awake()
    {
        for (int i = 0; i < 200; i++)
        {
            texes[i] = new Texture2D(rt.width, rt.height); // 전체적으로 생성
        }
    }


    public void PlayVideo()
    {
        StartCoroutine(CoPlayVideo());
    }  
    
    public void TakeVideo()
    {
        StartCoroutine(CoTakeVideo());
    }

    IEnumerator CoTakeVideo()
    {
        for (int i = 0; i < texes.Length; i++)
        {
            RenderTexture.active = rt;
            texes[i].ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texes[i].Apply();
            yield return new WaitForSeconds(0.02f);
        }

        Debug.Log("완료");
    }

    IEnumerator CoPlayVideo()
    {
        for (int i = 0; i < texes.Length; i++)
        {
            img.texture = texes[i];
            yield return new WaitForSeconds(0.02f);
        }
    }
}
