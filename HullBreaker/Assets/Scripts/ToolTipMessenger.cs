using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipMessenger : MonoBehaviour
{
    public Canvas parentCanvas;
    public Transform ToolTipTransform;
    public static ToolTipMessenger Instance;
    public CanvasGroup canvasGroup;

    public TMP_Text Title, Description;

    bool isShowing;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        isShowing = false;

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < 1 && isShowing) {
            canvasGroup.alpha += Time.deltaTime * 3;
        } else if (canvasGroup.alpha > 0 && !isShowing) {
            canvasGroup.alpha -= Time.deltaTime * 3;
        }

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out movePos);
        // Changed to fix issue with positioning in world map
        ToolTipTransform.position = Input.mousePosition;
        
    }

    public void Show(string title, string description) {
        // Clear the tooltip
        Title.text = "";
        Description.text = "";
        canvasGroup.alpha = 0;
        Title.text = title;
        Description.text = description;
        ToolTipTransform.gameObject.SetActive(true);
        isShowing = true;
    }

    public void Hide() {
        ToolTipTransform.gameObject.SetActive(false);
        isShowing = false;
    }
}