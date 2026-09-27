using UnityEngine;

public struct MoveResult
{
    public bool Success;

    public Vector2Int PlayerFrom;
    public Vector2Int PlayerTo;

    public bool BoxMoved;
    public Vector2Int BoxFrom;
    public Vector2Int BoxTo;
    public bool UsedConveyor;
}