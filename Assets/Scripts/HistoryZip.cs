using System.Collections;
using Carrot;
using UnityEngine;

public class HistoryZip : MonoBehaviour
{
    int length = 0;

    public void OnLoad()
    {
        length = PlayerPrefs.GetInt("lengthHistory", 0);
    }

    public void Add(IDictionary data)
    {
        string sData = Json.Serialize(data);
        PlayerPrefs.SetString("h_" + length, sData);
        length++;
        PlayerPrefs.SetInt("lengthHistory", length);
    }
}
