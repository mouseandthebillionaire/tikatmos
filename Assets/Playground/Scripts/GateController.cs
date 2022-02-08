using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateController : MonoBehaviour
{
    public GameObject[] gates;

    public GameObject[] zones;

    public Text[] zoneCounters;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Count the number of people inside each zone
        for (int i = 0; i < zones.Length; i++) {

            // Obtains the center and size of the zone
            float x = zones[i].transform.position.x;
            float y = zones[i].transform.position.y;
            float sizeX = zones[i].GetComponent<SpriteRenderer>().bounds.extents.x*2;
            float sizeY = zones[i].GetComponent<SpriteRenderer>().bounds.extents.y*2;

            Collider2D[] colliders;
            colliders = Physics2D.OverlapBoxAll(new Vector2(x,y), new Vector2(sizeX, sizeY), 0);

            int people = 0;
            for (int j = 0; j < colliders.Length; j++) {
                if (colliders[j].gameObject.layer == 6) people++;
            }
            zoneCounters[i].text = "" + people;
        }
    }
}
