// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class ActionMenu : MonoBehaviour {
//     // The action buttons
//     public List<ActionButton> actionButtons = new List<ActionButton>();

//     // Start is called before the first frame update
//     void Start() {
//         // For each child of this object get the button
//         foreach (Transform child in transform) {
//             // Get the button
//             UnityEngine.UI.Button button = child.GetComponent<UnityEngine.UI.Button>();
//             // Get the button script
//             ActionButton buttonScript = button.GetComponent<ActionButton>();
//             // Add the button to the list
//             actionButtons.Add(buttonScript);
//         }
//     }

//     // Update is called once per frame
//     void Update() {
        
//     }

// }