using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    public UnityAction<GameObject, float, int> OnAttack;
    public UnityAction<Health> OnCritical;
    public UnityAction<Attack, GameObject, bool> OnKill;
    [HideInInspector]
    public Actor Agressor;

    bool poolable = false;

    public bool IgnoreNeutered = false;

    protected void Awake()
    {
        Agressor = GetComponentInParent<Actor>();
        poolable = !(GetComponent<Poolable>() == null);
        AttackAwake();
    }

    protected virtual void AttackAwake() { }


    protected void OnDisable()
    {
        if (poolable)
        {
            OnAttack = null;
            OnCritical = null;
            OnKill = null;
        }
    }

    public abstract void AttackTarget(GameObject target, float multiplier = 1f);

    public void SetupClone(Attack clone)
    {
        //Debug.Log("Cloning from " + gameObject.name + " to " + clone.gameObject.name);

        clone.OnAttack += OnAttack;
        clone.OnCritical += OnCritical;
        clone.OnKill += OnKill;
    }
}
