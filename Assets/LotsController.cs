using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotsController : MonoBehaviour
{
    public ScrollRect rect;
    private bool isDropping;
    // Start is called before the first frame update
    void Start()
    {
        isDropping = false;
        rect = GetComponent<ScrollRect>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropping)
        {
            rect.verticalNormalizedPosition = Mathf.Lerp(rect.verticalNormalizedPosition, 1.0f, Time.deltaTime * 4.0f);
        }
    }

    public void DrawLots()
    {
        isDropping = true;
    }
}
