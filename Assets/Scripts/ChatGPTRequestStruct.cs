using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ChatGPTMessageStruct
{
    public string role = "user";
    public string content = "Who are you";
    public ChatGPTMessageStruct() { }
    public ChatGPTMessageStruct(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}

[Serializable]
public class ChatGPTRequestStruct
{
    public string model = "gpt-3.5-turbo-0613";
    public ChatGPTMessageStruct[] messages = new ChatGPTMessageStruct[1] 
    { 
        new ChatGPTMessageStruct(),
    };
    public float temperature = 0.5f;
}
