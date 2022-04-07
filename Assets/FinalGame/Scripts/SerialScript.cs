using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialScript : MonoBehaviour
{
    // Serial data
    SerialPort stream = new SerialPort("COM5", 9600);
    Thread serialThread;
    string serialData;
    private bool serialReceived = false;

    // Global variables
    public bool crankUp;
    public bool crankDown;
    public bool knobUp;
    public bool knobDown;
    public bool knobLeft;
    public bool knobRight;
    public bool deviceButton;

    private bool buttonPressed = false;

    // Singleton
    public static SerialScript S;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Create Singleton
        if (S == null) S = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize serial
        if (!stream.IsOpen) {
            if (SerialPort.GetPortNames().Length > 0) {
                stream.Open();

                serialThread = new Thread(new ThreadStart(ParseData));
                serialThread.Start();

                print("Connected to serial");
            }
        }

        ResetVariables();

        // Update the crank
        if (Input.GetKeyDown(KeyCode.Period)) crankUp = true;
        if (Input.GetKeyDown(KeyCode.Comma)) crankDown = true;

        // Update the horizontal knob
        if (Input.GetKeyDown(KeyCode.LeftArrow)) knobLeft = true;
        if (Input.GetKeyDown(KeyCode.RightArrow)) knobRight = true;

        // Update the vertical knob
        if (Input.GetKeyDown(KeyCode.UpArrow)) knobUp = true;
        if (Input.GetKeyDown(KeyCode.DownArrow)) knobDown = true;

        // Update the button
        if (Input.GetKeyDown(KeyCode.Space)) deviceButton = true;
        if (buttonPressed) {
            deviceButton = true;
            buttonPressed = false;
        }
    }

    void ResetVariables() {
        if (!serialReceived) {
            // Reset variables
            if (crankUp) crankUp = false;
            if (crankDown) crankDown = false;
            if (knobLeft) knobLeft = false;
            if (knobRight) knobRight = false;
            if (knobUp) knobUp = false;
            if (knobDown) knobDown = false;
            if (deviceButton) deviceButton = false;
        }
        else serialReceived = false;
    }

    void OnApplicationQuit()
    {
        if (stream.IsOpen) {
            stream.Close();
            serialThread.Abort();
        }
    }

    void OnApplicationPause() {

    }

    void ParseData() {
        while(true) {
            serialData = stream.ReadLine();
            
            string[] parsedData = serialData.Split(':');
            switch(parsedData[0]) {

                case "crank":
                    if (parsedData[1] == "up") crankUp = true;
                    if (parsedData[1] == "down") crankDown = true;
                    break;

                case "knob1":
                    if (parsedData[1] == "up") knobLeft = true;
                    if (parsedData[1] == "down") knobRight = true;
                    break;

                case "knob2":
                    if (parsedData[1] == "up") knobUp = true;
                    if (parsedData[1] == "down") knobDown = true;
                    break;

                case "button":
                    if (parsedData[1] == "on" && !buttonPressed) {
                        buttonPressed = true;
                    }
                    break;
            }

            if (parsedData.Length > 1 && (parsedData[1] == "up" || parsedData[1] == "down" || parsedData[1] == "on" )) serialReceived = true;
        }
    }
}

