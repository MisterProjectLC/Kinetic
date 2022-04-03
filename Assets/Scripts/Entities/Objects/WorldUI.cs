using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public float MinDistance;
    public float MaxDistance;

    CanvasGroup canvasGroup;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        camera = Camera.main;
        GetComponent<Canvas>().worldCamera = camera;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (camera.transform.position - transform.position).magnitude;
        canvasGroup.alpha = 1f - Mathf.Clamp((distance - MinDistance)/(MaxDistance - MinDistance), 0f, 1f);
    }
}
