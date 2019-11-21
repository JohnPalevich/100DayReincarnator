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
    public int columns = 24;
    public int rows = 10;

    //# of  all Objects
    public Count wallCount = new Count(4, 10);
    public Count pickUpCount = new Count(4, 6);

    //Objects to hold prefabs
    public GameObject[] floorTiles;
    public GameObject[] pickUpTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
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

    void LayoutObjectAtRandomPos(GameObject[] tileArr, int min, int max)
    {
        int objCount = Random.Range(min, max + 1);
        for(int i = 0; i < objCount; i++)
        {
            Vector3 randPos = RandomPosition();
            GameObject tile = tileArr[Random.Range(0, tileArr.Length)];
            Instantiate(tile, randPos, Quaternion.identity);

        }
    }

    void LayoutEnemies(GameObject[] tileArr, int min, int max)
    {
        int objCount = Random.Range(min, max + 1);
        for (int i = 0; i < objCount; i++)
        {
            Vector3 randPos = RandomPosition();
            GameObject tile = tileArr[Random.Range(0, tileArr.Length)];
            GameObject enemy = Instantiate(tile, randPos, Quaternion.identity);
        }
    }

    public void SetUpScene(int level)
    {
        BoardSetup();
        InitializeList();
        //LayoutObjectAtRandomPos(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandomPos(pickUpTiles, pickUpCount.minimum, pickUpCount.maximum);
        int numEnemies = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandomPos(enemyTiles, numEnemies, numEnemies );
    }
}
