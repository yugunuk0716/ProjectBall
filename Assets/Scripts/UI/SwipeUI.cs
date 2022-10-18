using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SwipeUI : MonoBehaviour
{
	[SerializeField] private Scrollbar scrollBar;
    [SerializeField] private Image[] circleContents;

    private float swipeTime = 0.2f;           
	private float[] scrollPageValues;         
	private float valueDistance = 0;          
	private float startTouchX;                
	private float endTouchX;               
    private float circleContentScale = 0.6f;

    private int currentPage = 0;
    private int maxPage = 0;

    bool isFirstTexting = true;
	[HideInInspector] public bool isSwipeMode = false;

    [Header("Explain")]
    [SerializeField] [TextArea()]string[] descriptions;
    [SerializeField] Text text;

    [HideInInspector] public CanvasGroup parentCvsGroup;

    private void Awake()
	{
		scrollPageValues = new float[transform.childCount];
		valueDistance = 1f / (scrollPageValues.Length - 1f);

		for (int i = 0; i < scrollPageValues.Length; ++i)
		{
			scrollPageValues[i] = valueDistance * i;
		}

		maxPage = transform.childCount;
        SetScrollBarValue(0);
        UpdateCircleContent();
    }

    public void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value = scrollPageValues[index];
	}

	private void Update()
	{
        if (parentCvsGroup.alpha != 1) return;

        if(isFirstTexting)
        {
            Explain();
            isFirstTexting = false;
        }

		UpdateInput();
        UpdateCircleContent();

    }

    private void UpdateInput()
	{

		if (isSwipeMode == true) return;

#if UNITY_EDITOR

		if (Input.GetMouseButtonDown(0))
		{
			startTouchX = Input.mousePosition.x;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			endTouchX = Input.mousePosition.x;

			UpdateSwipe();
		}
#endif

#if UNITY_ANDROID
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{

				startTouchX = touch.position.x;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
	
				endTouchX = touch.position.x;

				UpdateSwipe();
			}
		}
#endif
	}

	public void UpdateSwipe(bool isBtnPressed = false, bool isLeft_btnPressed = false)
	{
        bool isLeft;

        if (isBtnPressed)
        {
            isLeft = isLeft_btnPressed;
        }
        else if (Mathf.Abs(startTouchX - endTouchX) > (float)Screen.width / 20
            && EventSystem.current.currentSelectedGameObject == null)
		{
            isLeft = startTouchX < endTouchX;
		}
        else
        {
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        Debug.Log($"Pressed : {isBtnPressed}, isLeft_Btn : {isLeft_btnPressed}, LEFT : {isLeft}");


        if (isLeft)
		{
            if (currentPage == 0)
            {
                return;
            }
            else
            {
                --currentPage;
            }
        }
		else
		{
			if (currentPage == maxPage - 1)
            {
                return;
            }
            else
            {
                ++currentPage;
            }
        }


		StartCoroutine(OnSwipeOneStep(currentPage, true));
	}


	private IEnumerator OnSwipeOneStep(int index, bool bUpdateText = false)
	{
		float start = scrollBar.value;
		float current = 0;
		float percent = 0;

		isSwipeMode = true;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

			yield return null;
		}

		isSwipeMode = false;

        if(bUpdateText)
        {
            Explain();
        }
    }


    private void Explain()
    {
        text.DOKill();
        text.text = string.Empty;
        text.DOText(descriptions[currentPage], 1.5f);
    }

	private void UpdateCircleContent()
	{
        int index = 0;
        int nextIndex = 0;

        for (int i = 0; i < scrollPageValues.Length; ++i)
		{
            if (scrollBar.value < scrollPageValues[i] + (valueDistance / 2) && scrollBar.value > scrollPageValues[i] - (valueDistance / 2))
			{
                index = i;
                nextIndex = i + (scrollBar.value >= scrollPageValues[i] ? 1 : -1);
                if(nextIndex == 5)
                {
                    index = 4;
                    nextIndex = 3;
                }

                float a = 0, b = 0;

                if (index > nextIndex)
                {
                    int temp = index;
                    index = nextIndex;
                    nextIndex = temp;
                }

                index = Mathf.Clamp(index, 0, scrollPageValues.Length);
                nextIndex = Mathf.Clamp(nextIndex, 0, scrollPageValues.Length);

                a = scrollBar.value - scrollPageValues[index];
                b = scrollPageValues[nextIndex] - scrollBar.value;
                float c = 1 + circleContentScale * a / valueDistance;
                float d = 1 + circleContentScale * b / valueDistance;
                circleContents[index].transform.localScale = Vector3.one * d;
                circleContents[nextIndex].transform.localScale = Vector3.one * c;

                circleContents[index].color = new Color(1, 1, 1, c - 1);
                circleContents[nextIndex].color = new Color(1, 1, 1, d - 1);

                break;
            }
		}
	}
}