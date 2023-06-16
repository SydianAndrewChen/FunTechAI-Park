using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WishManager : MonoBehaviour
{
    public int totalWishCount = 10;
    public GameObject wishCard;
    public Transform rotateCenter;
    public Transform initPosition;
    public GameObject wishGroup;
    public GameObject questionBoard;

    public float rotateSpeed = 1000.0f;
    public bool hasWish = false;

    private float radius = 14.0f;
    private List<GameObject> wishCardList;

    private GameObject chosenCard;
    private float hitDistance;
    private Transform initCardTransform;
    private Vector3 beforeChosenPosition;
    private Vector3 beforeChosenRotation;

    public GameObject chosenCardHole;

    private bool isChoosing = false;

    // Start is called before the first frame update
    void Start()
    {
        chosenCard = null;
        hitDistance = -1;
        initCardTransform = wishCard.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotate();
        CheckPickCard();
    }

    public void ShowCard() 
    {
        if (hasWish) return;
        wishCardList = new List<GameObject>();
        for (int i = 0; i < totalWishCount; ++i)
        {
            GameObject obj = Instantiate(wishCard, wishGroup.transform);
            obj.name = string.Format("Card {0}", i);
            obj.transform.position = initPosition.position;
            float t = (float)(i - totalWishCount / 2) / totalWishCount;
            obj.transform.RotateAround(rotateCenter.position, new Vector3(0.0f, 0.0f, 1.0f), t * totalWishCount * 13.5f);
            obj.GetComponent<WishCard>().CardReturnButton.onClick.AddListener(HideCard);
            obj.GetComponent<WishCard>().CardQuestionButton.onClick.AddListener(delegate 
            { 
                questionBoard.SetActive(true);
                questionBoard.GetComponent<QuestionBoard>().chosenCard = obj;
                obj.GetComponent<WishCard>().CardFront.SetActive(false); 
            });
            wishCardList.Add(obj);


        }
        hasWish = true;
    }

    void CheckRotate()
    {
        if (wishCardList == null || isChoosing) return;
        if (Input.GetMouseButton(0))
        {
            if (
                /* Set boundary at first and last card */
                Vector2.Distance(new Vector2(wishCardList[0].transform.position.x, wishCardList[0].transform.position.y), new Vector2(0, -radius)) < 20 && Input.GetAxis("Mouse X") > 0 ||
                Vector2.Distance(new Vector2(wishCardList[wishCardList.Count - 1].transform.position.x, wishCardList[wishCardList.Count - 1].transform.position.y), new Vector2(0, -radius)) < 20 && Input.GetAxis("Mouse X") < 0
                )
                return;
/*            Debug.Log(Vector2.Distance(new Vector2(wishObjectList[0].transform.position.x, wishObjectList[0].transform.position.y), new Vector2(0, -radius)));
            Debug.Log(Input.GetAxis("Mouse X"));*/
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == transform)
            {
                foreach (GameObject obj in wishCardList)
                {
                    obj.transform.RotateAround(rotateCenter.position, new Vector3(0.0f, 0.0f, 1.0f), Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
                }
            }
        }
    }

    void CheckPickCard()
    {
        if (wishCardList == null) return;
        if (Input.GetMouseButtonDown(0) && !isChoosing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            
            if (hit.collider != null && hit.collider.gameObject.name.StartsWith("Card"))
            {
                Debug.Log("Hit card:");
                Debug.Log(hit.collider.gameObject.name);
                isChoosing = true;
                chosenCard = hit.collider.gameObject;
                hitDistance = hit.distance;
                beforeChosenPosition = hit.collider.gameObject.transform.position;
                beforeChosenRotation = hit.collider.gameObject.transform.rotation.eulerAngles;

                chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.5f, 0.1f);
                chosenCard.transform.DORotate(new Vector3(0, 0, 0), 1.0f);

            }
        }
        if (Input.GetMouseButton(0) && chosenCard != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            Vector3 mouseWorldPosition = ray.GetPoint(hitDistance);
            chosenCard.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, chosenCard.transform.position.z);
        }

            if (Input.GetMouseButtonUp(0) && chosenCard != null)
        {
            if (Vector3.Distance(chosenCard.transform.position, chosenCardHole.transform.position) < 30.0f)
            {
                chosenCard.transform.DORotate(chosenCardHole.transform.rotation.eulerAngles, 0.5f);
                chosenCard.transform.DOScale(chosenCardHole.transform.localScale, 0.5f);
                chosenCard.transform.DOMove(chosenCardHole.transform.position, 0.7f).OnComplete(delegate {
                    chosenCard.transform.DOScaleY(chosenCard.transform.localScale.y * -1, 1.0f);
                    chosenCard.transform.DOScaleZ(chosenCard.transform.localScale.z * -1, 1.0f).OnComplete(delegate {
                        chosenCard.transform.DOScale(chosenCard.transform.localScale * 3.0f, 1.0f);
                        chosenCard.transform.DOMoveX(0, 1.0f);
                        chosenCard.transform.DOMoveY(0, 1.0f);
                        chosenCard.GetComponent<WishCard>().CardFront.SetActive(true);
                        chosenCard = null;
                    });
                });

            }
            else
            {
                chosenCard.transform.DOScale(initCardTransform.localScale, 0.5f);
                chosenCard.transform.DORotate(beforeChosenRotation, 0.5f);
                chosenCard.transform.DOMove(beforeChosenPosition, 0.7f).OnComplete(delegate { isChoosing = false; });
                chosenCard = null;
                hitDistance = -1;
                beforeChosenPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

    }

    public void HideCard()
    {
        wishGroup.SetActive(false);
    }
}
