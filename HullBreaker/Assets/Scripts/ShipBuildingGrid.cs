using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuildingGrid : MonoBehaviour
{
    
    public GameObject gridPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public int gridSpacing = 100;

    // Start is called before the first frame update
    void Start()
    {
        // Create a 2D array of GameObjects
        GameObject[,] grid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                // Instantiate a grid element
                GameObject gridElement = Instantiate(gridPrefab, transform);

                // Set the position of the grid element centered on the parent
                gridElement.transform.localPosition = new Vector3(x * gridSpacing, y * gridSpacing, 0);

                // Set the name of the grid element
                gridElement.name = "Grid Element " + x + ", " + y;

                // Add the grid element to the 2D array
                grid[x, y] = gridElement;

                // Set the grid element's parent to this object
                gridElement.transform.SetParent(transform);

                // Set the grid element's boolean to false
                gridElement.GetComponent<GridElement>().SetOccupied(false);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
