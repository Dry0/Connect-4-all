using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; // Verwijzing naar GridManager
    [SerializeField] private GameObject player1DiscPrefab; // Prefab voor speler 1
    [SerializeField] private GameObject player2DiscPrefab; // Prefab voor speler 2

    private bool isPlayer1Turn = true; // Houd bij wiens beurt het is

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
        // Zoek de laagste lege cel in de kolom
        for (int row = gridManager.GetRows() - 1; row >= 0; row--)
        {
            GameObject cell = gridManager.GetCell(row, columnIndex);
            if (cell.transform.childCount == 0) // Controleer of de cel leeg is
            {
                // Selecteer de juiste prefab voor de huidige speler
                GameObject discPrefab = isPlayer1Turn ? player1DiscPrefab : player2DiscPrefab;

                // Plaats de schijf in deze cel
                Vector3 position = cell.transform.position;
                Instantiate(discPrefab, position, Quaternion.identity, cell.transform);

                // Wissel van speler
                isPlayer1Turn = !isPlayer1Turn;
                Debug.Log($"Speler {(isPlayer1Turn ? 1 : 2)} is aan de beurt!");
                return;
            }
        }

        Debug.LogWarning("Kolom is vol! Kies een andere kolom.");
    }
}



// [SerializeField] private GridManager gridManager; // Refrence to the gridmanager 
// [SerializeField] private GameObject discprefab; // prefab for the discs
//
// private void OnEnable()
// {
//     InputManager.OnColumnSelected += HandleColumnSelection;
// }
//
// private void OnDisable()
// {
//     InputManager.OnColumnSelected -= HandleColumnSelection;
// }
//
// private void HandleColumnSelection(int columnIndex)
// {
//     // Looks for the lowest empty cell in the column
//     for (int row = gridManager.GetRows() - 1; row >= 0; row--) // fout melding object refrence
//     {
//         GameObject cell = gridManager.GetCell(row, columnIndex);
//         if (cell.transform.childCount == 0) // Checks if the cell is empty
//         {
//             // Puts a disc in this cell
//             Vector3 position = cell.transform.position;
//             Instantiate(discprefab, position, Quaternion.identity, cell.transform);
//             Debug.Log($"Disc placed in column {{columnIndex}}, row {{row}}");
//             return;
//         }
//     }
//     Debug.LogWarning("Column is full pick a diffrent column .");
// }
