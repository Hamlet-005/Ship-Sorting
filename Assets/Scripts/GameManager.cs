using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int movesLeft = 20;
    public TMP_Text movesText;

    void Start()
    {
        UpdateMovesUI();
    }

    void Awake()
    {
        Instance = this;
    }

    public void UseMove()
    {
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

    public void CheckWin()
    {
        Ship[] ships = FindObjectsByType<Ship>(FindObjectsSortMode.None);

        foreach (Ship ship in ships)
        {
            if (ship.isCompleted)
                continue;

            if (ship.EmptySlotCount() != ship.slots.Length)
                return;
        }

        Win();
    }

    public void Win()
    {
        Debug.Log("WIN");
    }

    public void Lose()
    {
        Debug.Log("LOSE");
    }
}