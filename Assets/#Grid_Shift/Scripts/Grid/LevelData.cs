using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "GridShift/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    [SerializeField] private Vector2Int playerStartPosition;

    [SerializeField] private Vector2Int[] wallPositions;
    [SerializeField] private Vector2Int[] goalPositions;
    [SerializeField] private Vector2Int[] boxPositions;

    public int Width => width;
    public int Height => height;

    public Vector2Int PlayerStartPosition => playerStartPosition;

    public Vector2Int[] WallPositions => wallPositions;
    public Vector2Int[] GoalPositions => goalPositions;
    public Vector2Int[] BoxPositions => boxPositions;

    [SerializeField] private Vector2Int[] conveyorRightPositions;

    public Vector2Int[] ConveyorRightPositions =>
        conveyorRightPositions;
}