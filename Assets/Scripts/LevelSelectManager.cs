using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Image[][] levelStars; 

    public Sprite filledStar;
    public Sprite emptyStar;

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            int stars = PlayerPrefs.GetInt("Stars_Level_" + levelIndex, 0);

            Transform levelBtn = levelButtons[i].transform;
            for (int s = 0; s < 3; s++)
            {
                Transform star = levelBtn.Find("Star-" + (s + 1));
                if (star != null)
                {
                    Image img = star.GetComponent<Image>();
                    img.sprite = filledStar;
                    img.color = s < stars ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
                }
            }

            int index = levelIndex;
            levelButtons[i].onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Level-" + index);
            });
        }
    }
}
