using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;

/// <summary>
/// CSV����
/// </summary>
public static class CSVUtils
{
    const string CSVDir = "Config/";

    /// <summary>
    /// ����CSV
    /// </summary>
    public static List<List<string>> ParseCSV(string path, int beginParseRow)
    {
        List<List<string>> dataList = new List<List<string>>();

        var ta = Resources.Load<TextAsset>(CSVDir + path);
        if (ta == null)
        {
            Debug.LogError("CSV�ļ������ڣ�" + CSVDir + path);
            return dataList;
        }
        string[] rowCollection = ta.text.Split('\n');
        for (int row = beginParseRow; row < rowCollection.Length; row++)
        {
            if (string.IsNullOrEmpty(rowCollection[row])) continue;
            rowCollection[row] = rowCollection[row].Replace("\r", "");
            string[] colCollection = rowCollection[row].Split('_');

            List<string> tempList = new List<string>();
            for (int col = 0; col < colCollection.Length; col++)
            {
                tempList.Add(colCollection[col]);
            }
            dataList.Add(tempList);
        }
        return dataList;
    }

    /// <summary>
    /// �õ���������
    /// </summary>
    public static List<T> GetDataArray<T>(string dataStr, char separator)
    {
        List<T> dataList = new List<T>();
        string[] dataArray = dataStr.Split(separator);
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (string.IsNullOrEmpty(dataArray[i])) continue;
            dataArray[i] = dataArray[i].Replace("\r", "");
            try
            {
                T data = (T)Convert.ChangeType(dataArray[i], typeof(T));
                dataList.Add(data);
            }
            catch
            {
                Debug.LogError($"string����ת{typeof(T)}ʧ�ܣ�{dataArray[i]}");
            }
        }
        return dataList;
    }
}

public enum CharacterStatus
{
    wait=0,
    dialogue=1,
    action=2,
    move=3
}

public class CharacterFrame
{
    public string text;
    public CharacterStatus characterStatus;
    public string place;
    public string target;
    
}

public class CharacterTimelineController : MonoBehaviour
{
    private int time;
    private CharacterFrame[] frames = new CharacterFrame[15];
    public string characterName;
    public TMP_Text dialogueText;

    private const float frameTime = 5.0f;
    public LocationManager locationManager;
    private float timer = frameTime + 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        ParseTimelineCSV(string.Format("Characters/{0}", characterName));
    }

    // Update is called once per frame
    void Update()
    {
        // TODO
        /*Judge whether 10 seconds passed*/
        /*        while (true)
                {
                }*/
        timer += Time.deltaTime;
        if (timer >frameTime)
        {
            StartCoroutine(UpdateOneVirtualFrame());
            timer = 0.0f;
        }
    }

    void ParseTimelineCSV(string filename)
    {
        var parseResult = CSVUtils.ParseCSV(filename, 1);
        for (int index = 0; index < 15; index++)
        {
            CharacterFrame frame = new CharacterFrame();
            frame.place = parseResult[index][2].ToLower();
            switch (parseResult[index][3])
            {
                case "wait dialogue":
                    frame.characterStatus = CharacterStatus.wait;
                    break;
                case "action":
                    frame.characterStatus = CharacterStatus.action;
                    break;
                case "dialogue":
                    frame.characterStatus = CharacterStatus.dialogue;
                    break;
                case "move":
                    frame.characterStatus = CharacterStatus.move;
                    break;
            }
            frame.text = parseResult[index][4];
            frame.target = parseResult[index][5].ToLower();
            frames[index] = frame;
        }
    }

    IEnumerator UpdateOneVirtualFrame()
    {
        var frame = frames[time];
        dialogueText.SetText(frames[time].text);

        switch (frame.characterStatus)
        {
            case CharacterStatus.move:
                Debug.Log(frame.target);
                CharacterMove(frame.target);
                break;
            default:
                break;
        }
        yield return null;
        time++;
    }


    void CharacterMove(string placeName)
    {
        var position = locationManager.GetLocation(placeName).position;
        if (position == null)
        {
            Debug.LogError(placeName);
            return;
        }
        // TODO
        GetComponent<Transform>().DOMove(position, frameTime * 0.9f);
    }
}
 