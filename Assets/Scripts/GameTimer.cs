using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private bool timerRunning;
    public bool timerFinished;
    public TextMeshProUGUI timerText;
    private float currentTime;
    public float baseTime;
    [SerializeField] private string currentTimeStringFormat;

    private void Start()
    {
        currentTime = baseTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;

                //Parse time format
                currentTimeStringFormat = ParseTime(currentTime);
                timerText.text = currentTimeStringFormat;
            }
            else
            {
                timerFinished = true;
                StopTimer();

                currentTime = 0;
                currentTimeStringFormat = ParseTime(currentTime);
                timerText.text = currentTimeStringFormat;

                GameManager.Game.GameOver();
            }
        }
    }

    //Parse time to display it as mm:ss:fff
    private string ParseTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public float StopTimer()
    {
        timerRunning = false;
        return currentTime;
    }
}
