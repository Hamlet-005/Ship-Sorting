using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public static GameManager Instance;

    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private GameObject soundButtonObject;
    private Image soundButtonImage;

    private bool soundEnabled = true;
    private AudioManager audioManager;

    public GameObject pauseButton;


    public bool isPaused = false;

    public int movesLeft = 20;
    public TMP_Text movesText;


    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        AutoFindUIReferences();

        if (FindAnyObjectByType<AudioManager>() != null)
            audioManager = FindAnyObjectByType<AudioManager>();

        if (pausePanel != null) pausePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (soundButtonImage == null)
        {
            Image[] allImages = FindObjectsByType<Image>(FindObjectsSortMode.None);
            foreach (Image img in allImages)
            {
                if (img.gameObject.name == "SoundOn/OffButton")
                {
                    soundButtonImage = img;
                    break;
                }
            }
        }

        if (soundButtonObject != null)
            soundButtonImage = soundButtonObject.GetComponent<Image>();

        UpdateMovesUI();
    }

    public void UseMove()
    {
        if (gameEnded)
            return;

        movesLeft--;

        UpdateMovesUI();

        if (movesLeft <= 0)
        {
            Lose();
        }
    }

    void UpdateMovesUI()
    {
        if (movesText != null)
            movesText.text = "Moves: " + movesLeft;
    }

    void AutoFindUIReferences()
    {
        if (pausePanel == null)
            pausePanel = GameObject.Find("PausePanel");

        if (winPanel == null)
            winPanel = GameObject.Find("WinPanel");

        if (losePanel == null)
            losePanel = GameObject.Find("LosePanel");

        if (pauseButton == null)
            pauseButton = GameObject.Find("PauseButton");

        if (movesText == null)
        {
            GameObject movesObj = GameObject.Find("Moves-Text");

            if (movesObj != null)
                movesText = movesObj.GetComponent<TMPro.TMP_Text>();
        }

        if (soundButtonObject == null)
        {
            Transform found = FindInChildren(
                GameObject.Find("Canvas")?.transform,
                "SoundOn/OffButton"
            );
            if (found != null)
                soundButtonObject = found.gameObject;
        }

        if (soundButtonObject != null)
            soundButtonImage = soundButtonObject.GetComponent<Image>();

    }

    Transform FindInChildren(Transform parent, string name)
    {
        if (parent == null) return null;
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindInChildren(child, name);
            if (found != null) return found;
        }
        return null;
    }

    public void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        Time.timeScale = 1f;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (audioManager != null) audioManager.PlayWinSound();

        int stars = winPanel.GetComponentInChildren<StarDisplay>().CalculateStars(movesLeft);
        SaveStars(stars);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winPanel.transform.SetAsLastSibling();
            winPanel.transform.localScale = Vector3.zero;

            winPanel.transform
                .DOScale(Vector3.one, 0.35f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    StarDisplay starDisplay = winPanel.GetComponentInChildren<StarDisplay>();
                    if (starDisplay != null)
                        starDisplay.ShowStars(stars);
                });
        }
    }


    public void CheckWin()
    {
        if (gameEnded)
            return;

        Container[] containers =
            FindObjectsByType<Container>(FindObjectsSortMode.None);

        foreach (Container container in containers)
        {
            if (container.currentShip != null &&
                container.currentShip.isCompleted)
            {
                continue;
            }

            return;
        }

        Win();
    }

    public void Lose()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        Time.timeScale = 1f;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        audioManager.PlayLoseSound();

        if (losePanel != null)
        {
            losePanel.SetActive(true);
            losePanel.transform.SetAsLastSibling();
            losePanel.transform.localScale = Vector3.zero;

            losePanel.transform
                .DOScale(Vector3.one, 0.35f)
                .SetEase(Ease.OutBack);
        }
    }

    public void TogglePause()
    {
        if (gameEnded)
            return;

        isPaused = !isPaused;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
            pausePanel.transform.SetAsLastSibling();
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
            pauseButton.transform.SetAsLastSibling();
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        AudioListener.volume = soundEnabled ? 1f : 0f;

        if (soundButtonImage != null)
        {
            soundButtonImage.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public int GetStars()
    {
        int third = movesLeft / 3;

        if (movesLeft >= third * 2) return 3;
        if (movesLeft >= third) return 2;
        return 1;
    }

    public void SaveStars(int stars)
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        string key = "Stars_Level_" + level;

        int current = PlayerPrefs.GetInt(key, 0);
        if (stars > current)
        {
            PlayerPrefs.SetInt(key, stars);
            PlayerPrefs.Save();
        }
    }
}