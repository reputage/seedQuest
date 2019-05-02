using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class MouseOverQuestion : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        int child = int.Parse(eventData.pointerEnter.name);
        eventData.pointerEnter.transform.parent.parent.parent.GetChild(2).GetChild(child).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        int child = int.Parse(eventData.pointerEnter.name);
        eventData.pointerEnter.transform.parent.parent.parent.GetChild(2).GetChild(child).gameObject.SetActive(false);
    }

}
