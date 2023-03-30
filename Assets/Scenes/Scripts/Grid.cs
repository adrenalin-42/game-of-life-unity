using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int width {get; set; }
    public int height {get; set; }
    private float cellSize;
    private Vector3 originPosition;
    public int[,] gridArray {get; set; }
    public TextMesh[,] debugTextArray {get; set; }
    private Transform gridTransform;
    private GameObject gridGameObject;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        this.gridGameObject = new GameObject("Grid");
        gridTransform = gridGameObject.transform;

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                GameObject gameObject = new GameObject("cell_" + i + "_" + j, typeof(TextMesh));
                Transform transform = gameObject.transform;
                transform.SetParent(gridTransform, false);
                // gameObject.
                transform.localPosition = GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f;
                // TextMesh textMesh = gameObject.GetComponent<TextMesh>();
                // debugTextArray[i, j] = textMesh;
                // textMesh.anchor = TextAnchor.MiddleCenter;
                // textMesh.alignment = TextAlignment.Left;
                // textMesh.text = i + ", " + j;
                // textMesh.fontSize = 20;
                // textMesh.color = Color.white;
                // textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;

                DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, -1f);
                DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 0f);
            }
        }
        DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 0f);
        DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 0f);

        // SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x, y) * cellSize + originPosition);
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (value > 0)
            {
                DrawRectangle(x, y, true);
            } else
            {
                RemoveRectangle(x, y);
            }
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;

        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return (gridArray[x, y]);
        } else
        {
            return (-1);
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return (GetValue(x, y)); 
    }

    public Vector3 GetDimensions()
    {
        return (new Vector3(width * cellSize / 2, height * cellSize / 2, -1));
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject("Line");
        myLine.transform.position = start;
        myLine.transform.SetParent(gridTransform, false);
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        Material temp_mat = new Material(Shader.Find("Unlit/Texture"));
        if (temp_mat)
        {
            lr.material = temp_mat;
        }
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private void DrawRectangle(int x, int y, bool active = true)
    {

        if (GameObject.Find("rect_" + x + "_" + y) != null)
            return;

        GameObject originalGO = GameObject.Find("SquareRef");
        GameObject myGO = new GameObject("rect_" + x + "_" + y);

        // SpriteRenderer gmSR = gm.GetComponent<SpriteRenderer>();
        if (originalGO != null)
        {
            myGO.AddComponent<SpriteRenderer>();
            myGO.GetComponent<SpriteRenderer>().sprite = originalGO.GetComponent<SpriteRenderer>().sprite;
            myGO.GetComponent<SpriteRenderer>().color = Color.blue;
            myGO.transform.position = new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2);
            myGO.transform.localScale = new Vector3(cellSize, cellSize) - new Vector3(0.5f, 0.5f);
        }
    }

    private void RemoveRectangle(int x, int y)
    {

        GameObject myGO = GameObject.Find("rect_" + x + "_" + y);

        if (myGO != null)
        {
            GameObject.Destroy(myGO);
        }        
    }

    public void GridRemove()
    {
        GameObject.Destroy(this.gridGameObject);
    }

    public void applyGrid(int [,] newGrid)
    {
        // this.gridArray = newGrid;
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                SetValue(i, j, newGrid[i, j]);
            }
        }
    }
}
