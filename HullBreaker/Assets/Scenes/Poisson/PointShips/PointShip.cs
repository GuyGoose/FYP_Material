using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointShip : MonoBehaviour
{
    public GameObject currentPoint;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        // Place ship at current point
        this.transform.position = currentPoint.transform.position;
        // Put ship into points ship list
        currentPoint.GetComponent<PointController>().ships.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if current point is visited (if not hide ship + stop animating)
        if (!currentPoint.GetComponent<PointController>().visited) {
            animator.enabled = false;
            // Disable sprite renderer in child object
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        } else {
            animator.enabled = true;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void MoveToConnectedPointRandom() {
        currentPoint.GetComponent<PointController>().ships.Remove(this);
        // Get a random connected point
        GameObject nextPoint = currentPoint.GetComponent<PointController>().connectedPoints[Random.Range(0, currentPoint.GetComponent<PointController>().connectedPoints.Count)];
        // Move to that point
        this.transform.position = nextPoint.transform.position;
        // Update current point
        currentPoint = nextPoint;
        currentPoint.GetComponent<PointController>().ships.Add(this);
    }
}
