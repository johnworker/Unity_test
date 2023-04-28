using System.Collections.Generic;
using UnityEngine;

public class Localizer : MonoBehaviour
{
    public static Localizer instance;

    public enum Languages
    {
        Czech,
        English
    }

    public Languages language;

    public TextAsset CzechFile;
    public TextAsset EnglishFile;

    private Dictionary<string, string> czech;
    private Dictionary<string, string> english;

    private void Awake()
    {
        instance = this;

        LoadTexts(CzechFile, out czech);
        LoadTexts(EnglishFile, out english);

        enabled = false;
    }

    private void LoadTexts(TextAsset file, out Dictionary<string, string> dictionary)
    {
        dictionary = new Dictionary<string, string>();
        string all = file.text;
        string[] fLines = all.Split(System.Environment.NewLine.ToCharArray());

        foreach(string line in fLines)
        {
            if (line != "" && int.TryParse(line[0].ToString(), out _))
            {
                string val = line.Substring(7).Replace("\\n", "\n");
                dictionary[line.Substring(0, 6)] = val;
            }
        }
    }

    public string GetText(string code)
    {
        try
        {
            if (language == Languages.Czech)
            {
                return czech[code];
            }
            else if (language == Languages.English)
            {
                return english[code];
            }
        }
        catch
        {
            return "";
        }
       
        return "";
    }
}
