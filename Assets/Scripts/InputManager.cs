using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void ColumnSelected(int columnIndex);
    public static event ColumnSelected OnColumnSelected;
   
    void Update()
    {
       // Checks for input per column
       for (int i = 0; i < 7; i++) // 7 columns    
       {
           if (Input.GetKeyDown(KeyCode.Alpha1 + i))
           {
               OnColumnSelected?.Invoke(i); //Activate the event for the selected column
           }
       } 
    }
}
