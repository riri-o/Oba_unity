using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LrcLyricsDisplay : MonoBehaviour
{
    public string lrcFileName = "Lrc-BirthdaySong.txt"; // LRCファイル
    private List<LyricLineInfo> lyricsList = new List<LyricLineInfo>();

    [System.Serializable]
    public class LyricLineInfo
    {
        public float startTime;
        public string text;
        public List<LyricPartInfo> parts = new List<LyricPartInfo>();
    }

    [System.Serializable]
    public class LyricPartInfo
    {
        public string word;
        public Color color;
    }

    void Start()
    {
        LoadLrcFile();
    }

    public List<LyricLineInfo> GetLyricLines()
    {
        return lyricsList;
    }

    private void LoadLrcFile()
    {
        string path = Path.Combine(Application.dataPath, lrcFileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"LRC file not found: {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"\[\d+:\d+\.\d+\]"))
            {
                string timePart = line.Substring(1, line.IndexOf("]") - 1);
                string[] timeComponents = timePart.Split(':');
                float minutes = float.Parse(timeComponents[0]);
                float seconds = float.Parse(timeComponents[1]);
                float startTime = minutes * 60 + seconds;

                string textPart = line.Substring(line.IndexOf("]") + 1);
                lyricsList.Add(new LyricLineInfo { startTime = startTime, text = textPart });
            }
        }

        Debug.Log($"Loaded {lyricsList.Count} lyrics from {lrcFileName}");
    }
}

//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using TMPro;

//public class LrcLyricsDisplay : MonoBehaviour
//{
//    public TextMeshProUGUI[] textField; // �̎���\������TextMeshProUGUI�I�u�W�F�N�g�i3�s���j
//    public string lrcFileName = "Lrc-BirthdaySong.txt"; // LRC�t�@�C�����iAssets�t�H���_���j
//    private List<LyricLineInfo> lyricsList = new List<LyricLineInfo>(); // �̎������i�[���郊�X�g
//    private int currentLyricIndex = 0; // ���݂̉̎��C���f�b�N�X
//    private float timeInit = 0; // �̂̎n�܂�̎���
//    private Color[] colors = { Color.red, Color.green, Color.yellow }; // �g�p����3�F
//    private string[] colorNames = { "Red", "Green", "Yellow" }; // �F��

//    [System.Serializable]
//    public class LyricPartInfo
//    {
//        public string word; // �P��
//        public Color color; // ���蓖�Ă�ꂽ�F
//    }

//    [System.Serializable]
//    public class LyricLineInfo
//    {
//        public float startTime; // �\�������i�b�P�ʁj
//        public string text; // �̎����e
//        public List<LyricPartInfo> parts = new List<LyricPartInfo>(); // �P�ꂲ�Ƃ̐F���
//    }

//    void Start()
//    {
//        LoadLrcFile(); // LRC�t�@�C����ǂݍ���
//        AssignRandomColors(); // �P�ꂲ�ƂɃ����_���ɐF�����蓖��
//        ExportColorLog(); // �F���������L�^
//        lyricsList.Add(new LyricLineInfo { startTime = timeInit, text = "" });
//        UpdateLyricsDisplay(); // �����\�����X�V
//        Debug.Log("lyrics.Count: " + lyricsList.Count);
//    }

//    /// <summary>
//    /// Update �֐��F�����I�Ɉ��Ԋu�ŌĂяo�����֐�
//    /// ���݂̎����Ɋ�Â��ĉ̎����X�V
//    /// </summary>
//    void Update()
//    {
//        // if (lyricsList.Count: 7) then (currentLyricIndex: 0 - 6)
//        int numTotalLine = lyricsList.Count;

//        // Quit game if finish to display all lyrics 
//        if (currentLyricIndex >= numTotalLine - 1)
//        {
//            ExitApplication();
//        }

//        // currentTime: ���݂̃V�[�������[�h����Ă���̌o�ߎ��Ԃ��擾
//        float currentTime = Time.timeSinceLevelLoad;

//        // ���̉̎��s�ɐi�ނׂ��^�C�~���O���m�F
//        LyricLineInfo nextLine = lyricsList[currentLyricIndex + 1];
//        if (currentLyricIndex < numTotalLine - 2 && currentTime >= nextLine.startTime)
//        {
//            Debug.Log("currentLyricIndex: " + currentLyricIndex + ", " + lyricsList[currentLyricIndex].text);
//            // Update the index of lyrics
//            currentLyricIndex++;
//            // Display the lyrics of NEXT line
//            UpdateLyricsDisplay();
//        }
//    }

//    /// <summary>
//    /// lyricsList �쐬
//    /// </summary>
//    void LoadLrcFile()
//    {
//        string path = Path.Combine(Application.dataPath, lrcFileName);
//        if (!File.Exists(path))
//        {
//            Debug.LogError($"LRC file not found: {path}");
//            return;
//        }

