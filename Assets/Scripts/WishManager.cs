using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishManager : MonoBehaviour
{
    public int totalWishCount = 10;
    public GameObject wishObject;
    public Transform rotateCenter;

    public float rotateSpeed = 1000.0f;
    private float radius = 14.0f;
    private List<GameObject> wishObjectList;
    // Start is called before the first frame update
    void Start()
    {
        wishObjectList = new List<GameObject>();
        for (int i = 0; i < totalWishCount; ++i)
        {
            GameObject obj = Instantiate(wishObject);
            obj.name = string.Format("Card {0}", i);
            float t = (float)(i - totalWishCount / 2) / totalWishCount;
            obj.transform.RotateAround(rotateCenter.position, new Vector3(0.0f, 0.0f, 1.0f), t * totalWishCount * 13.5f);
            wishObjectList.Add(obj);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

    }

    void Rotate()
    {
        if (Input.GetMouseButton(0))
        {
            if (
                /* Set boundary at first and last card */
                Vector2.Distance(new Vector2(wishObjectList[0].transform.position.x, wishObjectList[0].transform.position.y), new Vector2(0, -radius)) < 20 && Input.GetAxis("Mouse X") > 0 ||
                Vector2.Distance(new Vector2(wishObjectList[wishObjectList.Count - 1].transform.position.x, wishObjectList[wishObjectList.Count - 1].transform.position.y), new Vector2(0, -radius)) < 20 && Input.GetAxis("Mouse X") < 0
                )
                return;
            Debug.Log(Vector2.Distance(new Vector2(wishObjectList[0].transform.position.x, wishObjectList[0].transform.position.y), new Vector2(0, -radius)));
            Debug.Log(Input.GetAxis("Mouse X"));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == transform)
            {
                Debug.Log("Hit"!);
                foreach (GameObject obj in wishObjectList)
                {
                    obj.transform.RotateAround(rotateCenter.position, new Vector3(0.0f, 0.0f, 1.0f), Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
                }
            }
        }
    }
}
