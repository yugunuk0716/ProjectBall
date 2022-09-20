using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageInfoUI : UIBase
{
    public TextMeshProUGUI stageIndexText;
    public Button enterButton;
    public Image[] starImages;

    public override void Init()
    {
        GetCanvasGroup();

    }

    public override void Load()
    {
    
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
