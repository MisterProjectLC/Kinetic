using UnityEngine;
using UnityEngine.UI;

public class FallIndicator : MonoBehaviour
{
    Color color;
    Image image;
    PlayerFallHandler fallHandler;

    [SerializeField]
    GameObjectReference PlayerReference;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        color = image.color;
        fallHandler = PlayerReference.Reference.GetComponentInChildren<PlayerFallHandler>();
        fallHandler.OnChange += OnChange;
    }

    // Update is called once per frame
    void OnChange()
    {
        color.a = fallHandler.TimeUnderLimit;
        image.color = color;
    }
}
