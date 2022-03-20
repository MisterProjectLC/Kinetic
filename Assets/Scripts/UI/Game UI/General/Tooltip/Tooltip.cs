using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    RectTransform rectTransform;
    Image image;
    Color backgroundColor;
    float clock = 0f;
    CanvasGroup canvasGroup;

    [SerializeField]
    float waitTime = 0.5f;

    [SerializeField]
    Text title;

    [SerializeField]
    Text description;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        backgroundColor = image.color;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;
        backgroundColor.a = 0f;
        clock = 0f;
        image.color = backgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.unscaledDeltaTime;
        if (clock > waitTime && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += 2*Time.unscaledDeltaTime;
            backgroundColor.a += 2*Time.unscaledDeltaTime;
            image.color = backgroundColor;
        }

        Vector2 position = Input.mousePosition;
        transform.position = position;
        rectTransform.pivot = new Vector2(position.x < Screen.width / 2 ? 0 : 1, position.y < Screen.height / 2 ? 0 : 1);
    }

    public void Setup(string title, string description)
    {
        this.title.text = title;
        this.description.text = description;
    }
}
