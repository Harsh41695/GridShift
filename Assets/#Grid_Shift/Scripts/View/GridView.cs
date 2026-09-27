using System.Collections;
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
    [SerializeField] private GameObject conveyorRightPrefab;

    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1.05f;

    [Header("Movement")]
    [SerializeField] private float moveDuration = 0.15f;

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

                Instantiate(
                    floorPrefab,
                    worldPosition,
                    Quaternion.identity,
                    transform
                );

                CellType cellType = model.GetCell(gridPosition);

                if (cellType == CellType.Wall)
                {
                    Instantiate(
                        wallPrefab,
                        worldPosition,
                        Quaternion.identity,
                        transform
                    );
                }
                else if (cellType == CellType.Goal)
                {
                    Instantiate(
                        goalPrefab,
                        worldPosition,
                        Quaternion.identity,
                        transform
                    );
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

                if (model.IsRightConveyor(gridPosition))
                {
                    Instantiate(
                        conveyorRightPrefab,
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

        StopCoroutineIfNeeded(playerObject);

        StartCoroutine(
            MoveObject(
                playerObject.transform,
                GridToWorld(gridPosition)
            )
        );
    }



    public void UpdateBoxPosition(
        Vector2Int from,
        Vector2Int to)
    {
        if (!boxObjects.TryGetValue(from, out GameObject box))
            return;

        boxObjects.Remove(from);
        boxObjects.Add(to, box);

        StopCoroutineIfNeeded(box);

        StartCoroutine(
            MoveObject(
                box.transform,
                GridToWorld(to)
            )
        );
    }


    public void UpdateBoxPositionWithConveyor(
     Vector2Int from,
     Vector2Int conveyorPosition,
     Vector2Int finalPosition)
    {
        if (!boxObjects.TryGetValue(from, out GameObject box))
            return;

        boxObjects.Remove(from);
        boxObjects.Add(finalPosition, box);

        StartCoroutine(
            MoveBoxThroughConveyor(
                box.transform,
                GridToWorld(conveyorPosition),
                GridToWorld(finalPosition)
            )
        );
    }

    private IEnumerator MoveBoxThroughConveyor(
    Transform boxTransform,
    Vector3 conveyorPosition,
    Vector3 finalPosition)
    {
        yield return MoveObject(
            boxTransform,
            conveyorPosition
        );

        yield return new WaitForSeconds(0.05f);

        yield return MoveObject(
            boxTransform,
            finalPosition
        );
    }

    private IEnumerator MoveObject(
        Transform objectTransform,
        Vector3 targetPosition)
    {
        Vector3 startPosition = objectTransform.position;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsedTime / moveDuration
            );

            // Smooth acceleration/deceleration
            t = Mathf.SmoothStep(0f, 1f, t);

            objectTransform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        objectTransform.position = targetPosition;
    }


    private void StopCoroutineIfNeeded(GameObject target)
    {
        // For now intentionally empty.
        // Input locking will prevent overlapping animations.
    }


   
    public void Clear()
    {
        StopAllCoroutines();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        boxObjects.Clear();
        playerObject = null;
    }
}