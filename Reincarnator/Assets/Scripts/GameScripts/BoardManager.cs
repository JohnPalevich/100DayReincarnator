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
    public int columns = 36;
    public int rows = 15;

    //# of  all Objects
    public Count wallCount = new Count(4, 10);
    public Count pickUpCount = new Count(4, 6);

    //Objects to hold prefabs
    public GameObject[] floorTiles;
    public GameObject[] pickUpTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject pit;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    //Creates a list of possible board positions for the coming objects to be placed.
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

    //Sets up the outside wall ring and the inner floor tiles.
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

    //Chooses a random position for the object to be spawned
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randPos = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randPos;
    }

    //Lays out X amount of objects from a specific type of array.
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

    //Almost same as LayoutObject at Random Pos, however specifically for enemies.
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

    //Lays out "pits" in rectangular form.
    void LayoutPits(GameObject tile)
    {
        int pitCount = Random.Range(2,4);
        for (int i = 0; i < pitCount; i++)
        {
            int width = Random.Range(2,4);
            int height = Random.Range(1,3);
            Vector3 randPos = RandomPosition();
            gridPositions.Add(randPos);
            for(int j = 0; j < width; j++)
            {
                for(int k = 0; k < height; k++)
                {
                    if ((randPos.x + j < (columns - 3) && randPos.y + k > (rows - 3)) || randPos.y > 3 || randPos.x > 3) {
                        Vector3 pos = new Vector3(randPos.x + j, randPos.y + k, 0);
                        int index = gridPositions.IndexOf(pos);
                        if (index > 0)
                        {
                            gridPositions.RemoveAt(index);
                            GameObject pitTile = Instantiate(tile, pos, Quaternion.identity);
                            pitTile.transform.SetParent(boardHolder);
                        }
                    }
                }
            }
            
        }
    }

    //Code which sets up the level, some characteristics will be determined by level number.
    public void SetUpScene(int level)
    {
        BoardSetup();
        InitializeList();
        int index = gridPositions.IndexOf(new Vector3(12,5,0));
        gridPositions.RemoveAt(index);
        LayoutPits(pit);
        LayoutObjectAtRandomPos(pickUpTiles, pickUpCount.minimum, pickUpCount.maximum);
        //int numEnemies = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandomPos(enemyTiles, 2, 2);
    }
}
