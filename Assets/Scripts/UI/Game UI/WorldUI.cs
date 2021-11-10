using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public float MinDistance;
    public float MaxDistance;

    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Camera.main.transform.position - transform.position).magnitude;
        canvasGroup.alpha = 1f - Mathf.Clamp((distance - MinDistance)/(MaxDistance - MinDistance), 0f, 1f);
    }
}
