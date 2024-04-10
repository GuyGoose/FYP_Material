using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StatusEffect : MonoBehaviour
{
    public int Waterlogged_nb = 0;
    public GameObject Waterlogged_icon;
    public int Burning_nb = 0;
    public GameObject Burning_icon;
    public int Regeneration_nb = 0;
    public GameObject Regeneration_icon;
    public int Virus_nb = 0;
    public GameObject Virus_icon;
    public int Reinforced_nb = 0;
    public GameObject Reinforced_icon;
    public int Improved_nb = 0;
    public GameObject Improved_icon;
    public int Accuracy_nb = 0;
    public GameObject Accuracy_icon;

    public void ApplyStatusEffect(EnumHolder.StatusEffect statusEffect, int statusAmount)
    {
        switch (statusEffect)
        {
            case EnumHolder.StatusEffect.Waterlogged:
                Waterlogged_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Burning:
                Burning_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Regeneration:
                Regeneration_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Virus:
                Virus_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Reinforced:
                Reinforced_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Improved:
                Improved_nb += statusAmount;
                break;
            case EnumHolder.StatusEffect.Accuracy:
                Accuracy_nb += statusAmount;
                break;
            default:
                Debug.Log("Invalid status effect");
                break;
        }
    }

    public void AdjustAtEndOfTurn(Health targetHealth)
    {
        if (Waterlogged_nb > 0)
        {
            Waterlogged_nb--;
        }
        if (Burning_nb > 0)
        {
            // Deal damage to the target equal to the burning amount
            targetHealth.TakeDamage(Burning_nb);
            Burning_nb--;
        }
        if (Regeneration_nb > 0)
        {
            // Heal the target equal to the regeneration amount
            targetHealth.Heal(Regeneration_nb);
            Regeneration_nb--;
        }
        if (Virus_nb > 0)
        {
            Virus_nb--;
        }
        if (Reinforced_nb > 0)
        {
            // Add shield to the target equal to the reinforced amount
            targetHealth.Shield(Reinforced_nb);
            Reinforced_nb--;
        }
    }

    public int CheckImproved()
    {
        if (Improved_nb > 0)
        {
            return Improved_nb;
        }
        return 0;
    }

    public int CheckAccuracy()
    {
        if (Accuracy_nb > 0)
        {
            return Accuracy_nb;
        }
        return 0;
    }

    public bool isWaterlogged()
    {
        return Waterlogged_nb > 0;
    }

    public bool isVirus()
    {
        return Virus_nb > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Waterlogged_nb > 0)
        {
            Waterlogged_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Waterlogged_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of waterlogged stacks
            text.text = Waterlogged_nb.ToString();
        }
        else
        {
            Waterlogged_icon.SetActive(false);
        }

        if (Burning_nb > 0)
        {
            Burning_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Burning_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of burning stacks
            text.text = Burning_nb.ToString();
        }
        else
        {
            Burning_icon.SetActive(false);
        }

        if (Regeneration_nb > 0)
        {
            Regeneration_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Regeneration_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of regeneration stacks
            text.text = Regeneration_nb.ToString();
        }
        else
        {
            Regeneration_icon.SetActive(false);
        }

        if (Virus_nb > 0)
        {
            Virus_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Virus_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of virus stacks
            text.text = Virus_nb.ToString();
        }
        else
        {
            Virus_icon.SetActive(false);
        }

        if (Reinforced_nb > 0)
        {
            Reinforced_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Reinforced_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of reinforced stacks
            text.text = Reinforced_nb.ToString();
        }
        else
        {
            Reinforced_icon.SetActive(false);
        }

        if (Improved_nb > 0)
        {
            Improved_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Improved_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of improved stacks
            text.text = Improved_nb.ToString();
        }
        else
        {
            Improved_icon.SetActive(false);
        }

        if (Accuracy_nb > 0)
        {
            Accuracy_icon.SetActive(true);
            // Get the TextMeshProUGUI component of the child object
            TextMeshProUGUI text = Accuracy_icon.GetComponentInChildren<TextMeshProUGUI>();
            // Set the text to the number of accuracy stacks
            text.text = Accuracy_nb.ToString();
        }
        else
        {
            Accuracy_icon.SetActive(false);
        }
        
    }
}
