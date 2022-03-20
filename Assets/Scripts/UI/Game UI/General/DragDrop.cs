using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    public string Type = "Ability";
    public UnityAction<DropSlot> OnInsert;
    public UnityAction<DropSlot> OnRemove;
    public UnityAction OnClick;
    public DropSlot AssignedSlot;
    
    private void Awake()
    {
        OnInsert += AssignToSlot;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void AssignToSlot(DropSlot slot)
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
            AssignedSlot.OnRemove(gameObject);
            OnRemove?.Invoke(AssignedSlot);
            AssignedSlot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
