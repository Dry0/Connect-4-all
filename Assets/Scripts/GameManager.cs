using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; // Refrence to GridManager
    [SerializeField] private GameObject player1DiscPrefab; // Prefab for player 1
    [SerializeField] private GameObject player2DiscPrefab; // Prefab for player 2

    private bool isPlayer1Turn = true; // Keep track of whose turn it is

    private void OnEnable()
    {
        ColumnSelector.OnColumnClicked += HandleColumnClick;
    }

    private void OnDisable()
    {
        ColumnSelector.OnColumnClicked -= HandleColumnClick;
    }

    private void HandleColumnClick(int columnIndex)
    {
        // Looks for the lowest cell in the column
        for (int row = gridManager.GetRows() - 1; row >= 0; row--)
        {
            GameObject cell = gridManager.GetCell(row, columnIndex);
            if (cell.transform.childCount == 0) // Checks if the cell is empty
            {
                // Selects the right Prefab for the current player
                GameObject discPrefab = isPlayer1Turn ? player1DiscPrefab : player2DiscPrefab;

                // Places the disc in this cell 
                Vector3 position = cell.transform.position;
                GameObject newDisc = Instantiate(discPrefab, position, Quaternion.identity, cell.transform);
                
                // Looks for the winner
                CheckForWin(row,columnIndex, newDisc);

                // Switches player
                isPlayer1Turn = !isPlayer1Turn;
                Debug.Log($"Speler {(isPlayer1Turn ? 1 : 2)} is aan de beurt!");
                return;
            }
        }

        Debug.LogWarning("Kolom is vol! Kies een andere kolom.");
    }

    private void CheckForWin(int row, int column, GameObject playerDisc)
    {
        if (CountInDirection(row,column, 1,0, playerDisc) + CountInDirection(row, column, -1, 0, playerDisc) >= 3 || // Horizontal
            CountInDirection(row, column, 0, 1, playerDisc) + CountInDirection(row, column, 0, 1, playerDisc) >= 3 || // Vertical
            CountInDirection(row, column, 1, 1, playerDisc) + CountInDirection(row, column, -1, -1, playerDisc) >= 3 || // Diagonal (\)
            CountInDirection(row, column, 1,-1, playerDisc) + CountInDirection(row, column, -1, 1, playerDisc) >= 3); // Diagonal (/)
        {
            Debug.Log($"Player {(isPlayer1Turn ? 2: 1)} Wins!"); // Player 1 or 2 wins (change already after turn)
            // Here you can add further actions, such as showing a UI screen or stopping the game.
        }
    }

    private int CountInDirection(int startRow, int startColumn, int rowDirection, int columnDirection, GameObject playerDisc)
    {
        int count = 0;
        int currentRow = startRow + rowDirection;
        int currentColumn = startColumn + columnDirection;
        
        while (currentRow >= 0 && currentRow < gridManager.GetRows() &&
               currentColumn >= 0 && currentColumn < gridManager.GetColumns())
        {
            GameObject cell = gridManager.GetCell(currentRow, currentColumn);
            
            // Checks if the cell has a disc of the same player
            if (cell.transform.childCount > 0 &&
                cell.transform.GetChild(0).gameObject.CompareTag(playerDisc.tag))
            {
                count++;
                currentRow += rowDirection;
                currentColumn += columnDirection;
            }
            else
            {
                break; // Stops if the line gets interrupted
            }
        }

        return count;
    }
    
}