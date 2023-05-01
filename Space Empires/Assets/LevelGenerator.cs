using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGenerator : MonoBehaviour
{
     [Range(0,100)]
    public int astroidDensity;
    public int hight = 256;
    public int width = 128;
   
    [SerializeField] string seed;
    public bool enableRandSeed;
    System.Random randNumberGen ;
    public GameObject astroid;
    public GameObject star;
    public GameObject planet;
    public int iterations;
    public int aliveThings = 4;
    
    
    
    int [,] level;
    



    void Start()
    {
        level = GenerateLevel();
        PhysicalLevel(level);

    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            PhysicalLevel(level);
        }
         if (Input.GetMouseButtonDown(1))
        {
            level = SmoothMap(level, aliveThings);
        }
       
    
    }

    int [,] GenerateLevel()
    {
        level = new int [hight , width];
        GenerateRandomLevel();

        for(int i = 0; i< iterations; i++)
        {
            SmoothMap( level, aliveThings);
        }
        return level;

    }


    void GenerateRandomLevel()
    {
        if (enableRandSeed)
        {
            seed = Time.time.ToString();
        }

        randNumberGen = new System.Random(seed.GetHashCode());

        for (int x = 0; x < hight; x++)
        {
            for (int y = 0; y < width; y++ )
            {
                if(x == 0 || x == level.GetUpperBound(0) -1 || x == level.GetUpperBound(0) -2 || y== 0 || y == level.GetUpperBound(1) -1 | y == level.GetUpperBound(1) -2)
                {
                    level[x,y] = 1;
                }
                else 
                {
                    level[x,y] = (randNumberGen.Next(0,100) < astroidDensity) ? 1 : 0;
                }
                
            }
        }
    }

   
    
    

    int GetSurroundingDeadCellCount(int xGrid, int yGrid)
    {
        int count = 0;
        for (int xNeighbor = xGrid - 1; xNeighbor <= xGrid + 1; xNeighbor++)
        {
            for (int yNeighbor = yGrid -1 ; yNeighbor <= yGrid +1 ; yNeighbor ++)
            {
                if (xNeighbor >= 0 && xNeighbor < hight && yNeighbor >= 0 && yNeighbor < width)
                {
                    if(xNeighbor != xGrid || yNeighbor != yGrid)
                    {
                        count+= level[xNeighbor, yNeighbor];
                    }
                }
                else
                {
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
                int neighbourThings = GetSurroundingDeadCellCount(x,y);
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
        Debug.Log("smoothing done");
        return level;
    }


    void OnDrawGizmos()
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
    }


    void PhysicalLevel(int [,] lvl)
    {
      
        foreach ( GameObject astroid in GameObject.FindGameObjectsWithTag ("Space"))
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
                    Instantiate(astroid, new Vector3(x, 0f, y), Quaternion.Euler(Vector3.zero));
                }
                if (level[centerCellX, centerCellY] == 0 && starPlaced )
                {
                    Instantiate(star, new Vector3(centerCellX, 0f, centerCellY), Quaternion.Euler(Vector3.zero));
                    starPlaced = false;
                }
                if (level[x, y] == 0 && planetsPlaced &&  centerCellX - 25 < x && x < centerCellX + 25 &&  centerCellY - 25 < y && y < centerCellY +25 && x% 35 == 0 && y% 35 == 1)
                {
                    Instantiate(planet, new Vector3(x, 0f, y), Quaternion.Euler(Vector3.zero));
                    //planetsPlaced = false;
                }
                
            }
        }

        
    }


}