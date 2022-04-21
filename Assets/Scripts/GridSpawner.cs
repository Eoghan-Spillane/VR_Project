using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;


public class GridSpawner : MonoBehaviour
{   

    public TextMeshProUGUI objectTotalSelectorText,yellowSliderText, redSliderText, totalObjectsLoaded;
    public UnityEngine.UI.Slider totalObjectSlider, yellowSlider, redSlider;
    public TextMeshProUGUI bluePercentage, yellowPercentage, redPercentage;

    private int blueRemaining, yellowRemaining, redRemaining;
    private int totalCount, yellowTotal, redTotal;
    public GameObject[] objectsToSpawn;
    public Vector3 gridOrigin = Vector3.zero;
    private int objectCount;
    public UnityEngine.UI.Toggle shuffle;

    
    public void Start() {
        totalObjectSlider.onValueChanged.AddListener(delegate {updateGUI();});
        yellowSlider.onValueChanged.AddListener(delegate {updateGUI();});
        redSlider.onValueChanged.AddListener(delegate {updateGUI();});
    }

    public void updateGUI(){
            yellowSlider.maxValue = 100 - redSlider.value;
            redSlider.maxValue = 100 - yellowSlider.value;


            yellowSliderText.text = "Yellow Slider: " + yellowSlider.value.ToString();
            redSliderText.text = "Red Slider: " + redSlider.value.ToString();

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
    }

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private int gridX, gridZ;
    private float gridSpacing = 1f;
    private int sideOfGrid;
    private int shuffleCount;
    private List<Vector3> shuffledObjects;
    private System.Random rand = new System.Random();

    public void spawnGrid(){
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

        if (shuffle.isOn){
            shuffleCount = 0;
            shuffledObjects = new List<Vector3>();
            
            for (int x = 0; x < gridX; x++){
                for(int z = 0; z < gridZ; z++){
                    shuffleCount++;
                    Vector3 spawnPosition = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOrigin;
                    shuffledObjects.Add(spawnPosition);
                }
            }

            // The Knuth-Fisher–Yates shuffle, as implemented by Durstenfeld
            // https://blog.codinghorror.com/the-danger-of-naivete/
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            // Shuffles array without have to make a copy
            for (int i = shuffledObjects.Count - 1; i > 0; i--){
                int n =  rand.Next(i + 1); //Pick random index out of remainder

                // Swap the randomly chosen one with the last in array
                var t = shuffledObjects[i];
                shuffledObjects[i] = shuffledObjects[n];
                shuffledObjects[n] = t;
            }

            // Spawn Objects but using shuffled locations in grid
            foreach(Vector3 spawnPosition in shuffledObjects){
                spawnObject(spawnPosition, Quaternion.identity);
            }
        }
        else if(!shuffle.isOn){
            for (int x = 0; x < gridX; x++){
                for(int z = 0; z < gridZ; z++){
                    Vector3 spawnPosition = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOrigin;
                    spawnObject(spawnPosition, Quaternion.identity);
                }
            }
        }
    

        totalObjectsLoaded.text = "Objects in Scene: " + spawnedObjects.Count;
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
       spawnedObjects = new List<GameObject>();
    }
}
