using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Input Constants
    // note: renamed the keys to better fit the actual inputs
    public KeyCode upCrank = KeyCode.O;
    public KeyCode downCrank = KeyCode.P;
    public KeyCode knob0_left = KeyCode.K;
    public KeyCode knob0_right = KeyCode.L;
    public KeyCode knob1_left = KeyCode.N;
    public KeyCode knob1_right = KeyCode.M;
    public KeyCode deviceButton = KeyCode.Space;

    public KeyCode customerServedCorrectly = KeyCode.C;
    public KeyCode loadNextCustomer = KeyCode.Period;
    //
    // // Get rid of these later
    public KeyCode leftSlider = KeyCode.K;
    public KeyCode rightSlider = KeyCode.L;
    public KeyCode knobLeft = KeyCode.N;
    public KeyCode knobRight = KeyCode.M;
    
    // Channel
    public int deviceChannel;

    // We also need to keep track of constants from the Device & Window
    
    // Speech Tuning Constants
    // Have we successfully tuned to the customer's language
    public bool tuned;

    // Is there currently a customer
    public bool customerActive;
    
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
}
