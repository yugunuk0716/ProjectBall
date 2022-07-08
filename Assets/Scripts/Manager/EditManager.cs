using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[System.Serializable]
public class NodeClass
{
    public string name;
    public string sprite;
    public float x;
    public float y;
    public TileType tile;

    public List<NodeClass> data;
}

public class EditManager : MonoBehaviour
{
    public static EditManager instance;

    private Dictionary<string, GameObject> dicPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> dictSprites = new Dictionary<string, Sprite>();
    public List<GameObject> listPrefabs = new List<GameObject>();
    public GameObject tilePrefab;
    private Sprite[] tileSprites;


    private void Start()
    {
        instance = this;

        dicPrefabs = new Dictionary<string, GameObject>();
        foreach (var obj in listPrefabs)
        {
            dicPrefabs[obj.name] = obj;
        }

        tileSprites = Resources.LoadAll<Sprite>("Platforms");
        dictSprites = new Dictionary<string, Sprite>();
        foreach (var spr in tileSprites)
        {
            dictSprites.Add(spr.name, spr);
        }
    }

#if UNITY_EDITOR


    [MenuItem("Stage/Save/Save as Txt")]
    static void SaveAs()
    {
        Debug.Log("Saving current edit...");

        if (instance)
        {
            string[] filters = { "Text", "txt", "Json", "json", "All Files", "*" };
            var path = EditorUtility.SaveFilePanel("Save Stage as txt", "", "stage0000.txt", "txt");

            if (path.Length != 0)
            {
                string str = instance.StringfyStage();
                File.WriteAllText(path, str);
            }
        }
    }


    [MenuItem("Stage/Load/Load Txt")]
    static void Load()
    {
        Debug.Log("Load edit...");

        if (instance)
        {
            var path = EditorUtility.OpenFilePanel("Load from txt", "", "txt");

            if (path.Length != 0)
            {
                string str = File.ReadAllText(path);
                NodeClass stage = new NodeClass();
                stage = JsonUtility.FromJson<NodeClass>(str);
                instance.ParseNode(stage);
            }


        }

    }



#endif



    private void ParseNode(NodeClass node, GameObject parent = null)
    {
        if (parent == null)
        {
            parent = new GameObject("NewStage");
        }

        if (node != null)
        {
            if (node.data != null && node.data.Count > 0)
            {
                GameObject currentObj;

                currentObj = MakeNode(node, parent);
                foreach (var child in node.data)
                {
                    ParseNode(child as NodeClass, currentObj);
                }
            }
            else
            {
                MakeNode(node, parent);
            }
        }
    }

    public GameObject MakeNode(NodeClass node, GameObject parent)
    {
        GameObject obj = null;

        if (parent != null)
        {
            if (dicPrefabs.ContainsKey(node.name))
            {
                obj = Instantiate(dicPrefabs[node.name]);
                obj.transform.position = new Vector2(node.x, node.y);
                obj.transform.SetParent(parent.transform);
            }
            else
            {
                if (node.sprite != null)
                {
                    obj = MakeSpriteNode(node, parent);
                }
                else
                {
                    obj = new GameObject(node.name);
                    obj.transform.SetParent(parent.transform);
                }
            }
        }

        return obj;
    }


    public GameObject MakeSpriteNode(NodeClass node, GameObject parent)
    {
        GameObject obj = null;

        if (parent != null)
        {
            obj = Instantiate(tilePrefab);


            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            obj.name = node.name;

            if (dictSprites.ContainsKey(node.sprite))
            {
                sr.sprite = dictSprites[node.sprite];
            }
            else
            {
                Debug.Log("Sprite no Found" + node.sprite);
            }

            obj.transform.SetParent(parent.transform);
            obj.transform.position = new Vector2(node.x, node.y);
        }

        return obj;
    }

    private void Parse(string str)
    {
        int idx = 0;
        ParseNode(str, idx);
    }

    public bool ParseNode(string str, int idx)
    {
        if (str[idx] == '{')
        {
            int st = idx;
            int endSt = 0;
            int nextSt = str.IndexOf('{', idx + 1);

            if (nextSt >= idx + 1)
            {
                ParseNode(str, nextSt);
            }
            else
            {
                endSt = str.IndexOf('}', idx + 1);
                if (endSt >= idx + 1)
                {
                    string nodeStr = str.Substring(idx + 1, endSt);
                    string[] data = nodeStr.Split(',');


                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = data[i].Replace("}", "");
                        data[i] = ParseItem(data[i]);
                        print(data[i]);
                    }
                }
                else
                {
                    Debug.LogError("Parsing Error");
                }
            }

        }
        else
        {
            Debug.LogError("?");
        }

        return true;
    }

    private string ParseItem(string str)
    {
        string result = string.Empty;

        if (str.IndexOf(':') >= 1)
        {
            result = str.Split(':')[1];
        }

        return result;
    }

    private string StringfyStage()
    {
        string result = string.Empty;
        GameObject stage = GameObject.Find("Stage");

        if (stage != null)
        {
            result += StringfyNode(stage, 0);

        }
        return result;
    }

    private string SetIndent(int indent)
    {
        string result = string.Empty;
        for (int j = 0; j < indent; j++)
        {
            result += " ";
        }

        return result;
    }

    private string StringfyNode(GameObject node, int indent)
    {
        string result = SetIndent(indent) + "{";
        result += $"\"name\":\"{node.transform.name}\", \"data\":[\n";

        for (int i = 0; i < node.transform.childCount; i++)
        {
            Transform tr = node.transform.GetChild(i);
            SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (i > 0)
                {
                    result += ",\n";
                }

                result += SetIndent(indent + 1);
                result += $"{{\"name\":" + "\"" + tr.name + "\"" + ", \"sprite\":" + "\"" + sr.sprite.name + "\"" + ", \"x\":" + "\"" + tr.position.x + "\"" + ", \"y\":" + "\"" + tr.position.y + "\"}";
            }
            else
            {
                result += StringfyNode(tr.gameObject, indent + 1);
            }

        }
        result += "]}";

        return result;
    }

}
