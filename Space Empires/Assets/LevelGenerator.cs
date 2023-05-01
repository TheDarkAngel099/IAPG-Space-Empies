using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGenerator : MonoBehaviour
{
     [Range(0,100)]
    public int astroidDensity; // Density of asteroids in the generated level, adjustable in the editor
    public int hight = 256; // Height of the generated level
    public int width = 128; // Width of the generated level

    [SerializeField] string seed; // Seed for the random number generator, can be set in the editor
    public bool enableRandSeed; // If true, a random seed is generated based on the current time
    System.Random randNumberGen; // Random number generator used for generating the level
    public GameObject astroid;// Prefab for asteroids to be instantiated
    public GameObject star;// Prefab for the star to be instantiated
    public GameObject planet; // Prefab for planets to be instantiated
    public int iterations; // Number of smoothing iterations to perform on the generated level
    public int aliveThings = 4; // Condition variable for the smoothing algorithm



    int [,] level;  // 2D array representing the generated level




    void Start()
    {
        level = GenerateLevel(); //Generate level
        PhysicalLevel(level); //place walls in the scene

    }
    void Update()
    {
        // Debug Stuff
        //if (Input.GetMouseButtonDown(0))
        //{
            //PhysicalLevel(level);
        //}
         //if (Input.GetMouseButtonDown(1))
        //{
            //level = SmoothMap(level, aliveThings);
        //}
       
    
    }

    // Function to Generate a new level which returns a 2D array
    int[,] GenerateLevel()
    {
        level = new int [hight , width]; // Initialize the level array
        GenerateRandomLevel(); //Generate a random level

        // Smooth the level for given iterations
        for (int i = 0; i< iterations; i++)
        {
            SmoothMap( level, aliveThings);
        }
        return level; // Return the generated level

    }

    // Generate a random level
    void GenerateRandomLevel() 
    {
        // Generate a random seed if random seed is enabled
        if (enableRandSeed)
        {
            seed = Time.time.ToString();
        }
        // Initialize the random number generator
        randNumberGen = new System.Random(seed.GetHashCode());

        for (int x = 0; x < hight; x++)
        {
            for (int y = 0; y < width; y++ )
            {
                // Set cells on the border to be walls
                if (x == 0 || x == level.GetUpperBound(0) -1 || x == level.GetUpperBound(0) -2 || y== 0 || y == level.GetUpperBound(1) -1 | y == level.GetUpperBound(1) -2)
                {
                    level[x,y] = 1;
                }
                else 
                {
                    // Randomly set cells to be walls
                    level[x,y] = (randNumberGen.Next(0,100) < astroidDensity) ? 1 : 0;
                }
                
            }
        }
    }




    // Count the number of walls surrounding a cell
    int GetSurroundingDeadCellCount(int xGrid, int yGrid)
    {
        int count = 0;
        // Loop through every neighboring cell
        for (int xNeighbor = xGrid - 1; xNeighbor <= xGrid + 1; xNeighbor++)
        {
            for (int yNeighbor = yGrid -1 ; yNeighbor <= yGrid +1 ; yNeighbor ++)
            {
                // Check if the neighboring cell is within the level
                if (xNeighbor >= 0 && xNeighbor < hight && yNeighbor >= 0 && yNeighbor < width)
                {
                    // Check if the neighboring cell is not the center cell
                    if (xNeighbor != xGrid || yNeighbor != yGrid)
                    {
                        // If the neighboring cell is not alive, increment the count
                        count += level[xNeighbor, yNeighbor];
                    }
                }
                else
                {
                    // If the neighboring cell is outside the level, consider it dead
                    
                    count++;
                }
            }
        }
        return count;
    }

    int [,] SmoothMap( int [,]level ,int aliveThings )
    {
        for (int x = 0; x < hight; x++)
        {
            for (int y = 0; y < width; y++ )
            {
                int neighbourThings = GetSurroundingDeadCellCount(x,y); // Get the number of neighboring dead cells
                // Apply the smoothing rule based on the number of dead neighbors
                if (neighbourThings > aliveThings ) //condition variables
                {
                    level[x,y] = 1;
                }
                else if (neighbourThings < aliveThings)
                {
                    level[x,y] = 0;
                }
            }
        }
        // Print a message indicating that the smoothing is done
        //Debug.Log("smoothing done");
        return level;
    }

   


    void PhysicalLevel(int [,] lvl)
    {
        // Remove any existing objects with the "Astroid" tag
        foreach ( GameObject astroid in GameObject.FindGameObjectsWithTag ("Asteroid"))
        {
            Destroy(astroid);
        }

        

        int [,] level = lvl;
        bool starPlaced = true;
        bool planetsPlaced = true; 
        int centerCellX = hight/2;
        int centerCellY = width / 2;
        level[centerCellX, centerCellY] = 0;
       

        
        for (int x = 0; x < level.GetUpperBound(0); ++x)
        {
            for (int y = 0; y < level.GetUpperBound(1); ++y)
            {
                
                if (level[x, y] == 1)
                {
                    // Instantiate an asteroid where the indices are 1
                    Instantiate(astroid, new Vector3(x, 0f, y), Quaternion.Euler(Vector3.zero));
                }
                //place a star at the center of the level
                if (level[centerCellX, centerCellY] == 0 && starPlaced )
                {
                    Instantiate(star, new Vector3(centerCellX, 0f, centerCellY), Quaternion.Euler(Vector3.zero));
                    starPlaced = false;
                }
                //place planets around the center / star
                if (level[x, y] == 0 && planetsPlaced &&  centerCellX - 25 < x && x < centerCellX + 25 &&  centerCellY - 25 < y && y < centerCellY +25 && x% 35 == 0 && y% 35 == 1)
                {
                    Instantiate(planet, new Vector3(x, 0f, y), Quaternion.Euler(Vector3.zero));
                    //planetsPlaced = false; //uncomment this line if you want to place only one planet
                }
                
            }
        }

        
    }

    //draw level on gizmos for testing as generating physical levels take too much computational power so test variable values on gizmos
    /*void OnDrawGizmos()
    {
       if  (level != null) 
       {
        for (int x = 0; x < hight ; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                Gizmos.color = (level[x,y] == 1)?Color.black:Color.white;
					Vector3 pos = new Vector3(-hight/2 + x + .5f,0, -width/2 + y+.5f);
					Gizmos.DrawCube(pos,Vector3.one);
            }
        }
       }
    }*/


}