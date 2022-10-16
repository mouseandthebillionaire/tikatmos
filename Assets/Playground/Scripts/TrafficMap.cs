using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficMap : MonoBehaviour
{
    public GameObject floor;
    public int numberOfFloors;
    public float xBounds, yBounds;

    public float spawnRadius, spawnTolerance;

    private GameObject[] floors;

    private Vector3 spawnLocation;

    private bool[] floorSpawned;
    
    // Start is called before the first frame update
    void Start()
    {
        floors = new GameObject[numberOfFloors];

        // Spawn the first node at a random location
        spawnLocation = new Vector3(Random.Range(-xBounds, xBounds), Random.Range(-yBounds, yBounds), 0);
        floors[0] = floor;
        Instantiate(floors[0], spawnLocation, Quaternion.identity);

        for (int i = 1; i < numberOfFloors; i++) StartCoroutine(SpawnFloor(spawnLocation, i));
        //floor.SetActive(false);
    }

    IEnumerator SpawnFloor(Vector3 location, int index) {
        yield return new WaitForSeconds(0);

        // Generate the location of the next node
        float x = location.x;
        float y = location.y;

        float randomTheta = Random.Range(0, 2*Mathf.PI);

        float xDiff = Mathf.Cos(randomTheta) * spawnRadius;
        float yDiff = Mathf.Sin(randomTheta) * spawnRadius;

        if (CheckOverlap(x + xDiff, y + yDiff, index)) StartCoroutine(SpawnFloor(location, index));
        else {
            // Spawn the next node
            spawnLocation = new Vector3(x + xDiff, y + yDiff, 0);
            floors[index] = floor;
            Instantiate(floors[index], spawnLocation, Quaternion.identity);
        }
    }

    public bool CheckOverlap(float x, float y, int index) {
        float floorExtent = Mathf.Abs(floor.transform.position.x - floor.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x) + spawnTolerance;

        float xMin = x - floorExtent;
        float xMax = x + floorExtent;

        float yMin = y - floorExtent;
        float yMax = y + floorExtent;

        // If the floor will go outside the bounds
        if (xMin <= -xBounds || xMax >= xBounds) return true;
        if (yMin <= -yBounds || yMax >= yBounds) return true;

        // If the floor overlaps with another floor
        for (int i = 0; i < index; i++) {
            if (floors[i] != null) {
                if (floors[i].transform.position.x >= xMin && floors[i].transform.position.x <= xMax
                && floors[i].transform.position.y >= yMin && floors[i].transform.position.y <= yMax) return true;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
