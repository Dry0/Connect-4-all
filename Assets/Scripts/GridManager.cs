using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // the grid: 2D Array of GameObjects
    private GameObject[,] _gridArray;
    
    // grid specifics
    [SerializeField] private int rows = 6; // Amount of rows
    [SerializeField] private int columns = 7; // Amount of columns
    [SerializeField] private GameObject cellPrefab; // Prefab for individual cells
    
    // Start gets used for initialization
    void Start()
    {
        CreateGrid();
    }

    public int GetRows()
    {
        return rows;
    }

    public int GetColumns()
    {
        return columns;
    }
    
    // Method to make grid
    private void CreateGrid()
    {
        _gridArray = new GameObject[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++) 
            {
                // Make a new cell
                GameObject cell = Instantiate(cellPrefab, new Vector3(col, -row, 0), Quaternion.identity);
                cell.name = $"Cell ({row}), ({col})";
                
                // Adds a cell to the grid
                _gridArray[row, col] = cell;
                
                // Set the cell as a child of the GridManager for better organization
                cell.transform.parent = transform;
            }
        }
    }
    
    // Optional method
    public GameObject GetCell(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < columns)
        {
            return _gridArray[row, col];
        }
        Debug.LogWarning("GetCell: invalid Index!");
        return null;
    }
}
