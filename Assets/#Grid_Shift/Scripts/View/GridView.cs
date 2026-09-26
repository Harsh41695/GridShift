using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject boxPrefab;

    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1.05f;

    private GameObject playerObject;

    private readonly Dictionary<Vector2Int, GameObject> boxObjects
    = new Dictionary<Vector2Int, GameObject>();

    public void Build(GridModel model)
    {
        for (int x = 0; x < model.Width; x++)
        {
            for (int y = 0; y < model.Height; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                Vector3 worldPosition = GridToWorld(gridPosition);

                Instantiate(floorPrefab, worldPosition, Quaternion.identity, transform);

                CellType cellType = model.GetCell(gridPosition);

                if (cellType == CellType.Wall)
                {
                    Instantiate(wallPrefab, worldPosition, Quaternion.identity, transform);
                }
                else if (cellType == CellType.Goal)
                {
                    Instantiate(goalPrefab, worldPosition, Quaternion.identity, transform);
                }

                if (model.HasBox(gridPosition))
                {
                    GameObject box = Instantiate(
                        boxPrefab,
                        worldPosition,
                        Quaternion.identity,
                        transform
                    );

                    boxObjects.Add(gridPosition, box);
                }

                if (model.PlayerPosition == gridPosition)
                {
                    playerObject = Instantiate(
                        playerPrefab,
                        worldPosition,
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
            gridPosition.y * cellSize,
            0f
        );
    }

    public void UpdatePlayerPosition(Vector2Int gridPosition)
    {
        if (playerObject == null)
            return;

        playerObject.transform.position = GridToWorld(gridPosition);
    }

    public void UpdateBoxPosition(
    Vector2Int from,
    Vector2Int to)
    {
        if (!boxObjects.TryGetValue(from, out GameObject box))
            return;

        boxObjects.Remove(from);
        boxObjects.Add(to, box);

        box.transform.position = GridToWorld(to);
    }
}