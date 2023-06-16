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

        string answer = "Test answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\nTest answer!\n";

        chosenCard.GetComponent<WishCard>().CardFrontText.text = answer;
        gameObject.SetActive(false);
        chosenCard.GetComponent<WishCard>().CardFront.SetActive(true);

    }
}
