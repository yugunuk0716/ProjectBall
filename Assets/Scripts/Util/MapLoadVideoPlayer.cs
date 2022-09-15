using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapLoadVideoPlayer : MonoBehaviour
{
    public Texture2D[] texes = new Texture2D[200];

    public RawImage img;
    public RenderTexture rt;



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
            var texture2D = new Texture2D(rt.width, rt.height);
            texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture2D.Apply();
            texes[i] = texture2D;
            yield return new WaitForSeconds(0.02f);
        }

        Debug.Log("텍스쳐 저장 완료");
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
