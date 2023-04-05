using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;
    bool gameIsActive;
    void Start()
    {
        grid = new Grid(28, 20, 5f, new Vector3(0, 0, 0));
        Camera.main.transform.position = grid.GetDimensions();
        gameIsActive = false;
        GenerateRandomIsland();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            if(grid.GetValue(mouseWorldPosition) != 666)
            { 
            if (grid.GetValue(mouseWorldPosition) == 0 )
            {
                grid.SetValue(mouseWorldPosition, 1, Color.yellow);
            } else
            {
                grid.SetValue(mouseWorldPosition, 0, Color.clear);
            }
            }
        }
        if (Input.GetKeyDown("k"))
        {
            
            gameIsActive = !gameIsActive;
        }
        if (gameIsActive)
        {
            if (Time.frameCount % 60 == 0)
                ActivateGame();
        }
    }

    private void ActivateGame()
    {

        int[,] tempGridArray = new int[grid.width, grid.height];

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                if (grid.gridArray[j, i] == 666)
                {
                    tempGridArray[j, i] = 666;
                    continue;
                }
                // int cellNeighbours = GetNeighbourCount(grid.gridArray, grid.width, grid.height, j, i);
                int cellNeighbours = GetNeighbourCount(grid.gridArray, grid.width, grid.height, j, i);

                //tempGridArray[j, i] = 0;

                // Rule 1
                // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                if (cellNeighbours < 2 && grid.gridArray[j, i] > 0)
                {
                    tempGridArray[j, i] = 0;
                }

                // Rule 2
                // Any live cell with two or three live neighbours lives on to the next generation.
                if ((cellNeighbours == 2 || cellNeighbours == 3) && grid.gridArray[j, i] > 0)
                {
                    tempGridArray[j, i] = 1;
                }

                // Rule 3
                // Any live cell with more than three live neighbours dies, as if by overpopulation.
                if (cellNeighbours > 3 && grid.gridArray[j, i] > 0)
                {
                    tempGridArray[j, i] = 0;
                }

                // Rule 4
                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                if (cellNeighbours == 3 && grid.gridArray[j, i] == 0)
                {
                    tempGridArray[j, i] = 1;
                }
                // grid.debugTextArray[j, i].text = j + ", " + i + ", " + cellNeighbours + ", " + grid.gridArray[j, i];
                // Debug.Log("j = " + j + " i = " + i + "cellNeigh = " + cellNeighbours);
            
            }

        }
        // grid = tempGrid;
        grid.applyGrid(tempGridArray, Color.red);
    }




    private int GetNeighbourCount(int [,] gridArray, int gridWidth, int gridHeight, int x, int y)
    {
        int neighbours = 0;

        for (int i = 0; i < 3; i++) // y
        {
            for (int j = 0; j < 3; j++) // x
            {
                if (i == 1 && j == 1)
                    continue;
                if (i + y - 1 >= 0 && i + y - 1 < gridHeight && j + x - 1 >= 0 && j + x - 1 < gridWidth)
                {
                    if (gridArray[j + x - 1, i + y - 1] > 0 && gridArray[j + x - 1, i + y - 1] != 666)
                    {
                        neighbours += 1;
                    }
                }
            }
        }

        return (neighbours);
    }

    private void GenerateRandomIsland()
    {
        int[,] tempGridArray = new int[grid.width, grid.height];

        
        for (int k = 0; k < 3; k++)
        {
            int centerX = Random.Range(0, grid.width);
            int centerY = Random.Range(0, grid.height);
            int radius = Random.Range(2, 5);

            for (int i = 0; i < grid.height; i++)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    // Calculate the distance between the current cell and the center point
                    float distance = Mathf.Sqrt((j - centerX) * (j - centerX) + (i - centerY) * (i - centerY));

                    // If the distance is within the radius, color the cell blue
                    if (distance <= radius)
                    {
                        tempGridArray[j, i] = 666;
                    }
                }
            }
        }
        grid.applyGrid(tempGridArray, Color.green);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        vec.z = 0f;
        return (vec);
    }
    
}
