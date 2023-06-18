using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMADatePicker;
using TMPro;

public class LotsManager : MonoBehaviour
{

    private bool hasSetupDate = false;
    public GameObject firstTimeGroup;
    public GameObject afterFirstTimeGroup;
    public GameObject datePicker;
    public GameObject waitAnime;
    public GameObject lotsDrawer;
    private LotsController lotsController;
    private ADP m_ADP;
    private PlayerInfo playerInfo;

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

        lotsController = lotsDrawer.GetComponent<LotsController>();
        playerInfo = new PlayerInfo();
        playerInfo.year = PlayerPrefs.GetInt("Year");
        playerInfo.month = PlayerPrefs.GetInt("Month");
        playerInfo.day = PlayerPrefs.GetInt("Day");
    }

    void Update()
    {
        if (isDrawing)
        {
            waitAnime.transform.RotateAround(waitAnime.transform.position, new Vector3(0.0f, 0.0f, 1.0f), -50.0f * Time.deltaTime);
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

        StartCoroutine(WishRequest());/*
        StartCoroutine(WishRequest());*/
        /*
        StartCoroutine(r.WishPost(playerInfo, questions));*/    
    }
    ChatGPTRequestStruct GenerateMessages(PlayerInfo playerInfo)
    {
        var s = new ChatGPTRequestStruct();
        s.messages = new ChatGPTMessageStruct[2] 
        {
            new ChatGPTMessageStruct("system", "You are an astrological fortune teller specializing in love predictions. Based on the player's gender, age, birthday, and other information, you predict the potential issues the player may encounter and forecast their luck for the day. You have to infer the zodiac of the user by the date of birth (by Year-Month-Day). The output consists of three parts: 1.A brief description of the day's fortune and possible situations, not exceeding 75 words; 2. Advice on areas the player should pay attention to, providing meaningful suggestions within 75 words; 3. A blessing for the player, not exceeding 10 words."),
            new ChatGPTMessageStruct("user", string.Format("birthday: {0}.{1}.{2}, gender:male, mood: good", playerInfo.year, playerInfo.month, playerInfo.day))
        };
        return s;
    }

    IEnumerator WishRequest()
    {
        ChatGPTRequest r = new ChatGPTRequest();
        Debug.Log(playerInfo);
        waitAnime.SetActive(true);
        isDrawing = true;/*
        yield return new WaitForSeconds(2.0f);*/
        ChatGPTRequestStruct rs = GenerateMessages(playerInfo);
        string jsonString = JsonUtility.ToJson(rs);
        Debug.Log(jsonString);
        yield return r.Post(jsonString);
        isDrawing = false;
        waitAnime.SetActive(false);
        lotsDrawer.GetComponent<LotsController>().DrawLots();
        lotsController.GetComponentInChildren<TMP_Text>().SetText(r.response);
    }



    IEnumerator TestCoroutine()
    {
        waitAnime.SetActive(true);
        isDrawing = true;/*
        yield return new WaitForSeconds(2.0f);*/
        yield return new WaitForSeconds(2.0f);
        waitAnime.SetActive(false);
        isDrawing = false;
        waitAnime.SetActive(false);
        lotsDrawer.GetComponent<LotsController>().DrawLots();
    }
}
