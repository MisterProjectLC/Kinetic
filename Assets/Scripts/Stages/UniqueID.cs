using UnityEngine;
using UnityEngine.Events;

public class UniqueID : MonoBehaviour
{
    public string ID { get; private set; }

    public UnityAction OnObjectRegistered;

    // Start is called before the first frame update
    void Awake()
    {
        ID = transform.position.sqrMagnitude + "-" + name + "-" + transform.GetSiblingIndex();

    }

    private void Start()
    {
        if (MySceneManager.MSM.ObjectRegistered(ID, gameObject.scene.name))
        {
            //Debug.Log("Already " + ID);
            OnObjectRegistered?.Invoke();
        }
    }


    public void RegisterID()
    {
        //Debug.Log("Registered " + ID);
        MySceneManager.MSM.RegisterObject(ID, gameObject.scene.name);
    }
}