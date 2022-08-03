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
    [SerializeField] private Animator timerAnimator;
    [Range(0, 1)][SerializeField] private float rushTimeThreshold;
    [SerializeField] private Text challenge;
    [SerializeField] private CanvasGroup challengeCanvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text perfectCount;
    [SerializeField] private GameObject perfectGroup;

    [Header("Pause")]
    [SerializeField] private GameObject pauseGroup;
    // [Header("Game Over")]
    [Header("End Game")]
    [SerializeField] private GameObject endgameGroup;
    [SerializeField] private Text endGameScore;
    [SerializeField] private Text bestScore;

    private bool isPlaying;

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;

        GameManager.Instance.OnScore += UpdateScore;

        GameManager.Instance.OnPause += delegate ()
        {
            isPlaying = false;
            timerAnimator.enabled = false;
            pauseGroup.SetActive(true);
        };

        GameManager.Instance.OnResume += delegate ()
        {
            isPlaying = true;
            timerAnimator.enabled = true;
            pauseGroup.SetActive(false);
        };

        GameManager.Instance.OnTimeOut += delegate () { isPlaying = false; };

        GameManager.Instance.OnRevive += delegate ()
        {
            isPlaying = true;
        };

        GameManager.Instance.OnEndGame += ShowEndgameGroup;
    }

    public void OnGameStart()
    {
        isPlaying = true;
        challenge.text = GameManager.Instance.GetCurrentLevel.description;
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
        AudioManager.Instance.PlayMenuAudio();
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

    public void UseSecondChance()
    {
        GameManager.Instance.OnRevive();
    }

    private void ShowEndgameGroup()
    {
        isPlaying = false;
        endgameGroup.SetActive(true);

        int score = GameManager.Instance.Score;
        int theBestScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score >= theBestScore)
        {
            endGameScore.color = bestScore.color;
        }
        endGameScore.text = score.ToString();
        bestScore.text = theBestScore.ToString();
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
            timerBar.fillAmount = Mathf.Clamp(timerBarFill, 0.0f, 1.0f);

            timerAnimator.SetBool("Warning", timerBarFill <= rushTimeThreshold);
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
