using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Powerup : Pickup
{
    static Vector3 rotation = new Vector3(0f, 60f, 0f);

    [SerializeField]
    GameObject icon;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }

    private void OnEnable()
    {
        icon.SetActive(true);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    protected override void OnAutoDestruct()
    {
        icon.SetActive(false);
    }
}
