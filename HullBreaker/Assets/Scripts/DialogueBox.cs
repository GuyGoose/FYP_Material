using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    //public string[] wordsToDisplay;
    public float textSpeed;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        //DisplayDialogue("Hello, this is a test dialogue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDialogue(string dialogue) {
        index = 0;
        dialogueText.text = "";
        StopAllCoroutines();
        StartCoroutine(TypeText(dialogue));
    }

    IEnumerator TypeText(string dialogue) {
        // Convert the string to a char array and display each letter with a delay
        foreach (char letter in dialogue.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
