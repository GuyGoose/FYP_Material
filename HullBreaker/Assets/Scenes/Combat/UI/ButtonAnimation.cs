using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    // Used to anaimate the Action Images to correspond with the states of the buttons in them

    // When the button is being hovered over > Enable the bool "Hovered" in the animator
    // When the button is pressed > Enable the trigger "Pressed" in the animator

    public Animator animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hovered", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hovered", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetTrigger("Pressed");
    }

    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     animator.SetTrigger("Pressed");
    // }
}