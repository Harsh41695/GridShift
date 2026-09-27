using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private LevelData[] levels;

    private int currentLevelIndex = 0;
    private LevelData CurrentLevel => levels[currentLevelIndex];

    [Header("References")]
    [SerializeField] private GridView gridView;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameplayUI gameplayUI;

    private GridModel gridModel;
    private MoveHistory moveHistory;

    private bool levelComplete;


    private void Start()
    {
        moveHistory = new MoveHistory();

        CreateLevel();

        gameplayUI.UpdateMoveCount(0);
        gameplayUI.HideWin();
    }


    private void OnEnable()
    {
        inputController.OnMove += HandleMove;
        inputController.OnUndo += HandleUndo;
        inputController.OnRestart += RestartLevel;
    }


    private void OnDisable()
    {
        inputController.OnMove -= HandleMove;
        inputController.OnUndo -= HandleUndo;
        inputController.OnRestart -= RestartLevel;
    }



    private void CreateLevel()
    {
        gridModel = new GridModel(
            CurrentLevel.Width,
            CurrentLevel.Height
        );

        foreach (Vector2Int position in CurrentLevel.WallPositions)
        {
            gridModel.SetCell(position, CellType.Wall);
        }

        foreach (Vector2Int position in CurrentLevel.GoalPositions)
        {
            gridModel.SetCell(position, CellType.Goal);
        }

        foreach (Vector2Int position in CurrentLevel.BoxPositions)
        {
            gridModel.AddBox(position);
        }

        gridModel.SetPlayerPosition(
            CurrentLevel.PlayerStartPosition
        );

        gridView.Build(gridModel);
    }


    private void LoadLevel()
    {
        levelComplete = false;

        moveHistory.Clear();

        gridView.Clear();

        CreateLevel();

        gameplayUI.UpdateMoveCount(0);
        gameplayUI.HideWin();
    }


    private void HandleMove(Vector2Int direction)
    {
        if (levelComplete)
            return;

        MoveResult result =
            gridModel.TryMovePlayer(direction);

        if (!result.Success)
            return;

        moveHistory.Record(result);

        gridView.UpdatePlayerPosition(
            result.PlayerTo
        );

        if (result.BoxMoved)
        {
            gridView.UpdateBoxPosition(
                result.BoxFrom,
                result.BoxTo
            );
        }

        gameplayUI.UpdateMoveCount(
            moveHistory.MoveCount
        );

        CheckLevelComplete();
    }

    public void HandleUndo()
    {
        if (levelComplete)
            return;

        if (!moveHistory.TryUndo(out MoveResult move))
            return;

        gridModel.UndoMove(move);

        gridView.UpdatePlayerPosition(
            move.PlayerFrom
        );

        if (move.BoxMoved)
        {
            gridView.UpdateBoxPosition(
                move.BoxTo,
                move.BoxFrom
            );
        }

        gameplayUI.UpdateMoveCount(
            moveHistory.MoveCount
        );
    }



    private void CheckLevelComplete()
    {
        if (!gridModel.IsLevelComplete())
            return;

        levelComplete = true;

        gameplayUI.ShowWin();
    }



    public void RestartLevel()
    {
        LoadLevel();
    }



    public void NextLevel()
    {
        if (currentLevelIndex < levels.Length - 1)
        {
            currentLevelIndex++;

            LoadLevel();
        }
        else
        {
            Debug.Log("GAME COMPLETE!");
        }
    }
}