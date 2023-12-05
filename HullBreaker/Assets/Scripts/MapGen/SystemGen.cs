using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGen : MonoBehaviour
{
    public GameObject planetPrefab;
    public int numberOfPlanets = 10;
    // Array of planet sizes
    public float[] planetSizes = { 1f, 3f, 5f };
    public float maxLinkDistance = 4f;

    private List<Planet> planets = new List<Planet>();
    private List<Link> links = new List<Link>();

    void Start()
    {
        GeneratePlanets();
        CreateLinks();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RecreateSystem();
        }
    }

    void GeneratePlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            float size = planetSizes[Random.Range(0, planetSizes.Length)];
            Vector3 position = new Vector3(Random.Range(-100f, 100f), Random.Range(-50f, 50f), 0f);

            GameObject planetObject = Instantiate(planetPrefab, position, Quaternion.identity);
            planetObject.transform.localScale = new Vector3(size, size, 1f);

            Planet planet = new Planet
            {
                planetObject = planetObject,
                position = position
            };

            planets.Add(planet);
        }
    }

    void CreateLinks()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            for (int j = i + 1; j < planets.Count; j++)
            {
                float distance = Vector3.Distance(planets[i].position, planets[j].position);
                if (distance <= maxLinkDistance)
                {
                    Link link = new Link
                    {
                        planetA = planets[i],
                        planetB = planets[j]
                    };

                    planets[i].connectedPlanets.Add(planets[j]);
                    planets[j].connectedPlanets.Add(planets[i]);

                    GameObject lineObject = new GameObject("Link");
                    LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, planets[i].position);
                    lineRenderer.SetPosition(1, planets[j].position);
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;

                    link.linkObject = lineObject;

                    links.Add(link);
                }
            }

            // Ensure each planet connects to at least one other planet
            if (planets[i].connectedPlanets.Count == 0)
            {
                int randomIndex = Random.Range(0, planets.Count);
                while (randomIndex == i)
                {
                    randomIndex = Random.Range(0, planets.Count);
                }

                Link link = new Link
                {
                    planetA = planets[i],
                    planetB = planets[randomIndex]
                };

                planets[i].connectedPlanets.Add(planets[randomIndex]);
                planets[randomIndex].connectedPlanets.Add(planets[i]);

                GameObject lineObject = new GameObject("Link");
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, planets[i].position);
                lineRenderer.SetPosition(1, planets[randomIndex].position);
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;

                link.linkObject = lineObject;

                links.Add(link);
            }
        }
    }

    void RecreateSystem()
    {
        foreach (Planet planet in planets)
        {
            Destroy(planet.planetObject);
        }
        planets.Clear();

        foreach (Link link in links)
        {
            Destroy(link.linkObject);
        }
        links.Clear();

        GeneratePlanets();
        CreateLinks();
    }
}
