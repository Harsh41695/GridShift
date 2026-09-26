using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private GridView gridView;

    private GridModel gridModel;

    [SerializeField] private InputController inputController;
    private MoveHistory moveHistory;

   
    private void Start()
    {
        moveHistory = new MoveHistory();
        CreateLevel();
    }
    private void OnEnable()
    {
        inputController.OnMove += HandleMove;
        inputController.OnUndo += HandleUndo;

    }

    private void OnDisable()
    {
        inputController.OnMove -= HandleMove;
        inputController.OnUndo -= HandleUndo;
    }
    private void CreateLevel()
    {
        gridModel = new GridModel(
            levelData.Width,
            levelData.Height
        );

        foreach (Vector2Int position in levelData.WallPositions)
        {
            gridModel.SetCell(position, CellType.Wall);
        }

        foreach (Vector2Int position in levelData.GoalPositions)
        {
            gridModel.SetCell(position, CellType.Goal);
        }

        foreach (Vector2Int position in levelData.BoxPositions)
        {
            gridModel.AddBox(position);
        }

        gridModel.SetPlayerPosition(
            levelData.PlayerStartPosition
        );

        gridView.Build(gridModel);
    }

    private void HandleMove(Vector2Int direction)
    {
        MoveResult result = gridModel.TryMovePlayer(direction);

        if (!result.Success)
            return;

        moveHistory.Record(result);

        gridView.UpdatePlayerPosition(result.PlayerTo);

        if (result.BoxMoved)
        {
            gridView.UpdateBoxPosition(
                result.BoxFrom,
                result.BoxTo
            );
        }

        Debug.Log($"Moves: {moveHistory.MoveCount}");

        if (gridModel.IsLevelComplete())
        {
            Debug.Log("LEVEL COMPLETE!");
        }
    }
    private void HandleUndo()
    {
        if (!moveHistory.TryUndo(out MoveResult move))
            return;

        gridModel.UndoMove(move);

        gridView.UpdatePlayerPosition(move.PlayerFrom);

        if (move.BoxMoved)
        {
            gridView.UpdateBoxPosition(
                move.BoxTo,
                move.BoxFrom
            );
        }

        Debug.Log($"Moves: {moveHistory.MoveCount}");
    }
}