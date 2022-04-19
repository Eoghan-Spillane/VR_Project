using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GridSpawner : MonoBehaviour
{   

    public TextMeshProUGUI objectTotalSelectorText,yellowSliderText, redSliderText;
    public UnityEngine.UI.Slider totalObjectSlider, yellowSlider, redSlider;
    public TextMeshProUGUI bluePercentage, yellowPercentage, redPercentage;

    private int blueRemaining, yellowRemaining, redRemaining;
    private int totalCount, yellowTotal, redTotal;
    public GameObject[] objectsToSpawn;
    public Vector3 gridOrigin = Vector3.zero;
    private float pollingTime = 1f;
    private float time;
    private int objectCount;

    void Update(){
        // Count the objects
        time += Time.deltaTime;

        // If it's been 1 second, update total object text count
        if(time >= pollingTime) {
            //Slider Positons
            yellowSliderText.text = "Yellow Slider: " + yellowSlider.value.ToString();
            redSliderText.text = "Yellow Slider: " + redSlider.value.ToString();

            //Total of each type
            if ((int)totalObjectSlider.value <= 1000){
                totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 10) * 10;
                yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
                redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );
            }
            else if ((int)totalObjectSlider.value <= 10000){
                totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 500) * 500;
                yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
                redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );
            }
            else {
                totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 2500) * 2500;
                yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
                redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );
            }

            blueRemaining = totalCount - (yellowTotal + redTotal);
            bluePercentage.text = blueRemaining + " (" + Mathf.RoundToInt( (blueRemaining * 100) / totalCount) +"%)";
            yellowPercentage.text = yellowTotal + " (" + Mathf.RoundToInt( (yellowTotal * 100) / totalCount) +"%)";
            redPercentage.text = redTotal + " (" + Mathf.RoundToInt( (redTotal * 100) / totalCount) +"%)"; 
            objectTotalSelectorText.text = "Total Object Slider (Blue by default): " + totalCount;

            time -= pollingTime;
        }
    }

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private int gridX, gridZ;
    private float gridSpacing = 1f;
    private int sideOfGrid;

    public void spawnGrid(){
        Debug.Log("Spawn Grid");
        Debug.Log(totalObjectSlider.value.ToString() + " / " + yellowSlider.value.ToString() + " / " + redSlider.value.ToString());

        if (totalObjectSlider.value <= 1000){
            totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 10) * 10;

            yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
            redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );

            sideOfGrid = Mathf.RoundToInt( Mathf.Sqrt(totalCount) );
            gridX = gridZ = sideOfGrid;
        }
        else if (totalObjectSlider.value <= 10000){
            totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 500) * 500;

            yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
            redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );

            sideOfGrid = Mathf.RoundToInt( Mathf.Sqrt(totalCount) );
            gridX = gridZ = sideOfGrid;
        }
        else {
            totalCount = Mathf.FloorToInt(((int)totalObjectSlider.value) / 2500) * 2500;

            yellowTotal = Mathf.RoundToInt( (yellowSlider.value / 100) * totalCount );
            redTotal = Mathf.RoundToInt( (redSlider.value / 100) * totalCount );

            sideOfGrid = Mathf.RoundToInt( Mathf.Sqrt(totalCount) );
            gridX = gridZ = sideOfGrid;
        }

        // Clear Objects First
        despawn();

        for (int x = 0; x < gridX; x++){
            for(int z = 0; z < gridZ; z++){
                Vector3 spawnPosition = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOrigin;
                spawnObject(spawnPosition, Quaternion.identity);
            }
        }
    }

    void spawnObject(Vector3 positionToSpawn, Quaternion rotationToSpawn){
        if (redTotal != 0){
            GameObject clone = Instantiate(objectsToSpawn[2], positionToSpawn, rotationToSpawn);
            redTotal = redTotal - 1;
            objectCount++;
            spawnedObjects.Add(clone);
        }
        else if (yellowTotal != 0){
            GameObject clone = Instantiate(objectsToSpawn[1], positionToSpawn, rotationToSpawn);
            yellowTotal = yellowTotal - 1;
            objectCount++;
            spawnedObjects.Add(clone);
        }
        else{
            GameObject clone = Instantiate(objectsToSpawn[0], positionToSpawn, rotationToSpawn);
            objectCount++;
            spawnedObjects.Add(clone);
        }
    }

    public void despawn(){
       foreach(var objectete in spawnedObjects){
           Destroy(objectete);
       } 
       objectCount = 0;
    }
}