//        string[] lines = File.ReadAllLines(path);
//        // �ŏI�s�����L�^�p�ϐ�
//        float timeEndLine = 0f;

//        foreach (string line in lines)
//        {
//            // LRC�`�����p�[�X���鐳�K�\��
//            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"\[\d+:\d+\.\d+\]"))
//            {
//                // �����������擾
//                string timePart = line.Substring(1, line.IndexOf("]") - 1);
//                string[] timeComponents = timePart.Split(':');
//                float minutes = float.Parse(timeComponents[0]);
//                float seconds = float.Parse(timeComponents[1]);
//                float startTime = timeInit + minutes * 60 + seconds;

//                // �̎��������擾
//                string textPart = line.Substring(line.IndexOf("]") + 1);

//                // ���X�g�ɒǉ�
//                lyricsList.Add(new LyricLineInfo { startTime = startTime, text = textPart });
//                timeEndLine = startTime;
//            }
//        }
//        // �Ō�ɏI���̎w�� EOF �Ă���
//        lyricsList.Add(new LyricLineInfo { startTime = timeEndLine + 4, text = "GAME END." });
//        lyricsList.Add(new LyricLineInfo { startTime = timeEndLine + 4, text = "" });
//        // 
//        Debug.Log("lyricsList: \n");
//        foreach (LyricLineInfo lyricsLine in lyricsList)
//        {
//            Debug.Log(lyricsLine.startTime + ", " + lyricsLine.text + "\n");
//        }

//        Debug.Log($"Loaded {lyricsList.Count} lyrics from {lrcFileName}");
//    }

//    void AssignRandomColors()
//    {
//        foreach (var line in lyricsList)
//        {
//            string[] wordList = line.text.Split(' '); // �P�ꂲ�Ƃɕ���
//            foreach (var word in wordList)
//            {
//                int randomIndex = Random.Range(0, colors.Length); // �����_���ŐF��I��
//                line.parts.Add(new LyricPartInfo { word = word, color = colors[randomIndex] });
//            }
//        }
//    }

//    void ExportColorLog()
//    {
//        string logPath = Path.Combine(Application.dataPath, "LyricsColorLog.txt");
//        using (StreamWriter writer = new StreamWriter(logPath))
//        {
//            writer.WriteLine("Lyrics Color Log:");
//            foreach (var line in lyricsList)
//            {
//                writer.WriteLine($"[{line.startTime:00.00}]");
//                foreach (var part in line.parts)
//                {
//                    string colorName = ColorToName(part.color);
//                    writer.WriteLine($"  \"{part.word}\" - {colorName}");
//                }
//            }
//        }
//        Debug.Log($"Color log saved to {logPath}");
//    }

//    string ColorToName(Color color)
//    {
//        if (color == Color.red) return "RED";
//        if (color == Color.green) return "GREEN";
//        if (color == Color.yellow) return "YELLOW";
//        return "UNKNOWN";
//    }

//    void UpdateLyricsDisplay()
//    {
//        // �^�񒆂̍s���X�V���邽�߂̃C���f�b�N�X
//        int middleLineIndex = 1;

//        for (int i = 0; i < textField.Length; i++)
//        {
//            // �\������̎��s������i�O��1�s + ���ݍs�j
//            int lyricIndex = currentLyricIndex + i - middleLineIndex;

//            if (lyricIndex >= 0 && lyricIndex < lyricsList.Count)
//            {
//                // �e�L�X�g��F�t���ō\�z
//                string coloredText = "";
//                //string coloredText = (currentLyricIndex+1).ToString();
//                foreach (var part in lyricsList[lyricIndex].parts)
//                {
//                    string hexColor = ColorUtility.ToHtmlStringRGB(part.color);
//                    coloredText += $"<color=#{hexColor}>{part.word}</color> ";
//                }

//                textField[i].text = coloredText.Trim();

//                // �^�񒆂̍s�͕s�����A����ȊO�͔�����
//                if (i == middleLineIndex)
//                {
//                    textField[i].color = new Color(1f, 1f, 1f, 1f); // �s����
//                }
//                else
//                {
//                    textField[i].color = new Color(1f, 1f, 1f, 0.2f); // ������
//                }
//            }
//            else
//            {
//                // �̎����Ȃ��ꍇ�͋󔒂ɐݒ�
//                textField[i].text = "";
//            }
//        }
//    }

//    void ExitApplication()
//    {
//#if UNITY_EDITOR
//        // Unity�G�f�B�^�̏ꍇ�A�Đ����[�h���~
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//    // �����^�C���̏ꍇ�A�A�v���P�[�V�������I��
//    Application.Quit();
//#endif
//    }
//}