using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            if (sceneName != null) SceneManager.LoadSceneAsync(sceneName);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
