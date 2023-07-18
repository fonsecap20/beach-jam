using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject obstacle;
    public int numberToSpawn;

    //Boundaries for the area of the asteroid field
    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float bottomBoundary;

    void Start()
    {
        for(int i = 0; i < numberToSpawn; i++){
            float xSpawn = Random.Range(leftBoundary, rightBoundary);
            float ySpawn = Random.Range(bottomBoundary, topBoundary);
            Instantiate(obstacle, new Vector3(xSpawn,ySpawn,0), Quaternion.identity);
        }
    }
}
