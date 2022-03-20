using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public string Title;

    [HideInInspector]
    [TextArea(3, 10)]
    public string Description;


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Entering " + gameObject.name);
        TooltipSystem.TS.Show(Title, Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.TS.Hide();
    }
}
