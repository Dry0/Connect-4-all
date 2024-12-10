using TMPro;
using UnityEngine;
using UnityEngine.UI; 
public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; // Reference to GridManager
    [SerializeField] private GameObject player1DiscPrefab; // Prefab for player 1
    [SerializeField] private GameObject player2DiscPrefab; // Prefab for player 2
    [SerializeField] private GameObject winPanel; // Panel that appears when a player wins
    [SerializeField] private TextMeshProUGUI winMessageText; // Tekxt that show who won

    private bool _isPlayer1Turn = true; // Keep track of whose turn it is
    private bool _isGameOver; // Keep track of whether the game has ended, default is false 

    private void Start()
    {
        winPanel.SetActive(false); // Make sure the win UI is hidden at startup
    }

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
        if (_isGameOver) return; // Stop input when the game is over

        // Find the lowest available cell in the column
        for (int row = gridManager.GetRows() - 1; row >= 0; row--)
        {
            GameObject cell = gridManager.GetCell(row, columnIndex);
            if (cell.transform.childCount == 0) // Check that the cell is empty
            {
                // Choose the right prefab for the current player
                GameObject discPrefab = _isPlayer1Turn ? player1DiscPrefab : player2DiscPrefab;

                // Places the disc in this cell
                Vector3 position = cell.transform.position;
                GameObject newDisc = Instantiate(discPrefab, position, Quaternion.identity, cell.transform);

                // Saves the current player before the turn changes
                bool currentPlayerIsPlayer1 = _isPlayer1Turn;

                // See if this move produced a winner
                if (CheckForWin(row, columnIndex, newDisc))
                {
                    EndGame(currentPlayerIsPlayer1);
                }
                else
                {
                    // Switches turn if nobody wins
                    _isPlayer1Turn = !_isPlayer1Turn;
                    Debug.Log($"Player {(_isPlayer1Turn ? 1 : 2)} turn!");
                }

                return;
            }
        }

        Debug.LogWarning("Column is full pick a different column.");
    }

    private bool CheckForWin(int row, int column, GameObject playerDisc)
    {
        // Checks for horizontal, vertical and diagonal
        return (CountInDirection(row, column, 1, 0, playerDisc) + CountInDirection(row, column, -1, 0, playerDisc) >= 3 || // Horizontaal
                CountInDirection(row, column, 0, 1, playerDisc) + CountInDirection(row, column, 0, -1, playerDisc) >= 3 || // Verticaal
                CountInDirection(row, column, 1, 1, playerDisc) + CountInDirection(row, column, -1, -1, playerDisc) >= 3 || // Diagonaal (\)
                CountInDirection(row, column, 1, -1, playerDisc) + CountInDirection(row, column, -1, 1, playerDisc) >= 3);  // Diagonaal (/)
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

            if (cell.transform.childCount > 0 &&
                cell.transform.GetChild(0).gameObject.CompareTag(playerDisc.tag))
            {
                count++;
                currentRow += rowDirection;
                currentColumn += columnDirection;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    private void EndGame(bool player1Won)
    {
        _isGameOver = true; // Set the game on "Ended"
        winMessageText.text = $"Player {(player1Won ? 1 : 2)} wins!"; // Shows the winner
        winPanel.SetActive(true); // Make the win UI visible (with the Play Again button)
    }

    public void PlayAgain()
    {
        // Resets the game
        foreach (Transform cell in gridManager.transform)
        {
            if (cell.childCount > 0)
            {
                Destroy(cell.GetChild(0).gameObject); // Deletes all discs
            }
        }

        winPanel.SetActive(false); // Hides the WinPanel
        _isGameOver = false; // Resets the player status
        _isPlayer1Turn = true; // Player 1 starts again
    }
}