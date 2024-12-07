using UnityEngine;
using UnityEngine.UI; // Voor UI-elementen

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; // Reference to GridManager
    [SerializeField] private GameObject player1DiscPrefab; // Prefab for player 1
    [SerializeField] private GameObject player2DiscPrefab; // Prefab for player 2
    [SerializeField] private GameObject winPanel; // Panel dat verschijnt bij winst
    [SerializeField] private Text winMessageText; // Tekst die de winnaar toont

    private bool _isPlayer1Turn = true; // Keep track of whose turn it is
    private bool _isGameOver; // Houd bij of het spel is afgelopen, standaard is false

    private void Start()
    {
        winPanel.SetActive(false); // Zorg dat de win UI verborgen is bij start
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
        if (_isGameOver) return; // Stop input als het spel voorbij is

        // Zoek de laagste beschikbare cel in de kolom
        for (int row = gridManager.GetRows() - 1; row >= 0; row--)
        {
            GameObject cell = gridManager.GetCell(row, columnIndex);
            if (cell.transform.childCount == 0) // Controleer of de cel leeg is
            {
                // Kies de juiste prefab voor de huidige speler
                GameObject discPrefab = _isPlayer1Turn ? player1DiscPrefab : player2DiscPrefab;

                // Plaats de schijf in deze cel
                Vector3 position = cell.transform.position;
                GameObject newDisc = Instantiate(discPrefab, position, Quaternion.identity, cell.transform);

                // Sla de huidige speler op voordat de beurt wisselt
                bool currentPlayerIsPlayer1 = _isPlayer1Turn;

                // Kijk of deze zet een winnaar heeft opgeleverd
                if (CheckForWin(row, columnIndex, newDisc))
                {
                    EndGame(currentPlayerIsPlayer1);
                }
                else
                {
                    // Wissel van beurt als niemand wint
                    _isPlayer1Turn = !_isPlayer1Turn;
                    Debug.Log($"Speler {(_isPlayer1Turn ? 1 : 2)} is aan de beurt!");
                }

                return;
            }
        }

        Debug.LogWarning("Kolom is vol! Kies een andere kolom.");
    }

    private bool CheckForWin(int row, int column, GameObject playerDisc)
    {
        // Controleer horizontaal, verticaal en diagonaal
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
        _isGameOver = true; // Zet het spel op "beëindigd"
        winMessageText.text = $"Speler {(player1Won ? 1 : 2)} wint!"; // Toon de winnaar
        winPanel.SetActive(true); // Maak het win UI zichtbaar (met de Play Again knop)
    }

    public void PlayAgain()
    {
        // Reset het spel
        foreach (Transform cell in gridManager.transform)
        {
            if (cell.childCount > 0)
            {
                Destroy(cell.GetChild(0).gameObject); // Verwijder alle discs
            }
        }

        winPanel.SetActive(false); // Verberg de win UI
        _isGameOver = false; // Reset de spelstatus
        _isPlayer1Turn = true; // Speler 1 begint opnieuw
    }
}