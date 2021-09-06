using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int Type = 0;
    [HideInInspector]
    public Vector2 Offset = Vector2.zero;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() && 
            eventData.pointerDrag.GetComponent<DragDrop>().Type == Type)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + Offset;
            eventData.pointerDrag.GetComponent<DragDrop>().OnInsert.Invoke(this);
        }
    }
}
