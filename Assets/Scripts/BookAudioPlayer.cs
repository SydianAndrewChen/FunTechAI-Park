using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BookAudioPlayer : MonoBehaviour
{
    public Button playButton;
    public Button pauseButton;
    public GameObject bookCover;
    public float rotationSpeed = 10.0f;
    private bool isRotating;
    // Start is called before the first frame update
    void Start()
    {
/*        bookCover.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 10.0f);
        bookCover.transform.DORotateQuaternion(Quaternion.Euler(0.0f, 0.0f, 360.0f), 10.0f);*/
        playButton.onClick.AddListener(delegate
        {
            isRotating = true;
        });
        pauseButton.onClick.AddListener(delegate
        {
            isRotating = false;
            bookCover.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        });
    }

    // Update is called once per frame
    void Update()
    {

        if (isRotating) bookCover.transform.RotateAround(bookCover.transform.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }
}
