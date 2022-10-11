using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour {
    private float     xPos;
    public float      paddleSpeed = .05f;
    public float      leftWall, rightWall;

    public KeyCode    leftPaddle, rightPaddle;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(leftPaddle)) {
            if (xPos > leftWall) {
                xPos -= paddleSpeed;
            }
        }

        if (Input.GetKey(rightPaddle)) {
            if (xPos < rightWall) {
                xPos += paddleSpeed;
            }
        }

        transform.localPosition = new Vector3(xPos, transform.position.y, 0);
    }
}

