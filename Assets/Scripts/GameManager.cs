using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int movesLeft = 20;
    public TMP_Text movesText;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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

    public void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        Debug.Log("WIN");
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
        Debug.Log("LOSE");
    }
}