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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            if (grid.GetValue(mouseWorldPosition) == 0)
            {
                grid.SetValue(mouseWorldPosition, 42);
            } else
            {
                grid.SetValue(mouseWorldPosition, 0);
            }
        }
        if (Input.GetKeyDown("k"))
        {
            gameIsActive = !gameIsActive;
        }
        if (gameIsActive)
        {
            if (Time.frameCount % 300 == 0)
                ActivateGame();
        }
    }

    private void ActivateGame()
    {
        int[,] tempGridArray = new int[grid.width, grid.height];;

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                // int cellNeighbours = GetNeighbourCount(grid.gridArray, grid.width, grid.height, j, i);
                int cellNeighbours = GetNeighbourCount(grid.gridArray, grid.width, grid.height, j, i);

                tempGridArray[j, i] = 0;

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
        grid.applyGrid(tempGridArray);
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
                    if (gridArray[j + x - 1, i + y - 1] > 0)
                    {
                        neighbours += 1;
                    }
                }
            }
        }

        return (neighbours);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        vec.z = 0f;
        return (vec);
    }
    
}
