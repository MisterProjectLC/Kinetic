using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string Type = "Ability";
    [HideInInspector]
    public Vector2 Offset = Vector2.zero;

    [SerializeField]
    public LocalizedString LabelText = "";

    [HideInInspector]
    Text Label;

    public DragDrop InsertedDragDrop;
    Transform InsertedDragDropOldParent;
    public UnityAction<DragDrop> OnInserted;

    bool soundEnabled = false;

    private void Awake()
    {
        Offset = Vector2.zero;
        Label = GetComponentInChildren<Text>();
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }

    void OnEnable()
    {
        Label.GetComponent<Text>().text = LabelText.value;
        StartCoroutine(EnableSound());
    }

    IEnumerator EnableSound()
    {
        yield return new WaitForSeconds(0.03f);
        soundEnabled = true;
    }

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
        if (InsertedDragDrop)
            return;

        Label.gameObject.SetActive(false);

        if (GetComponent<AudioSource>() && soundEnabled)
            GetComponent<AudioSource>().Play();


        InsertedDragDropOldParent = dragDrop.transform.parent;
        Debug.Log("Dropped: " + dragDrop.GetComponent<LoadoutOption>().Option.name + ", " + InsertedDragDropOldParent.gameObject.name + 
            ". Offset: " + Offset);

        InsertedDragDrop = dragDrop.GetComponent<DragDrop>();
        dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + Offset;
        InsertedDragDrop.OnInsert?.Invoke(this);
        InsertedDragDrop.transform.SetParent(transform);
        OnInserted?.Invoke(InsertedDragDrop);
    }


    public void OnRemove(GameObject dragDrop)
    {
        if (!InsertedDragDrop || !InsertedDragDropOldParent)
            return;

        InsertedDragDrop.transform.SetParent(InsertedDragDropOldParent);
        Debug.Log("Removed: " + InsertedDragDrop.GetComponent<LoadoutOption>().Option.name + ", " + InsertedDragDropOldParent.gameObject.name);
        InsertedDragDrop = null;
        Label.gameObject.SetActive(true);
        soundEnabled = true;
    }
}
