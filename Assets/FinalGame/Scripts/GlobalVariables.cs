using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public KeyCode upCrank = KeyCode.UpArrow;
    public KeyCode downCrank = KeyCode.DownArrow;
    public KeyCode leftSlider = KeyCode.LeftArrow;
    public KeyCode rightSlider = KeyCode.RightArrow;
    public KeyCode knobLeft = KeyCode.Comma;
    public KeyCode knobRight = KeyCode.Period;
    public KeyCode deviceButton = KeyCode.Space;

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
        
    }
}
