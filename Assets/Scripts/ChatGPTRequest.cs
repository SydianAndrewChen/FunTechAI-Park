using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
public class ChatGPTRequest
{
    private string url = "";
    public string response;

    string FormulateQuestion(PlayerInfo playerInfo)
    {
        return new string("Write something about this player");
    }
    IEnumerator WishPost( PlayerInfo playerInfo, string questions)
    {
        ChatGPTRequestStruct r = new ChatGPTRequestStruct();
        r.messages[0].content = FormulateQuestion(playerInfo);
        yield return Post(url, JsonUtility.ToJson(r));
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
        response = request.downloadHandler.text;
        JObject obj = JObject.Parse(request.downloadHandler.text);
        response = (string)((JArray)obj["choices"])[0]["message"];
    }
}
