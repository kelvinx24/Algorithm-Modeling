using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [Header("Grid Settings")]
    [Range(1, 6)]
    public int n = 3; // Resulting grid size will be 2^n x 2^n
    public float cellSize = 1f;
    public float cellSpacing = 0.1f;

    [Header("References")]
    public GameObject cellPrefab; // Assign in Inspector

    private GameObject[,] cells;
    private int gridSize;

    void Start()
    {
        SpawnGrid();
        GetComponent<TrinomeTiling>().DoTiling(0, 0, gridSize, 0, 1);
    }

    void SpawnGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("Cell prefab is not assigned!");
            return;
        }

        gridSize = (int)Mathf.Pow(2, n);
        cells = new GameObject[gridSize, gridSize];
        float offset = (gridSize * (cellSize + cellSpacing) - cellSpacing) / 2f;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Vector3 pos = new Vector3(
                    row * (cellSize + cellSpacing) - offset + cellSize / 2f,
                    0,
                    col * (cellSize + cellSpacing) - offset + cellSize / 2f

                );

                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cell.transform.localScale = Vector3.one * cellSize;
                cell.name = $"Cell_{row}_{col}";
                cells[row, col] = cell;
            }
        }
    }

    bool IsInBounds(int row, int col)
    {
        return row >= 0 && row < gridSize && col >= 0 && col < gridSize;
    }

    public void SetCellColor(int row, int col, Color color)
    {
        if (IsInBounds(row, col))
        {
            Renderer cellRenderer = cells[row, col].GetComponent<Renderer>();
            if (cellRenderer != null)
            {
                cellRenderer.material.color = color;
            }
        }
    }
}
