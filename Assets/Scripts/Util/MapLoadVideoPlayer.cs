using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapLoadVideoPlayer : MonoBehaviour
{
    int videoLength = 100;
    private Texture2D[] texes;

    public RawImage img;
    public RenderTexture rt;

    Coroutine playVideo;

    WaitForSeconds ws;

    private void Awake()
    {
        ws = new WaitForSeconds(2f / videoLength);
        texes = new Texture2D[videoLength];
        for (int i = 0; i < videoLength; i++)
        {
            texes[i] = new Texture2D(rt.width, rt.height); // 전체적으로 생성
        }
    }

    public void PlayVideo()
    {
        if(playVideo != null)
        {
            StopCoroutine(playVideo);
        }
        playVideo = StartCoroutine(CoPlayVideo());
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
            yield return ws;
        }
    }

    IEnumerator CoPlayVideo()
    {
        for (int i = 0; i < texes.Length; i++)
        {
            img.texture = texes[i];
            yield return ws;
        }
        playVideo = null;
    }
}
