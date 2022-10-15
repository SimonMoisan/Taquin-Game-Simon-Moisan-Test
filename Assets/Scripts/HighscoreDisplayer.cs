using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreDisplayerTextValue;

    // Start is called before the first frame update
    void Start()
    {
        LoadCurrentHighscore();
    }

    private void LoadCurrentHighscore()
    {
        //Load current highscore is present and compare it to current score
        string jsonString = PlayerPrefs.GetString("highscore");
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        //Display highscore round to two dicimals
        if (highscore != null)
            highscoreDisplayerTextValue.text = highscore.highscoreTimer.ToString("F2");
    }
}
