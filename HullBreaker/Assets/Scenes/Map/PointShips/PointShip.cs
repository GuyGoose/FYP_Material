using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointShip : MonoBehaviour
{
    public string currentPointName;
    public int relations = 0;
    private Animator animator;
    private SpriteRenderer spriteColor;

    public GameObject currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spriteColor = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        // Find the current point by name and set this ship to that point
        currentPoint = GameObject.Find(currentPointName);
        // Put ship into points ship list
        currentPoint.GetComponent<PointController>().ships.Add(this);

        // TEMP - Randomize relations
        int rand = Random.Range(-100, 101);
        AdjustRelations(rand);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if current point is visited (if not hide ship + stop animating)
        if (!currentPoint.GetComponent<PointController>().visited) {
            animator.enabled = false;
            // Disable sprite renderer in child object
            spriteColor.enabled = false;
        } else {
            animator.enabled = true;
            spriteColor.enabled = true;
        }
    }

    // public void MoveToConnectedPointRandom() {
    //     currentPoint.GetComponent<PointController>().ships.Remove(this);
    //     // Get a random connected point
    //     GameObject nextPoint = currentPoint.GetComponent<PointController>().connectedPoints[Random.Range(0, currentPoint.GetComponent<PointController>().connectedPoints.Count)];
    //     // Move to that point
    //     //SlideToPoint(nextPoint);
    //     // Update current point
    //     currentPoint = nextPoint;
    //     currentPoint.GetComponent<PointController>().ships.Add(this);
    // }

    public void AdjustRelations(int x) {
        relations = relations + x;

        if (relations >= 50) {
            spriteColor.color = Color.green;
        } else if (relations <= -50) {
            spriteColor.color = Color.red;
        } else spriteColor.color = Color.white;

    }

    // TODO: Causes Error - Make a coroutine later
    // public void SlideToPoint(GameObject point) {
    //     StartCoroutine(SlideToPointCoroutine(point));
    // }

    // IEnumerator SlideToPointCoroutine(GameObject point) {
    //     float t = 0;
    //     Vector3 startPos = this.transform.position;
    //     while (t < 1) {
    //         t += Time.deltaTime * 2;
    //         this.transform.position = Vector3.Lerp(startPos, point.transform.position, t);
    //         yield return null;
    //     }
    // }
}
