using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    //Level Dimensions
    public int columns = 23;
    public int rows = 9;

    //# of  all Objects
    public Count wallCount = new Count(4, 10);
    public Count coinCount = new Count(4, 6);

    //Objects to hold prefabs
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemies;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();
        
        for(int x = 1; x < columns - 1; x++)
        {
            for(int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for(int x = -1; x < columns+1; x++)
        {
            for(int y = -1; y < rows+1; y++)
            {
                GameObject toInstantiate;
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randPos = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randPos;
    }

    //10:30 in vid: https://learn.unity.com/tutorial/level-generation?projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f6

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
