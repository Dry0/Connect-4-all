using UnityEngine;

public class ColumnSelector : MonoBehaviour
{
    public delegate void ColumnClicked(int columnIndex);
    public static event ColumnClicked OnColumnClicked;

    [SerializeField] private int columnIndex; // De index of this column

    private void OnMouseDown()
    {
        OnColumnClicked?.Invoke(columnIndex); //Activates the event for this column
    }
}