using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text movesText;

    [Header("Popups")]
    [SerializeField] private PopupAnimation winPanel;
    [SerializeField] private PopupAnimation gameCompletePanel;
    [SerializeField] private GameObject instructionPanel;

    public void UpdateMoveCount(int moveCount)
    {
        movesText.text = $"Moves: {moveCount}";
    }

    public void ShowWin()
    {
        winPanel.Show();
    }

    public void HideWin()
    {
        winPanel.gameObject.SetActive(false);
    }

    public void ShowGameComplete()
    {
        winPanel.gameObject.SetActive(false);
        gameCompletePanel.Show();
    }

    public void HideGameComplete()
    {
        gameCompletePanel.gameObject.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionPanel.SetActive(false);
    }
}