using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject levelsPanel;
    public GameObject settingsPanel;

    public Image soundButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool soundEnabled = true;

    public void OpenLevels()
    {
        levelsPanel.SetActive(true);
    }

    public void CloseLevels()
    {
        levelsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        AudioListener.volume = soundEnabled ? 1f : 0f;

        if (soundButtonImage != null)
            soundButtonImage.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
    }
}