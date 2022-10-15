using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ApplicationPlateform { Android, IOS }
public class GameManager : MonoBehaviour
{
    public bool playerCanPlay;
    [Space]
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private ImageBank imageBank;
    [Space]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas gameWinCanvas;
    [SerializeField] private Image fullImage;

    public static GameManager Game;
    private void Awake()
    {
        if (Game == null)
            Game = this;
    }

    private void Start()
    {
        InitiateGame();
        gameTimer.StartTimer();

        gameOverCanvas.enabled = false;
        gameWinCanvas.enabled = false;
    }

    private void InitiateGame()
    {
        playerCanPlay = true;

        //Find which application platerform is running, default is Android
        ApplicationPlateform currentApplicationPlateform = ApplicationPlateform.Android;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            currentApplicationPlateform = ApplicationPlateform.IOS;

        //Load image pieces from image bank depending on plateform then load it to the grid
        List<ImagePiece> imagePieces = LoadImageForPlateform(currentApplicationPlateform);
        GridManager.Grid.LoadImagePieces(imagePieces);
    }

    //Search the fist image setup for specified plateform (Android or IOS)
    private List<ImagePiece> LoadImageForPlateform(ApplicationPlateform applicationPlateform)
    {
        for (int i = 0; i < imageBank.imagePresets.Count; i++)
        {
            if (imageBank.imagePresets[i].plateformTarget == applicationPlateform)
            {
                fullImage.sprite = imageBank.imagePresets[i].fullImage; //Load full image for win screen
                return imageBank.imagePresets[i].imagePieces;
            }
        }
        return null;
    }

    public void GameOver()
    {
        playerCanPlay = false;
        gameOverCanvas.enabled = true;
    }

    public void GameWin()
    {
        playerCanPlay = false;
        gameWinCanvas.enabled = true;
        float winTimer = gameTimer.StopTimer();

        SaveHighscore(winTimer);
    }

    private void SaveHighscore(float currentTimerScore)
    {
        //Load current highscore is present and compare it to current score
        string jsonString = PlayerPrefs.GetString("highscore");
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        //Save current score if their is no highscore or if the highscore is lower than current score
        if ((highscore != null && highscore.highscoreTimer < currentTimerScore) || highscore == null)
        {
            Highscore newHighscores = new Highscore { highscoreTimer = currentTimerScore };
            string json = JsonUtility.ToJson(newHighscores);
            PlayerPrefs.SetString("highscore", json);
            PlayerPrefs.Save();
        }
    }

    public void ButtonRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ButtonGoHome()
    {
        SceneManager.LoadScene(0);

    }
}

public class Highscore
{
    public float highscoreTimer;
}
