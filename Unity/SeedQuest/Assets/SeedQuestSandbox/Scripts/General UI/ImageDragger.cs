using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ImageDragger : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool useViewport;
    public GameObject viewport;

    private Vector3 currentPosition;

    public void OnDrag(PointerEventData eventData)
    {
        if (currentPosition == Vector3.zero)
            currentPosition = Input.mousePosition;

        transform.position = transform.position - (currentPosition - Input.mousePosition);
        currentPosition = Input.mousePosition;

        if (useViewport)
        {
            float x = (transform.GetComponent<RectTransform>().sizeDelta.x - viewport.GetComponent<RectTransform>().rect.width) / 2;
            float y = (transform.GetComponent<RectTransform>().sizeDelta.y - viewport.GetComponent<RectTransform>().rect.height) / 2;

            if (transform.localPosition.x > x)
                transform.localPosition = new Vector3(x, transform.localPosition.y, 0);

            else if (transform.localPosition.x < -x)
                transform.localPosition = new Vector3(-x, transform.localPosition.y, 0);

            if (transform.localPosition.y > y)
                transform.localPosition = new Vector3(transform.localPosition.x, y, 0);
            else if (transform.localPosition.y < -y)
                transform.localPosition = new Vector3(transform.localPosition.x, -y, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentPosition = Vector3.zero;
        if (useViewport)
            return;

        if (transform.localPosition.x > 500)
            transform.localPosition = new Vector3(500, transform.localPosition.y, 0);

        else if (transform.localPosition.x < -500)
            transform.localPosition = new Vector3(-500, transform.localPosition.y, 0);

        if (transform.localPosition.y > 600)
            transform.localPosition = new Vector3(transform.localPosition.x, 600, 0);

        else if (transform.localPosition.y < -600)
            transform.localPosition = new Vector3(transform.localPosition.x, -600, 0);
    }
}
