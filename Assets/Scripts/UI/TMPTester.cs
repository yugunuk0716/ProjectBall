using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTester : MonoBehaviour
{
    public TextMeshProUGUI tmp1;
    public TextMeshProUGUI tmp2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(R());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator R()
    {
        yield return null;  
        Vector3[] vertices1 = tmp1.mesh.vertices;
        Vector3[] vertices2 = tmp2.mesh.vertices;

        for (int i = 0; i < vertices1.Length; i++)
        {
            vertices1[i] += new Vector3(i * 0.1f, i * 0.1f, i * 0.1f);
            print(vertices1[i]);

        }

        for (int i = 0; i < vertices2.Length; i++)
        {
            vertices2[i] -= new Vector3(i * 0.1f, i * 0.1f, i * 0.1f);
            print(vertices2[i]);
        }
        Mesh mesh1 = new Mesh();
        mesh1.vertices = vertices1;

        Mesh mesh2 = new Mesh();
        mesh2.vertices = vertices2;



        tmp1.canvasRenderer.SetMesh(mesh1);
        tmp2.canvasRenderer.SetMesh(mesh2);
    }
}
