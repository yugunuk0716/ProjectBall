using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TMPTester : MonoBehaviour
{

    public GameObject tmap;

    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = tmap.GetComponent<Tilemap>();
        TilemapRenderer tmapR = tmap.GetComponent<TilemapRenderer>();

        string s = JsonUtility.ToJson(tilemap);
        string s2 = JsonUtility.ToJson(tmapR);
        print(s);
        print(s2);

    }

    // Update is called once per frame
  
}
