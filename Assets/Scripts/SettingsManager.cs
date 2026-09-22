using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Panel")]
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button soundButton;
    public Button musicButton;
    public Button vibrationButton;
    public Button backButton;

    [Header("Button Texts")]
    public TextMeshProUGUI soundButtonText;
    public TextMeshProUGUI musicButtonText;
    public TextMeshProUGUI vibrationButtonText;

    [Header("Audio")]
    public AudioSource mineAudioSource;
    public AudioSource backgroundMusicSource;

    private bool soundOn = true;
    private bool musicOn = true;
    private bool vibrationOn = true;

    void Start()
    {
        // Kayıtlı ayarları yükle
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        vibrationOn = PlayerPrefs.GetInt("VibrationOn", 1) == 1;

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (soundButton != null)
        {
            soundButton.onClick.AddListener(ToggleSound);
        }

        if (musicButton != null)
        {
            musicButton.onClick.AddListener(ToggleMusic);
        }

        if (vibrationButton != null)
        {
            vibrationButton.onClick.AddListener(ToggleVibration);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSettings);
        }

        ApplySettings();
        UpdateButtonTexts();
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    void ToggleSound()
    {
        soundOn = !soundOn;

        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySettings();
        UpdateButtonTexts();
    }

    void ToggleMusic()
    {
        musicOn = !musicOn;

        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySettings();
        UpdateButtonTexts();
    }

    void ToggleVibration()
    {
        vibrationOn = !vibrationOn;

        PlayerPrefs.SetInt("VibrationOn", vibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        if (vibrationOn)
        {
            Handheld.Vibrate();
        }

        UpdateButtonTexts();
    }

    void ApplySettings()
    {
        if (mineAudioSource != null)
        {
            mineAudioSource.mute = !soundOn;
        }

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.mute = !musicOn;
        }
    }

    void UpdateButtonTexts()
    {
        if (soundButtonText != null)
        {
            soundButtonText.text = soundOn ? "AÇIK" : "KAPALI";
        }

        if (musicButtonText != null)
        {
            musicButtonText.text = musicOn ? "AÇIK" : "KAPALI";
        }

        if (vibrationButtonText != null)
        {
            vibrationButtonText.text = vibrationOn ? "AÇIK" : "KAPALI";
        }
    }

    // MineManager tarafından kullanılır
    public bool IsVibrationOn()
    {
        return vibrationOn;
    }
}