using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public BallControllUI ballControllUI { get; set; } = null;
    [SerializeField] Image paddingObj;

    float width = 0f;
    float ratio = 1f;

    public int paddingIndex = 0;

    private void Start()
    {
        width = Screen.width;

        if (Screen.width < 1080)
        {
            ratio = 1080f / (float)Screen.width;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 1)
        {
            On(false);
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;

        int index = (int)((Input.mousePosition.x + (95 / ratio) - ((width - 1000 / ratio) / 2)) / (int)(190 / ratio));
        MovePaddingObj(index);
    }

    public void On(bool on)
    {
        gameObject.SetActive(on);
        paddingObj.gameObject.SetActive(on);

    }

    public void MovePaddingObj(int index)
    {
        if(paddingIndex != index)
        {
            paddingObj.transform.SetSiblingIndex(index);
            paddingIndex = index;
        }
    }

    public void OnEndDrag()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        
        paddingIndex = Mathf.Clamp(paddingIndex, 0, gm.ballUIList.Count - 1);

        for (int i = paddingIndex; i < gm.ballUIList.Count; i++)
        {
            gm.ballUIList[i].order++;
        }

        ballControllUI.order = paddingIndex;

        gm.BallUiSort();
        ballControllUI.transform.SetSiblingIndex(paddingIndex);

        gm.ballUIList.ForEach((x) => x.SetInteractValues(false));

        On(false);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(ballControllUI.directionImg.rectTransform.DOScaleX(1, 0.4f));
        seq.Join(ballControllUI.rt.DOSizeDelta(new Vector2(190, 190), 0.4f).OnComplete(() =>
        {
            gm.ballUIList.ForEach((x) => x.SetInteractValues(true));
            ballControllUI = null;
            GameManager.canInteract = true;
        }));
    }
}
