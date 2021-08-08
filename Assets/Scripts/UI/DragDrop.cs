using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    public int Type = 0;
    public UnityAction<DropSlot> OnInsert;
    public UnityAction<DropSlot> OnRemove;
    public DropSlot assignedSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        OnInsert += assignToSlot;
    }

    void assignToSlot(DropSlot slot)
    {
        assignedSlot = slot;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (assignedSlot)
        {
            OnRemove.Invoke(assignedSlot);
            assignedSlot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }
}
