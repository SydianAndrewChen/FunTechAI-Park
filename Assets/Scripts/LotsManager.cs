using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMADatePicker;

public class LotsManager : MonoBehaviour
{

    private bool hasSetupDate = false;
    public GameObject firstTimeGroup;
    public GameObject afterFirstTimeGroup;
    public GameObject datePicker;
    public GameObject waitAnime;
    public GameObject lotsController;
    private ADP m_ADP;

    private bool isDrawing;

    private int day;
    private int month;
    private int year;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Month"))
        {
            hasSetupDate = true;
            Destroy(firstTimeGroup);
            afterFirstTimeGroup.SetActive(true);
            year = PlayerPrefs.GetInt("Year");
            month = PlayerPrefs.GetInt("Month");
            day = PlayerPrefs.GetInt("Day");
        }
        else
        {
            firstTimeGroup.SetActive(true);
            afterFirstTimeGroup.SetActive(false);
            m_ADP = datePicker.GetComponent<ADP>();
        }

    }

    void Update()
    {
        if (isDrawing)
        {
            waitAnime.transform.RotateAround(waitAnime.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 50.0f * Time.deltaTime);

        }
    }

    public void SetupDate()
    {
        var date = m_ADP.GetDate();
        PlayerPrefs.SetInt("Year", date.yyyy);
        PlayerPrefs.SetInt("Month", date.MM);
        PlayerPrefs.SetInt("Day", date.dd);
        hasSetupDate = true;
        firstTimeGroup.SetActive(false);
        afterFirstTimeGroup.SetActive(true);
    }

    public void ClearDate()
    {
        PlayerPrefs.DeleteKey("Year");
        PlayerPrefs.DeleteKey("Month");
        PlayerPrefs.DeleteKey("Day");
        hasSetupDate = false;
    }

    public void DrawLotRequest()
    {
        StartCoroutine(TestCoroutine());

    }

    IEnumerator TestCoroutine()
    {
        isDrawing = true;
        yield return new WaitForSeconds(2.0f);
        waitAnime.SetActive(false);
        isDrawing = false;
        lotsController.GetComponent<LotsController>().DrawLots();
        Debug.Log("Finished");
    }
}
