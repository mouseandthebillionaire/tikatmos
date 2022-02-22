using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Input Constants
    public KeyCode upCrank = KeyCode.UpArrow;
    public KeyCode downCrank = KeyCode.DownArrow;
    public KeyCode leftSlider = KeyCode.LeftArrow;
    public KeyCode rightSlider = KeyCode.RightArrow;
    public KeyCode knobLeft = KeyCode.Comma;
    public KeyCode knobRight = KeyCode.Period;
    public KeyCode deviceButton = KeyCode.Space;

    // We also need to keep track of constants from the Device & Window
    
    // Speech Tuning Constants
    // Have we successfully tuned to the customer's language
    public bool tuned;

    
    // Plant Care Constants?
    
    // Singleton
    public static GlobalVariables S;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (S == null) {
            S = this;
        } else {
            DestroyObject(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tuned);
    }
}
