using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    Outline outline;

    private void Start()
    {
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable == true)
        {
            outline.enabled = true;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
