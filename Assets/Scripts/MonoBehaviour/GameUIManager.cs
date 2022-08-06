using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Animator scoreAnimator;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text additionalScore;
    [SerializeField] private Text perfectCount;
    [SerializeField] private GameObject perfectGroup;

    [Header("Pause")]
    [SerializeField] private GameObject pauseGroup;
    [Header("Challenge Complete")]
    [SerializeField] private Animator challengeCompleteAnimator;
    [Header("Challenge Fail")]
    [SerializeField] private Animator challengeFailAnimator;
    [Header("End Game")]
    [SerializeField] private GameObject endgameGroup;
    [SerializeField] private Text endGameScore;
    [SerializeField] private Text bestScore;

    private bool isPlaying;
    private int previousScore;

    private const string ACTIVE = "Active";
    private const string SCORE = "Score";
    private const string MAIN_MENU = "MainMenu";
    private const string GAME = "Game";
    private const string WARNING = "Warning";

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

        GameManager.Instance.OnChallengeComplete += delegate ()
        {
            challengeCompleteAnimator.Play(ACTIVE);
        };

        GameManager.Instance.OnChallengeFail += delegate ()
        {
            challengeFailAnimator.Play(ACTIVE);
        };

        challenge.text = GameManager.Instance.GetChallengeText();
        previousScore = 0;
    }

    public void UpdateScore(bool combo)
    {
        int newScore = GameManager.Instance.Score;
        scoreText.text = newScore.ToString();
        additionalScore.text = "+" + (newScore - previousScore).ToString();
        previousScore = newScore;
        if (combo)
        {
            int comboCount = GameManager.Instance.ComboCount;
            perfectCount.text = "x" + comboCount.ToString();
            perfectGroup.SetActive(true);
        }
        else
        {
            perfectGroup.SetActive(false);
        }
        scoreAnimator.Play(SCORE);
    }

    public void BackHome()
    {
        AudioManager.Instance.PlayMenuAudio();
        SceneManager.LoadScene(MAIN_MENU);
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

    public void Replay()
    {
        SceneManager.LoadScene(GAME);
    }

    public void Update()
    {
        if (isPlaying)
        {
            float timerBarFill = GameManager.Instance.TimeRemainInPercent;
            timerBar.fillAmount = Mathf.Clamp(timerBarFill, 0.0f, 1.0f);

            timerAnimator.SetBool(WARNING, timerBarFill <= rushTimeThreshold);
        }
    }

    private void ShowEndgameGroup()
    {
        isPlaying = false;
        endgameGroup.SetActive(true);

        int score = GameManager.Instance.Score;
        int theBestScore = Tracker.Instance.GetData(((int)TrackedDataType.BestScore));
        if (score >= theBestScore)
        {
            endGameScore.color = bestScore.color;
        }
        endGameScore.text = score.ToString();
        bestScore.text = theBestScore.ToString();
    }

    private void OnGameStart()
    {
        isPlaying = true;
        StartCoroutine(Fade(challengeCanvas, 2, 1.5f));
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
