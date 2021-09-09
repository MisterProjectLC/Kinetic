using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    public string Type = "Ability";
    public UnityAction<DropSlot> OnInsert;
    public UnityAction<DropSlot> OnRemove;
    public DropSlot AssignedSlot;

    private void Awake()
    {
        OnInsert += assignToSlot;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void assignToSlot(DropSlot slot)
    {
        AssignedSlot = slot;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (AssignedSlot)
        {
            OnRemove.Invoke(AssignedSlot);
            AssignedSlot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }
}
