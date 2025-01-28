using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LylicsGame : MonoBehaviour
{
    public Text textDisplay; // 歌詞を表示するTextオブジェクト
    public List<Image> characterImages; // キャラクター画像リスト
    private Outline currentOutline; // 現在選ばれたキャラクターの枠
    private System.Random random = new System.Random();

    private string[] lylicsTexts; // 動的に変更される歌詞の配列
    private int currentLyricIndex = 0;

    // LrcLyricsDisplayを参照
    public LrcLyricsDisplay lrcLyricsDisplay;

    void Start()
    {
        // LRCデータを取得して歌詞を初期化
        if (lrcLyricsDisplay != null)
        {
            InitializeLyricsFromLRC();
        }

        // 歌詞が正しく読み込まれているか確認
        if (lylicsTexts.Length > 0)
        {
            textDisplay.text = lylicsTexts[currentLyricIndex];
        }
        else
        {
            Debug.LogError("Lyrics array is empty.");
        }

        // キャラクター画像の初期アウトラインを非表示にする
        foreach (var image in characterImages)
        {
            var outline = image.gameObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = image.gameObject.AddComponent<Outline>();
            }
            outline.effectColor = Color.clear; // 初期状態は枠なし
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックで次へ進む
        {
            AdvanceLyric();
        }
    }

    void AdvanceLyric()
    {
        currentLyricIndex++;

        if (currentLyricIndex < lylicsTexts.Length)
        {
            textDisplay.text = lylicsTexts[currentLyricIndex];
            DecideChallenger(); // Challengerを選ぶ
        }
        else
        {
            textDisplay.text = ""; // 表示クリア
            Debug.Log("Story finished!");
        }
    }

    private void DecideChallenger()
    {
        if (characterImages.Count == 0)
        {
            Debug.LogError("Character images are not assigned.");
            return;
        }

        int challengerIndex = random.Next(characterImages.Count);

        if (currentOutline != null)
        {
            currentOutline.effectColor = Color.clear;
        }

        var selectedImage = characterImages[challengerIndex];
        currentOutline = selectedImage.gameObject.GetComponent<Outline>();
        if (currentOutline == null)
        {
            currentOutline = selectedImage.gameObject.AddComponent<Outline>();
        }
        currentOutline.effectColor = Color.red;
        currentOutline.effectDistance = new Vector2(5f, 5f);
    }

    private void InitializeLyricsFromLRC()
    {
        // LrcLyricsDisplayから歌詞を取得して配列に変換
        var lyricLines = lrcLyricsDisplay.GetLyricLines();
        lylicsTexts = new string[lyricLines.Count];
        for (int i = 0; i < lyricLines.Count; i++)
        {
            lylicsTexts[i] = lyricLines[i].text;
        }

        Debug.Log($"Lyrics initialized. Total lines: {lylicsTexts.Length}");
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class LylicsGame : MonoBehaviour
//{
//    // Public Text object to display lyrics
//    public Text textDisplay;
//    //public Image challengerImage; // Challengerを表示するImageオブジェクト
//    public List<Image> characterImages; // キャラクター画像リスト
//    private Outline currentOutline; // 現在選ばれたキャラクターの枠
//    private string[] characters = { "Character1", "Character2", "Character3", "Character4" };
//    private System.Random random = new System.Random();

//    // Private array of lyrics
//    private string[] lylicsTexts = {
//        "Happy birthday to you",
//        "Happy birthday to you",
//        "Happy birthday dear Lily",
//        "Happy birthday to you"
//    };

//    // Index to track the current lyric
//    private int currentLyricIndex = 0;

//    void Start()
//    {
//        // 初期化
//        if (lylicsTexts.Length > 0)
//        {
//            textDisplay.text = lylicsTexts[currentLyricIndex];
//        }
//        else
//        {
//            Debug.LogError("Lyrics array is empty.");
//        }

//        // キャラクター画像の初期アウトラインを非表示にする
//        foreach (var image in characterImages)
//        {
//            var outline = image.gameObject.GetComponent<Outline>();
//            if (outline == null)
//            {
//                outline = image.gameObject.AddComponent<Outline>();
//            }
//            outline.effectColor = Color.clear; // 初期状態は枠なし
//        }

//    }

//    void Update()
//    {
//        // Check for mouse click
//        if (Input.GetMouseButtonDown(0)) // Left mouse button
//        {
//            AdvanceLyric();
//        }
//    }

//    // Method to advance to the next lyric
//    void AdvanceLyric()
//    {
//        currentLyricIndex++;

//        if (currentLyricIndex < lylicsTexts.Length)
//        {
//            textDisplay.text = lylicsTexts[currentLyricIndex];
//            DecideChallenger(); // Challengerを選ぶ
//        }
//        else
//        {
//            textDisplay.text = ""; // Clear the text display
//            Debug.Log("Story finished!");
//        }
//    }


//    private void DecideChallenger()
//    {
//        // 要素数チェック
//        if (characters.Length != characterImages.Count)
//        {
//            Debug.LogError($"Size mismatch: characters.Length ({characters.Length}) and characterImages.Count ({characterImages.Count}) must be the same.");
//            return; // 処理を中断
//        }
//        // Challengerをランダムに決定
//        int challengerIndex = random.Next(characters.Length);
//        string opponentHandType = characters[challengerIndex];

//        //// Challengerの画像を設定
//        //challengerImage.sprite = Resources.Load<Sprite>(opponentHandType);

//        // 前回の枠をクリア
//        if (currentOutline != null)
//        {
//            currentOutline.effectColor = Color.clear;
//        }

//        // 選ばれたキャラクターの枠を赤く設定
//        var selectedImage = characterImages[challengerIndex];
//        currentOutline = selectedImage.gameObject.GetComponent<Outline>();
//        if (currentOutline == null)
//        {
//            currentOutline = selectedImage.gameObject.AddComponent<Outline>();
//        }
//        currentOutline.effectColor = Color.red;
//        currentOutline.effectDistance = new Vector2(5f, 5f);
//    }
//}
