using UnityEngine;

public class ColumnSelector : MonoBehaviour
{
    public delegate void ColumnClicked(int columnIndex);
    public static event ColumnClicked OnColumnClicked;

    [SerializeField] private int columnIndex; // De index van deze kolom

    private void OnMouseDown()
    {
        OnColumnClicked?.Invoke(columnIndex); // Activeer het event voor deze kolom
    }
}