using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text movesText;
    [SerializeField] private GameObject winPanel;

    public void UpdateMoveCount(int moveCount)
    {
        movesText.text = $"Moves: {moveCount}";
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
    }

    public void HideWin()
    {
        winPanel.SetActive(false);
    }
}