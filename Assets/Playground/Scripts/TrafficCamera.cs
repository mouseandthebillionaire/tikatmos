using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCamera : MonoBehaviour
{
    public float speed;

    public float[] cameraBounds = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.position.y;

        if (Input.GetKeyDown(GlobalVariables.S.upCrank) && yPos + speed <= cameraBounds[1]) yPos += speed;
        if (Input.GetKeyDown(GlobalVariables.S.downCrank) && yPos - speed >= cameraBounds[0]) yPos -= speed;

        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
