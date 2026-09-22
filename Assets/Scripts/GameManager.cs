using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI highestLevelText;
    public GameObject gameOverPanel;
    public ButtonController buttonController;

    // ============================================================
    // LEVEL TAMAMLAMA PANELİ
    // ============================================================

    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelCompleteNumberText;

    public float levelCompleteDuration = 1.2f;

    // ============================================================
    // ANA MENÜ
    // ============================================================

    public GameObject mainMenuPanel;

    // ============================================================
    // MAYIN SİSTEMİ
    // ============================================================

    public MineManager mineManager;
    public InterstitialAdManager interstitialAdManager;

    // ============================================================
    // CAN SİSTEMİ
    // ============================================================

    public Image lifeHeart;

    private int lives = 2;

    // ============================================================
    // ARKA PLAN
    // ============================================================

    public Image background;
    public Sprite[] backgroundImages;

    public float levelTime = 20f;

    private float currentTime;

    // ============================================================
    // EN YÜKSEK SEVİYE
    // ============================================================

    private int highestLevel = 1;

    // ============================================================
    // PAUSE SİSTEMİ
    // ============================================================

    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;

    private bool isPaused = false;

    // ============================================================
    // BAŞLANGIÇ
    // ============================================================

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        lives = 2;
        currentTime = levelTime;

        highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        if (highestLevel < 1)
        {
            highestLevel = 1;
        }

        if (buttonController != null)
        {
            buttonController.canPlay = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Oyun başlamadan Pause Button gizli
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);
            mineManager.StopMines();
            mineManager.gameObject.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        UpdateLevelText();
        UpdateHighestLevelText();
        UpdateTimerText();
        UpdateBackground();
        UpdateLifeDisplay();
    }

    // ============================================================
    // OYUNA BAŞLA
    // ============================================================

    public void StartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Oyun başlayınca Pause Button görünür
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);
            mineManager.RestartMines();
        }

        if (buttonController != null)
        {
            buttonController.canPlay = true;
            buttonController.ResetClicks();
        }

        currentTime = levelTime;

        UpdateTimerText();
        UpdateLifeDisplay();
    }

    // ============================================================
    // OYUN
    // ============================================================

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (buttonController != null && !buttonController.canPlay)
        {
            return;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                currentTime = 0;
            }

            UpdateTimerText();
        }

        if (currentTime <= 0)
        {
            currentTime = levelTime;

            LoseLife();
        }
    }

    // ============================================================
    // PAUSE
    // ============================================================

    public void PauseGame()
    {
        if (buttonController != null && !buttonController.canPlay)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        isPaused = true;

        if (buttonController != null)
        {
            buttonController.canPlay = false;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    // ============================================================
    // PAUSE DEVAM ET
    // ============================================================

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }

        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        if (buttonController != null)
        {
            buttonController.canPlay = true;
        }
    }

    // ============================================================
    // TIMER
    // ============================================================

    void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(currentTime);

        timerText.text = "00:" + seconds.ToString("00");
    }

    // ============================================================
    // LEVEL YAZISI
    // ============================================================

    void UpdateLevelText()
    {
        if (levelText == null)
            return;

        levelText.text = "LEVEL " + currentLevel;
    }

    // ============================================================
    // EN YÜKSEK SEVİYE YAZISI
    // ============================================================

    void UpdateHighestLevelText()
    {
        if (highestLevelText == null)
            return;

        highestLevelText.text = highestLevel.ToString();
    }

    // ============================================================
    // CAN KAYBETME
    // ============================================================

    public void LoseLife()
    {
        if (buttonController != null && !buttonController.canPlay)
        {
            return;
        }

        lives--;

        UpdateLifeDisplay();

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        currentTime = levelTime;

        UpdateTimerText();

        if (buttonController != null)
        {
            buttonController.canPlay = true;
        }
    }

    // ============================================================
    // KALP GÖRÜNÜMÜ
    // ============================================================

    void UpdateLifeDisplay()
    {
        if (lifeHeart == null)
            return;

        if (lives <= 1)
        {
            lifeHeart.gameObject.SetActive(false);
        }
        else
        {
            lifeHeart.gameObject.SetActive(true);
        }
    }

    // ============================================================
    // GAME OVER
    // ============================================================

    void GameOver()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Game Over'da Pause Button gizli
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (mineManager != null)
        {
            mineManager.StopMines();
        }

        if (buttonController != null)
        {
            buttonController.canPlay = false;
        }
    }

    // ============================================================
    // LEVEL ATLAMA
    // ============================================================

    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel > highestLevel)
        {
            highestLevel = currentLevel;

            PlayerPrefs.SetInt("HighestLevel", highestLevel);
            PlayerPrefs.Save();

            UpdateHighestLevelText();
        }

        levelTime = 20f - (currentLevel - 1);

        if (levelTime < 10f)
        {
            levelTime = 10f;
        }

        currentTime = levelTime;

        UpdateLevelText();
        UpdateTimerText();

        UpdateBackground();

        if (mineManager != null)
        {
            mineManager.StopMines();
        }

        ShowLevelComplete();
    }

    // ============================================================
    // LEVEL TAMAMLAMA EKRANI
    // ============================================================

    void ShowLevelComplete()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (buttonController != null)
        {
            buttonController.canPlay = false;
        }

        if (levelCompleteNumberText != null)
        {
            levelCompleteNumberText.text = currentLevel.ToString();
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            StopCoroutine(nameof(HideLevelComplete));
            StartCoroutine(HideLevelComplete());
        }
        else
        {
            StartNextLevel();
        }
    }

    // ============================================================
    // LEVEL TAMAMLAMA EKRANINI KAPAT
    // ============================================================

    IEnumerator HideLevelComplete()
    {
        yield return new WaitForSeconds(levelCompleteDuration);

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        // HER 5 LEVELDE BİR INTERSTITIAL REKLAM
        if (currentLevel % 5 == 0)
        {
            if (interstitialAdManager != null)
            {
                interstitialAdManager.ShowInterstitialAd(StartNextLevel);
            }
            else
            {
                StartNextLevel();
            }
        }
        else
        {
            StartNextLevel();
        }
    }

    // ============================================================
    // YENİ LEVELİ BAŞLAT
    // ============================================================

    void StartNextLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Yeni level başladığında Pause Button görünür
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);
            mineManager.RefreshMines(currentLevel);
        }

        if (buttonController != null)
        {
            buttonController.canPlay = true;
            buttonController.ResetClicks();
        }

        currentTime = levelTime;

        UpdateTimerText();
    }

    // ============================================================
    // ARKA PLAN
    // ============================================================

    void UpdateBackground()
    {
        if (background == null)
            return;

        if (backgroundImages == null ||
            backgroundImages.Length == 0)
            return;

        int backgroundIndex =
            (currentLevel - 1) / 5;

        backgroundIndex =
            Mathf.Clamp(
                backgroundIndex,
                0,
                backgroundImages.Length - 1
            );

        background.sprite =
            backgroundImages[backgroundIndex];
    }

    // ============================================================
    // RESTART
    // ============================================================

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        StopAllCoroutines();

        currentLevel = 1;

        levelTime = 20f;

        currentTime = levelTime;

        lives = 2;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Tekrar oyuna girildiği için Pause Button görünür
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        UpdateLifeDisplay();

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);
            mineManager.RestartMines();
        }

        if (buttonController != null)
        {
            buttonController.canPlay = true;
            buttonController.ResetClicks();
        }

        UpdateLevelText();
        UpdateTimerText();
        UpdateBackground();
    }

    // ============================================================
    // ANA MENÜYE DÖN
    // ============================================================

    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        StopAllCoroutines();

        currentLevel = 1;

        levelTime = 20f;

        currentTime = levelTime;

        lives = 2;

        if (buttonController != null)
        {
            buttonController.canPlay = false;
            buttonController.ResetClicks();
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Ana menüde Pause Button gizli
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);

            mineManager.StopMines();

            mineManager.gameObject.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        UpdateLevelText();
        UpdateTimerText();
        UpdateLifeDisplay();
        UpdateBackground();
        UpdateHighestLevelText();
    }

    // ============================================================
    // REWARDED REKLAM SONRASI DEVAM ET
    // ============================================================

    public void ContinueAfterReward()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Ödüllü reklamdan sonra oyun devam ettiği için görünür
        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(true);
        }

        lives = 1;

        UpdateLifeDisplay();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (mineManager != null)
        {
            mineManager.gameObject.SetActive(true);
            mineManager.RefreshMines(currentLevel);
        }

        currentTime = levelTime;
        UpdateTimerText();

        if (buttonController != null)
        {
            buttonController.canPlay = true;
            buttonController.ResetClicks();
        }
    }
}