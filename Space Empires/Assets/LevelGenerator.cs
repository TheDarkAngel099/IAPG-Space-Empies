using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
  int [,] level;
  public GameObject astroids;
  [SerializeField] int width;
  [SerializeField] int hight;
  [Range (0,100)]
  public int astroidDensity;
  [SerializeField] int iterations;
  [SerializeField] string seed;
   public bool enableRandSeed;
    [SerializeField] int aliveObjects;

  int[,] GenerateRandomLevel()
  {
    if (enableRandSeed)
        {
            seed = Time.time.ToString();
        }
    System.Random psudoRandSeed = new System.Random(seed.GetHashCode());
    int[,] initLevel = new int[width, hight];

    for (int x = 0; x < initLevel.GetUpperBound(0); x++)
    {
      for (int y = 0; y < initLevel.GetUpperBound(1); y++ )
      {
        if (x == 0 || x== initLevel.GetUpperBound(0) -1 || y == 0 || y == initLevel.GetUpperBound(1)-1)
        {
          initLevel[x,y] =1;
        }
        else
        {
           initLevel[x,y] = (psudoRandSeed.Next(0,100) < astroidDensity) ? 1 : 0;
        
          
        }
      }
    }
    

    return initLevel;

  }

   int GetSourroundingAstroids(int [,] level , int xGrid , int yGrid)
   {
    int count = 0;

      for (int xNeighbor = xGrid - 1; xNeighbor <= xGrid + 1; xNeighbor++)
        {
            for (int yNeighbor = yGrid -1 ; yNeighbor <= yGrid +1 ; yNeighbor ++)
            {
                if (xNeighbor >= 0 && xNeighbor < width && yNeighbor >= 0 && yNeighbor < hight)
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

  int [,] SmoothLevel(int [,] level , int iterations , int aliveThingsCondition)
  {
    for (int i = 0 ; i < iterations; i++)
    {
      for (int x = 0; x < level.GetUpperBound(0) ; x++)
      {
        for (int y = 0; y < level.GetUpperBound(1); y++)
        {
          int neighborAstroids = GetSourroundingAstroids(level , x, y);

          if (x == 0 || x == level.GetUpperBound(0) -1 || y == 0 || y == level.GetUpperBound(1) -1 )
          {
            level [x, y] = 1;
          }
          else if ( neighborAstroids > aliveThingsCondition)
          {
            level[x, y] = 1;
          }
          else if (neighborAstroids < aliveThingsCondition)
          {
            level[x,y] = 0;
          }
        }
      }
      Debug.Log("smoothing done" + i);
    }

    return level;
  }


  void GeneratePhysicalLevel(int [,] level)
  {
    foreach ( GameObject astroid in GameObject.FindGameObjectsWithTag("Space"))
    {
      Destroy(astroid);
    }

     for (int x = 0; x < level.GetUpperBound(0); ++x)
        {
            for (int y = 0; y < level.GetUpperBound(1); ++y)
            {
                if (level[x, y] == 1)
                {
                    Instantiate(astroids, new Vector3(x, 0f, y), Quaternion.Euler(Vector3.zero));
                }
            }
        }
  }

void LevelGen()
{
  level = GenerateRandomLevel();
  level = SmoothLevel( level, iterations ,aliveObjects);
  GeneratePhysicalLevel (level);

}
void Start()
{
  


}

void Update()
{
  if (Input.GetMouseButtonDown(0))
  {
    LevelGen();
  }
  if (Input.GetMouseButtonDown(1))
  {
    level = SmoothLevel( level, iterations ,aliveObjects);
    GeneratePhysicalLevel (level);
  }

}
   
}
