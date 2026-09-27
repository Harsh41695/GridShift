using System.Collections.Generic;
using UnityEngine;

public class GridModel
{
    private readonly int width;
    private readonly int height;

    private readonly CellType[,] cells;
    private readonly HashSet<Vector2Int> boxPositions;

    public int Width => width;
    public int Height => height;

    public Vector2Int PlayerPosition { get; private set; }

    private readonly HashSet<Vector2Int> conveyorRightPositions
    = new HashSet<Vector2Int>();

    public GridModel(int width, int height)
    {
        this.width = width;
        this.height = height;

        cells = new CellType[width, height];
        boxPositions = new HashSet<Vector2Int>();
    }

    public CellType GetCell(Vector2Int position)
    {
        return cells[position.x, position.y];
    }

    public void SetCell(Vector2Int position, CellType type)
    {
        cells[position.x, position.y] = type;
    }

    public bool IsInside(Vector2Int position)
    {
        return position.x >= 0 &&
               position.x < width &&
               position.y >= 0 &&
               position.y < height;
    }

    public bool HasBox(Vector2Int position)
    {
        return boxPositions.Contains(position);
    }

    public void AddBox(Vector2Int position)
    {
        boxPositions.Add(position);
    }

    public void RemoveBox(Vector2Int position)
    {
        boxPositions.Remove(position);
    }

    public void SetPlayerPosition(Vector2Int position)
    {
        PlayerPosition = position;
    }

    public MoveResult TryMovePlayer(Vector2Int direction)
    {
        MoveResult result = new MoveResult
        {
            Success = false,
            PlayerFrom = PlayerPosition,
            PlayerTo = PlayerPosition,
            BoxMoved = false,
            UsedConveyor = false
        };

        Vector2Int targetPosition = PlayerPosition + direction;

        if (!IsInside(targetPosition))
            return result;

        if (GetCell(targetPosition) == CellType.Wall)
            return result;

        if (HasBox(targetPosition))
        {
            Vector2Int boxTargetPosition =
                targetPosition + direction;

            if (!IsInside(boxTargetPosition))
                return result;

            if (GetCell(boxTargetPosition) == CellType.Wall)
                return result;

            if (HasBox(boxTargetPosition))
                return result;

            Vector2Int finalBoxPosition = boxTargetPosition;

           

            if (IsRightConveyor(boxTargetPosition))
            {
                Vector2Int conveyorTarget =
                    boxTargetPosition + Vector2Int.right;

                bool canMoveRight =
                    IsInside(conveyorTarget) &&
                    GetCell(conveyorTarget) != CellType.Wall &&
                    !HasBox(conveyorTarget);

                if (canMoveRight)
                {
                    finalBoxPosition = conveyorTarget;
                    result.UsedConveyor = true;
                }
            }

            RemoveBox(targetPosition);
            AddBox(finalBoxPosition);

            result.BoxMoved = true;

            result.BoxFrom = targetPosition;

            result.BoxTo = finalBoxPosition;
        }

        PlayerPosition = targetPosition;

        result.Success = true;
        result.PlayerTo = targetPosition;

        return result;
    }

    public void UndoMove(MoveResult move)
    {
        PlayerPosition = move.PlayerFrom;

        if (move.BoxMoved)
        {
            RemoveBox(move.BoxTo);
            AddBox(move.BoxFrom);
        }
    }
    public bool IsLevelComplete()
    {
        foreach (Vector2Int boxPosition in boxPositions)
        {
            if (GetCell(boxPosition) != CellType.Goal)
                return false;
        }

        return boxPositions.Count > 0;
    }

    public void AddRightConveyor(Vector2Int position)
    {
        conveyorRightPositions.Add(position);
    }

    public bool IsRightConveyor(Vector2Int position)
    {
        return conveyorRightPositions.Contains(position);
    }
}