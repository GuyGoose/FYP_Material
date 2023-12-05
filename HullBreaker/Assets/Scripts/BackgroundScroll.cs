using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the background right by 1 pixel every frame with deltaTime
        transform.position += new Vector3(0.5f, 0, 0) * Time.deltaTime;

        // When the background is at x = 115.2, move it back to x = -57.6
        if (transform.position.x >= 115.2 * 2)
        {
            transform.position = new Vector3(-57.6f, 0, 0);
        }
    }
}
