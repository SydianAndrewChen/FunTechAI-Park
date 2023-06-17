using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class TestNetRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PostTest()
    {
        var r = new ChatGPTRequestStruct();
        string jsonString = JsonUtility.ToJson(r);
        Debug.Log(jsonString);
        string url = "https://api.openai.com/v1/chat/completions";
        StartCoroutine(Post(url, jsonString));
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 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");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
