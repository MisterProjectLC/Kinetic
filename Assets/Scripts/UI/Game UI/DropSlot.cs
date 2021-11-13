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
    string LabelText = "";

    [HideInInspector]
    public GameObject Label;

    public DragDrop InsertedDragDrop;
    public UnityAction<DragDrop> OnInserted;

    bool soundEnabled = false;

    private void Awake()
    {
        Label = GetComponentInChildren<Text>().gameObject;
        Label.GetComponent<Text>().text = LabelText;
        StartCoroutine("EnableSound");
    }

    IEnumerator EnableSound()
    {
        yield return new WaitForSeconds(0.01f);
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
        Label.SetActive(false);

        if (GetComponent<AudioSource>() && soundEnabled)
            GetComponent<AudioSource>().Play();

        InsertedDragDrop = dragDrop.GetComponent<DragDrop>();
        dragDrop.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + Offset;
        InsertedDragDrop.OnInsert.Invoke(this);
        if (OnInserted != null)
            OnInserted.Invoke(InsertedDragDrop);
    }


    public void OnRemove(GameObject dragDrop)
    {
        InsertedDragDrop = null;
        Label.SetActive(true);
    }
}