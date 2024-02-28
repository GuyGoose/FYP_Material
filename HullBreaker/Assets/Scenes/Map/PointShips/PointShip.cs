using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointShip : MonoBehaviour
{
    public string currentPointName;
    public int relations = 0;
    private Animator animator;
    private SpriteRenderer spriteColor;

    // public GameObject currentPoint;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spriteColor = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Awake() {
        //SetPositionToCurrentPoint();
    }

    public void OnFirstLoad() {
        // TEMP - Randomize relations
        int rand = Random.Range(-100, 101);
        relations = rand;
        //AdjustRelations(rand); // Causes error
        SetPositionToCurrentPoint();
    }

    public IEnumerator OnReload() {
        yield return new WaitForNextFrameUnit();
        SetPositionToCurrentPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if current point is visited (if not hide ship + stop animating)
        // if (currentPoint != null) {
        //     if (!currentPoint.GetComponent<PointController>().visited) {
        //         animator.enabled = false;
        //         // Disable sprite renderer in child object
        //         spriteColor.enabled = false;
        //     } else {
        //         animator.enabled = true;
        //         spriteColor.enabled = true;
        //     }
        // }
    }

    public void SetRelations(int x) {
        relations = x;
    }

    public void AdjustRelations(int x) {
        relations = relations + x;

        if (relations >= 50) {
            spriteColor.color = Color.green;
        } else if (relations <= -50) {
            spriteColor.color = Color.red;
        } else spriteColor.color = Color.white;

    }

    public void SetPositionToCurrentPoint() {
        // Find the current point by name and set the ship's position to it
        GameObject currentPoint = GameObject.Find(currentPointName);
        if (currentPoint != null) {
            this.transform.position = currentPoint.transform.position;
            currentPoint.GetComponent<PointController>().AddShip(this.gameObject);
        }
        // Add this ship to the current point's list of ships
        // Debug.Log("Current Point: " + currentPoint.GetComponent<PointController>().ships);
    }

}
