using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class MessageStruct
{
    public string role = "user";
    public string content = "Who are you";
}

[Serializable]
public class ChatGPTRequestStruct
{
    public string model = "gpt-3.5-turbo-0613";
    public MessageStruct[] messages = new MessageStruct[1];
    public float temperature = 0.5f;
}
