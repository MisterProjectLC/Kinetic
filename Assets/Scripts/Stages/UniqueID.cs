using UnityEngine;
using UnityEngine.Events;

public class UniqueID : MonoBehaviour
{
    public string ID { get; private set; }

    [SerializeField]
    MySceneManager.Lifetime lifetime = MySceneManager.Lifetime.ReturnOnGameover; 

    public UnityAction OnObjectRegistered;

    // Start is called before the first frame update
    void Awake()
    {
        ID = transform.position.sqrMagnitude + "-" + name + "-" + transform.GetSiblingIndex();
    }

    void Start()
    {
        if (MySceneManager.MSM.ObjectRegistered(ID, gameObject.scene.name))
        {
            //Debug.Log("Already " + ID);
            OnObjectRegistered?.Invoke();
        }
    }

    public void RegisterID()
    {
        MySceneManager.MSM.RegisterObject(ID, gameObject.scene.name, lifetime);
    }


    public void UnRegisterID()
    {
        MySceneManager.MSM.UnRegisterObject(ID, gameObject.scene.name, lifetime);
    }
}