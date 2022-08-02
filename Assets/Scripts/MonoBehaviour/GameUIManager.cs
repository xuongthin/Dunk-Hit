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
    private Material timerBarMaterial;
    [SerializeField] private Text challenge;
    [SerializeField] private CanvasGroup challengeCanvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text perfectCount;
    [SerializeField] private GameObject perfectGroup;

    [Header("Pause")]
    [SerializeField] private GameObject pauseGroup;
    // [Header("Game Over")]
    // [SerializeField] private GameObject gameOverGroup;
    [Header("End Game")]
    [SerializeField] private GameObject endgameGroup;
    [SerializeField] private Text endGameScore;
    [SerializeField] private Text bestScore;

    private bool isPlaying;

    private void Start()
    {
        timerBarMaterial = timerBar.material;

        GameManager.Instance.OnGameStart += OnGameStart;

        GameManager.Instance.OnScore += UpdateScore;

        GameManager.Instance.OnPause += delegate () { pauseGroup.SetActive(true); };

        GameManager.Instance.OnResume += delegate () { pauseGroup.SetActive(false); };

        GameManager.Instance.OnRevive += delegate ()
        {
            // gameOverGroup.SetActive(false);
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

    public void ShowEndgameGroup()
    {
        // gameOverGroup.SetActive(false);
        endgameGroup.SetActive(true);
        endGameScore.text = GameManager.Instance.Score.ToString();
        bestScore.text = PlayerPrefs.GetInt("High Score", 0).ToString();
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
            timerBarMaterial.SetFloat("_Fill", timerBar.fillAmount);
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
