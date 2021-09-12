using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string Type = "Ability";
    [HideInInspector]
    public Vector2 Offset = Vector2.zero;

    public DragDrop InsertedDragDrop;
    public UnityAction<DragDrop> OnInserted;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() && 
            eventData.pointerDrag.GetComponent<DragDrop>().Type == Type)
        {
            OnDrop(eventData.pointerDrag);
        }
    }

    public void OnDrop(GameObject dragDrop)
    {
        InsertedDragDrop = dragDrop.GetComponent<DragDrop>();
        dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + Offset;
        InsertedDragDrop.OnInsert.Invoke(this);
        if (OnInserted != null)
            OnInserted.Invoke(InsertedDragDrop);
    }
}
