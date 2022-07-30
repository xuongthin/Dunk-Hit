using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: time out warning

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("In Game")]
    [SerializeField] private Image timerBar;
    [SerializeField] private Text challenge;
    [SerializeField] private CanvasGroup challengeCanvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text perfectCount;
    [SerializeField] private GameObject perfectGroup;

    [Header("Pause")]
    [SerializeField] private GameObject pauseGroup;
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverGroup;
    [SerializeField] private Image secondChanceTimerBar;
    [SerializeField] private float timeForSecondChance;
    private float secondChanceTimer;
    [Header("End Game")]
    [SerializeField] private GameObject endgameGroup;
    [SerializeField] private Text endGameScore;
    [SerializeField] private Text bestScore;

    private bool isPlaying;
    private bool isGameOver;

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;

        GameManager.Instance.OnScore += UpdateScore;

        GameManager.Instance.OnPause += delegate () { pauseGroup.SetActive(true); };

        GameManager.Instance.OnResume += delegate () { pauseGroup.SetActive(false); };

        GameManager.Instance.OnTimeOut += ShowGameOverGroup;
        GameManager.Instance.OnTimeOut += StartSecondChanceTimer;

        GameManager.Instance.OnRevive += delegate ()
        {
            gameOverGroup.SetActive(false);
            isPlaying = true;
            isGameOver = false;
        };

        GameManager.Instance.OnEndGame += ShowEndgameGroup;
    }

    public void OnGameStart()
    {
        isPlaying = true;
        isGameOver = false;
        challenge.text = GameManager.Instance.GetCurrentLevel.challenge;
        StartCoroutine(Fade(challengeCanvas, 2, 0.75f));
    }

    public void UpdateScore(bool combo)
    {
        scoreText.text = GameManager.Instance.Score.ToString();
        if (combo)
        {
            perfectGroup.SetActive(true);
            perfectCount.text = "x" + GameManager.Instance.ComboCount.ToString();
        }
        else
        {
            perfectGroup.SetActive(false);
        }
    }

    public void BackHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        GameManager.Instance.OnPause();
    }

    public void ResumeGame()
    {
        GameManager.Instance.OnResume();
    }

    public void ShowGameOverGroup()
    {
        gameOverGroup.SetActive(true);
    }

    private void StartSecondChanceTimer()
    {
        isPlaying = false;
        isGameOver = true;
        secondChanceTimer = timeForSecondChance;
    }

    public void UseSecondChance()
    {
        GameManager.Instance.OnRevive();
    }

    public void ShowEndgameGroup()
    {
        gameOverGroup.SetActive(false);
        endgameGroup.SetActive(true);
        endGameScore.text = GameManager.Instance.Score.ToString();
        // TODO: update highest score text
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }

    public void Update()
    {
        if (isPlaying)
        {
            float timerBarFill = GameManager.Instance.TimeRemainInPercent;
            timerBar.fillAmount = timerBarFill;
        }
        else if (isGameOver)
        {
            secondChanceTimer -= Time.deltaTime;
            secondChanceTimerBar.fillAmount = secondChanceTimer / timeForSecondChance;
            if (secondChanceTimer <= 0)
            {
                ShowEndgameGroup();
            }
        }
    }

    private IEnumerator Fade(CanvasGroup canvas, float delay, float time)
    {
        yield return Yielders.Get(delay);
        float lerp = time;
        while (lerp >= 0)
        {
            lerp -= Time.deltaTime;
            canvas.alpha = lerp / time;
            yield return null;
        }
    }
}
