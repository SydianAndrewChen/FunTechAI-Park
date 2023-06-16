using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    // Get Component
    ScrollRect rect;
    public int totalCount;
    private float[] posArray;

    private float targetPos;
    private int targetIndex = 0;
    private bool isDrag = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        Vector2 pos = rect.normalizedPosition;
        float x = Mathf.Abs(pos.x - posArray[0]);
        targetIndex = 0;
        for (int i = 1; i < totalCount; ++i)
        {
            float tmp = Mathf.Abs(pos.x - posArray[i]);
            if (tmp < x)
            {
                x = tmp;
                targetIndex = i;
            }
        }
        targetPos = posArray[targetIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<ScrollRect>();
        posArray = new float[totalCount];
        for (int i = 0; i < totalCount-1; i++)
        {
            posArray[i] = (float)i / (totalCount-1);
        }
        posArray[totalCount - 1] = 1.0f;

        var playerList = GetComponentsInChildren<BookAudioPlayer>();
        foreach (var player in playerList)
        {
            player.playButton.onClick.AddListener(FreezeScrolling);
            player.pauseButton.onClick.AddListener(UnfreezeScrolling);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrag)
        {
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targetPos, Time.deltaTime * 4);
        }
    }

    public void FreezeScrolling()
    {
        GetComponent<ScrollRect>().horizontal = false;
    }

    public void UnfreezeScrolling()
    {
        GetComponent<ScrollRect>().horizontal = true;
    }
}
