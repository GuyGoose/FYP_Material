using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationController : MonoBehaviour
{
    public int pointRadius = 5;
    public int regionSize = 100;
    public enum DestinationType {
        Start,
        Empty,
        Encounter,
        Event,
        Shop,
        Boss
    }
    public GameObject pointPrefab;

    // Statistics for point generation
    public float emptyChance = 0.4f;
    public float encounterChance = 0.3f;
    public float eventChance = 0.2f;
    public float shopChance = 0.1f;

    public int minShops = 3;
    public int maxShops = 5;	

    List<Vector2> points;

    // Start is called before the first frame update
    void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(pointRadius, new Vector2(regionSize, regionSize), 30);

        // Generate points 
        // Rules for generation:
        // 1. Start point is always the first point
        // 2. Boss point is always the last point
        // 3. The number of shops is random should be between minShops and maxShops

        foreach (Vector2 point in points) {
            GameObject pointObject = Instantiate(pointPrefab, point, Quaternion.identity);
            pointObject.transform.parent = this.transform;
            if (point == points[0]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Start.ToString();
            } else if (point == points[points.Count - 1]) {
                pointObject.GetComponent<PointController>().type = DestinationType.Boss.ToString();
            } else {
                float random = Random.value;
                if (random < emptyChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Empty.ToString();
                } else if (random < emptyChance + encounterChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Encounter.ToString();
                } else if (random < emptyChance + encounterChance + eventChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Event.ToString();
                } else if (random < emptyChance + encounterChance + eventChance + shopChance) {
                    pointObject.GetComponent<PointController>().type = DestinationType.Shop.ToString();
                } else {
                    pointObject.GetComponent<PointController>().type = DestinationType.Empty.ToString();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
