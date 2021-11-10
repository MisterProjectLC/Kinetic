using UnityEngine;
using UnityEngine.UI;

public class FallIndicator : MonoBehaviour
{
    Color color;
    Image image;
    PlayerFallHandler fallHandler;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        color = image.color;
        fallHandler = ActorsManager.AM.GetPlayer().GetComponentInChildren<PlayerFallHandler>();
        fallHandler.OnChange += OnChange;
    }

    // Update is called once per frame
    void OnChange()
    {
        color.a = fallHandler.TimeUnderLimit;
        image.color = color;
    }
}
