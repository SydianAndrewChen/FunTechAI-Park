using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
public class ChatGPTRequest
{
    private string url = "https://api.openai.com/v1/chat/completions";
    public string response;

    public IEnumerator Post(string bodyJsonString)
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
        JObject obj = JObject.Parse(request.downloadHandler.text);
        response = ((JArray)obj["choices"])[0]["message"]["content"].ToString();
        Debug.Log(response);
    }
}
