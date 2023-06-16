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
    private ADP m_ADP;

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

    // Update is called once per frame
    void Update()
    {
        
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

    
}
