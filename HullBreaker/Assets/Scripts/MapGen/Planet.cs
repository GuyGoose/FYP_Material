using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Planet
{
    public GameObject planetObject;
    public Vector3 position;
    public List<Planet> connectedPlanets = new List<Planet>();
}
