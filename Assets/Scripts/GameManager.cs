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

    public Image soundButtonImage;

    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool soundEnabled = true;

    public GameObject pauseButton;


    public bool isPaused = false;

    public int movesLeft = 20;
    public TMP_Text movesText;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AutoFindUIReferences();

        if (pausePanel != null) pausePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

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
    }

    public void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        Time.timeScale = 1f;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winPanel.transform.SetAsLastSibling();
            winPanel.transform.localScale = Vector3.zero;

            winPanel.transform
                .DOScale(Vector3.one, 0.35f)
                .SetEase(Ease.OutBack);
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

        AudioListener.volume =
            soundEnabled ? 1f : 0f;

        if (soundButtonImage != null)
        {
            soundButtonImage.sprite =
                soundEnabled
                ? soundOnSprite
                : soundOffSprite;
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
}