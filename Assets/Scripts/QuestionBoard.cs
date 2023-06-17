using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionBoard : MonoBehaviour
{

    public Button QuestionButton;
    public Button[] RecQuestionButtons;
    public TMP_InputField InputQuestions;

    [HideInInspector]
    public GameObject chosenCard;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var recQuestionButton in RecQuestionButtons)
        {
            recQuestionButton.onClick.AddListener(delegate
            {
                InputQuestions.text = recQuestionButton.GetComponentInChildren<TMP_Text>().text;
            });
        }

        QuestionButton.onClick.AddListener(SendQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendQuestion()
    {
        /* Some questions to send*/


        /* Wait for a sec and play animation */

        /* Get something back from backend */

        StartCoroutine(SendQuestionRequest());
    }


    ChatGPTRequestStruct GenerateMessages(WishCard card)
    {
        var s = new ChatGPTRequestStruct();
        string q1 = string.Format("Answer the input question with regard to the following Tarot card:\n {0}", card.CardFrontText.text);
        string q2 = InputQuestions.text;
        s.messages = new ChatGPTMessageStruct[2]
        {
            new ChatGPTMessageStruct("system", q1),
            new ChatGPTMessageStruct("user", q2)
        };
        return s;
    }

    IEnumerator SendQuestionRequest()
    {

        string answer = "Test answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\n";

        ChatGPTRequest r = new ChatGPTRequest();
        
        ChatGPTRequestStruct rs = GenerateMessages(chosenCard.GetComponent<WishCard>());
        string jsonString = JsonUtility.ToJson(rs);
        Debug.Log(jsonString);
        yield return r.Post(jsonString);
        chosenCard.GetComponent<WishCard>().CardFrontText.SetText(r.response);
        gameObject.SetActive(false);
        chosenCard.GetComponent<WishCard>().CardFront.SetActive(true);
    }
}
