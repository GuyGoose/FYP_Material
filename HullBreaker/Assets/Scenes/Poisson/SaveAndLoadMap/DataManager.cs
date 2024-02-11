using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public GameObject pointPrefab;
    public GameObject pointShipPrefab;
    public MapGenerationController mapController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveMap();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadMap();
        }
    }

    // Save the map generation state
    public void SaveMap()
    {
        // Create a data structure to hold all the necessary information
        MapSaveData mapSaveData = new MapSaveData();

        // Serialize MapGenerationController
        mapSaveData.mapGenerationControllerData = JsonUtility.ToJson(mapController);

        // Serialize PointController data and positions
        foreach (Transform point in mapController.pointContainer.transform)
        {
            PointController pointController = point.GetComponent<PointController>();
            PointSaveData pointSaveData = new PointSaveData();
            pointSaveData.type = pointController.type;
            pointSaveData.planetName = pointController.planetName;
            pointSaveData.visited = pointController.visited;
            pointSaveData.landed = pointController.landed;
            pointSaveData.current = pointController.current;
            pointSaveData.isSelected = pointController.isSelected;
            pointSaveData.isCompleted = pointController.isCompleted;
            pointSaveData.position = point.position;
            pointSaveData.connectedPoints = pointController.connectedPoints;
            mapSaveData.pointSaveDataList.Add(pointSaveData);
        }

        // Serialize PointShip data
        foreach (Transform ship in mapController.pointShipContainer.transform)
        {
            PointShip pointShip = ship.GetComponent<PointShip>();
            PointShipSaveData pointShipSaveData = new PointShipSaveData();
            pointShipSaveData.currentPointName = pointShip.currentPoint.name;
            pointShipSaveData.relations = pointShip.relations;
            mapSaveData.pointShipSaveDataList.Add(pointShipSaveData);
        }

        // Convert mapSaveData to JSON
        string jsonData = JsonUtility.ToJson(mapSaveData);

        // Save JSON to a file
        string filePath = Application.persistentDataPath + "/mapSave.json";
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Map saved to: " + filePath);
    }

    // Load the map generation state
    public void LoadMap()
    {
        string filePath = Application.persistentDataPath + "/mapSave.json";
        if (File.Exists(filePath))
        {
            // Read JSON from file
            string jsonData = File.ReadAllText(filePath);

            // Deserialize JSON to mapSaveData
            MapSaveData mapSaveData = JsonUtility.FromJson<MapSaveData>(jsonData);

            // Deserialize MapGenerationController
            JsonUtility.FromJsonOverwrite(mapSaveData.mapGenerationControllerData, mapController);

            // Clear existing points and ships
            foreach (Transform point in mapController.pointContainer.transform)
            {
                Destroy(point.gameObject);
            }
            foreach (Transform ship in mapController.pointShipContainer.transform)
            {
                Destroy(ship.gameObject);
            }

            // Instantiate points and ships from saved data
            foreach (PointSaveData pointSaveData in mapSaveData.pointSaveDataList)
            {
                GameObject newPoint = Instantiate(pointPrefab, pointSaveData.position, Quaternion.identity);
                newPoint.transform.parent = mapController.pointContainer.transform;
                PointController pointController = newPoint.GetComponent<PointController>();
                pointController.type = pointSaveData.type;
                pointController.planetName = pointSaveData.planetName;
                pointController.visited = pointSaveData.visited;
                pointController.landed = pointSaveData.landed;
                pointController.current = pointSaveData.current;
                pointController.isSelected = pointSaveData.isSelected;
                pointController.isCompleted = pointSaveData.isCompleted;
                pointController.connectedPoints = pointSaveData.connectedPoints;
            }

            // Instantiate ships from saved data
            foreach (PointShipSaveData pointShipSaveData in mapSaveData.pointShipSaveDataList)
            {
                GameObject currentPoint = GameObject.Find(pointShipSaveData.currentPointName);
                GameObject newShip = Instantiate(pointShipPrefab, currentPoint.transform.position, Quaternion.identity);
                newShip.transform.parent = mapController.pointShipContainer.transform;
                PointShip pointShip = newShip.GetComponent<PointShip>();
                pointShip.currentPoint = currentPoint;
                pointShip.relations = pointShipSaveData.relations;
            }

            Debug.Log("Map loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No saved map found.");
        }
    }

    // Data structure to hold map save data
    [System.Serializable]
    private class MapSaveData
    {
        public string mapGenerationControllerData;
        public List<PointSaveData> pointSaveDataList = new List<PointSaveData>();
        public List<PointShipSaveData> pointShipSaveDataList = new List<PointShipSaveData>();
    }

    // Data structure to hold PointController save data
    [System.Serializable]
    private class PointSaveData
    {
        public string type;
        public string planetName;
        public bool visited;
        public bool landed;
        public bool current;
        public bool isSelected;
        public bool isCompleted;
        public Vector3 position;
        public List<string> connectedPoints = new List<string>();
    }

    // Data structure to hold PointShip save data
    [System.Serializable]
    private class PointShipSaveData
    {
        public string currentPointName;
        public int relations;
    }
}