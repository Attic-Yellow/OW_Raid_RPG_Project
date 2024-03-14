using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVTranslator : MonoBehaviour
{
    public enum Language
    {
        English,
        Korean
    }

    [MenuItem("Assets/Build CSVTranslator")]
    static void ConvertToJsonCoroutine()
    {
        foreach (Language lang in System.Enum.GetValues(typeof(Language)))
        {
            ConvertLanguageToJSON(lang);
        }
    }

    static void ConvertLanguageToJSON(Language lang)
    {
        var csvFileName = $"{lang}.csv";
        var jsonFileName = $"{lang}.json";

        string csvFilePath = Path.Combine("Assets/UIText", csvFileName);
        string jsonFilePath = Path.Combine("Assets/UIText", jsonFileName);

        Dictionary<string, string> uiTexts = new Dictionary<string, string>();

        string[] csvLines = File.ReadAllLines(csvFilePath, Encoding.GetEncoding("euc-kr"));

        foreach (string line in csvLines)
        {
            string[] splitLine = line.Split(',');
            if (splitLine.Length < 2) continue; // 유효하지 않은 행은 건너뜁니다.

            string key = splitLine[0].Trim();
            string value = splitLine[1].Trim();

            // UI 텍스트를 딕셔너리에 추가
            uiTexts[key] = value;
        }

        var combinedData = new
        {
            uiTexts
        };

        string json = JsonConvert.SerializeObject(combinedData);
        File.WriteAllText(jsonFilePath, json);
    }
}
