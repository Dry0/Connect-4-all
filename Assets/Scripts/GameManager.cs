using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; // Refrence to the gridmanager 
    [SerializeField] private GameObject discprefab; // prefab for the discs

    private void OnEnable()
    {
        InputManager.OnColumnSelected += HandleColumnSelection;
    }

    private void OnDisable()
    {
        InputManager.OnColumnSelected -= HandleColumnSelection;
    }

    private void HandleColumnSelection(int columnIndex)
    {
        // Looks for the lowest empty cell in the column
        for (int row = gridManager.GetRows() - 1; row >= 0; row--) // fout melding object refrence
        {
            GameObject cell = gridManager.GetCell(row, columnIndex);
            if (cell.transform.childCount == 0) // Checks if the cell is empty
            {
                // Puts a disc in this cell
                Vector3 position = cell.transform.position;
                Instantiate(discprefab, position, Quaternion.identity, cell.transform);
                Debug.Log($"Disc placed in column {{columnIndex}}, row {{row}}");
                return;
            }
        }
        Debug.LogWarning("Column is full pick a diffrent column .");
    }
}
