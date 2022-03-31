using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GridSpawner : MonoBehaviour
{
    public TextMeshProUGUI objectCounter;
    // Update is called once per frame
    private float pollingTime = 1f;
    private float time;
    private int objectCount;

    void Update(){
        // Count the objects
        time += Time.deltaTime;

        // If it's been 1 second, update total object text count
        if(time >= pollingTime) {
            objectCounter.text = "Total Objects: " + objectCount.ToString();
            time -= pollingTime;
        }
    }

    public GameObject[] objectsToSpawn;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    public int gridX;
    public int gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;
    public void spawnGrid(){
        for (int x = 0; x < gridX; x++){
            for(int z = 0; z < gridZ; z++){
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                spawnObject(spawnPosition, Quaternion.identity);
            }
        }
    }

    void spawnObject(Vector3 positionToSpawn, Quaternion rotationToSpawn){
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        
        GameObject clone = Instantiate(objectsToSpawn[randomIndex], positionToSpawn, rotationToSpawn);
        objectCount++;

        spawnedObjects.Add(clone);
    }

    public void despawn(){
       foreach(var objectete in spawnedObjects){
           Destroy(objectete);
       } 

       objectCount = 0;
    }
}
