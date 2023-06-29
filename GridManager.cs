using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public int gridSizeX = 5;
    public int gridSizeY = 10;
    public float cellSize = 1.1f;

    public GameObject[,] gridCells;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        float startX = -(gridSizeX - 1) * cellSize / 2f;
        float startY = -(gridSizeY - 1) * cellSize / 2f;

        gridCells = new GameObject[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 cellPosition = new Vector3(startX + x * cellSize, startY + y * cellSize, 0f);
                GameObject cell = Instantiate(gridCellPrefab, cellPosition, Quaternion.identity);
                cell.transform.parent = transform;
                cell.name = "Cell " + x + "," + y;
                gridCells[x, y] = cell;
            }
        }
    }
}
