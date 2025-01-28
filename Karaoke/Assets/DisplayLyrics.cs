using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Xml;
[System.Serializable]
public class ColorLog
{
    public List<ColorLogLine> ColorLogLines = new List<ColorLogLine>();
}
[System.Serializable]
public class ColorLogLine
{
    public int LineIndex;
    public List<ColorLogPart> Parts = new List<ColorLogPart>();
}
[System.Serializable]
public class ColorLogPart
{
    public string Text;
    public string Color;
}
public class DisplayLyrics : MonoBehaviour
{
    public TextMeshProUGUI textComponent;   // 表示用のTextMeshProUGUI
    public string inputFileName = "Orders.xml"; // 読み込む歌詞ファイル
    public string colorLogFileName = "Assets/ColorLog.json"; // 色情報のログファイル
    private ColorLog colorLog;             // JSONデータを保持
    private int currentLineIndex = 0;      // 現在表示中の先頭行
    private Color[] colors = { Color.red, Color.green, Color.blue }; // 使用する3色
    private string[] colorNames = { "RED", "GREEN", "BLUE" }; // 色名対応
    void Start()
    {
        // Step 1: ランダム色分けを記録
        GenerateColorLog();
        // Step 2: 色情報をロード
        LoadColorLog();
        // Step 3: スクロール表示を開始
        StartCoroutine(ScrollLyricsCoroutine());
    }
    void GenerateColorLog()
    {
        string path = Path.Combine(Application.dataPath, inputFileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return;
        }
        colorLog = new ColorLog();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        XmlNodeList lines = xmlDoc.SelectNodes("/Lyrics/Line");
        for (int i = 0; i < lines.Count; i++)
        {
            ColorLogLine logLine = new ColorLogLine { LineIndex = i + 1 };
            string lineText = lines[i].InnerText;
            string[] parts = lineText.Split(',');
            int lastColorIndex = 0;
            foreach (string part in parts)
            {
                int colorIndex = Random.Range(0, colors.Length);
                while (lastColorIndex == colorIndex)
                {
                    colorIndex = Random.Range(0, colors.Length);
                }
                ColorLogPart logPart = new ColorLogPart
                {
                    Text = part.Trim(),
                    Color = colorNames[colorIndex]
                };
                logLine.Parts.Add(logPart);
                // 今の色を記憶（次のパート連続同じ色割り当てないよう）
                lastColorIndex = colorIndex;
            }
            colorLog.ColorLogLines.Add(logLine);
        }
        // 色情報をJSONとして保存
        string json = JsonUtility.ToJson(colorLog, true);
        File.WriteAllText(colorLogFileName, json);
        Debug.Log($"Color log saved to {colorLogFileName}");
    }
    void LoadColorLog()
    {
        if (File.Exists(colorLogFileName))
        {
            string json = File.ReadAllText(colorLogFileName);
            colorLog = JsonUtility.FromJson<ColorLog>(json);
            Debug.Log("Color log loaded.");
        }
        else
        {
            Debug.LogError($"Color log file not found: {colorLogFileName}");
        }
    }
    IEnumerator ScrollLyricsCoroutine()
    {
        while (currentLineIndex < colorLog.ColorLogLines.Count)
        {
            // 現在の行と次の行を取得
            ColorLogLine line1 = colorLog.ColorLogLines[currentLineIndex];
            ColorLogLine line2 = currentLineIndex + 1 < colorLog.ColorLogLines.Count
                ? colorLog.ColorLogLines[currentLineIndex + 1]
                : null;
            // テキストを構築
            int rowNum = currentLineIndex + 1;
            string displayedText = rowNum.ToString() + BuildTextWithColors(line1);
            if (line2 != null)
            {
                displayedText += "\n" + (rowNum + 1).ToString() + BuildTextWithColors(line2);
            }
            // 表示テキストを更新
            textComponent.text = displayedText;
            // 3.5秒ごとに表示歌詞更新
            currentLineIndex++;
            yield return new WaitForSeconds(3.5f);
        }
    }
    string BuildTextWithColors(ColorLogLine line)
    {
        string text = "";
        // ColorLog ファイルから取得したこの１行の中でパート分けごとに色設定反映
        foreach (ColorLogPart part in line.Parts)
        {
            // ColorLog から色名を取得し、Color 型に変換
            Color color = ParseColor(part.Color);
            // Color 型を 16 進カラーコードに変換して <color> タグを挿入
            string colorCode = ColorToCode(color);
            text += $"<color={colorCode}>{part.Text}</color> ";
        }
        return text.Trim();
    }
    Color ParseColor(string colorName)
    {
        switch (colorName)
        {
            case "RED": return Color.red;
            case "GREEN": return Color.green;
            case "BLUE": return Color.blue;
            default: return Color.white;
        }
    }
    // ColorToHex : UnityのColor 型を HTML 形式の 16 進カラーコードに変換
    string ColorToCode(Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }
}