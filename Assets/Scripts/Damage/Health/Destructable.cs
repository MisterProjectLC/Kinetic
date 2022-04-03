using System.Collections;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private GameObject BrokenModelPrefab;
    [SerializeField]
    bool MatchModelScale = false;

    [SerializeField]
    private GameObject OriginalModel;

    private Health m_Health;


    private void Awake()
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += Delete;
    }

    void Start()
    {
        if (OriginalModel == null)
            OriginalModel = gameObject;

        m_Health = GetComponent<Health>();
        m_Health.OnDie += OnDie;
    }

    void OnDie()
    {
        if (BrokenModelPrefab != null)
        {
            if (GetComponent<UniqueID>())
                GetComponent<UniqueID>().RegisterID();

            GameObject newBroken = Instantiate(BrokenModelPrefab);
            newBroken.transform.position = OriginalModel.transform.position;
            newBroken.transform.rotation = OriginalModel.transform.rotation;
            if (MatchModelScale)
                newBroken.transform.localScale = OriginalModel.transform.lossyScale;

            Vector3 entityForce = m_Health.GetComponent<PhysicsEntity>() != null ? m_Health.GetComponent<PhysicsEntity>().GetMoveVelocity() : Vector3.zero;
            entityForce = entityForce.normalized * 13*Mathf.Clamp(entityForce.magnitude, 0f, 140f);
            foreach (Rigidbody rigidbody in newBroken.GetComponentsInChildren<Rigidbody>())
                rigidbody.AddForce(entityForce + Random.insideUnitSphere * Random.Range(0.05f, 0.5f), ForceMode.Impulse);
        }

        if (GetComponent<Poolable>())
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
        else
            StartCoroutine(Destruct());
    }


    IEnumerator Destruct()
    {
        /*
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        }
        else*/
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }

    void Delete()
    {
        //Debug.Log("Delete");
        Destroy(gameObject);
    }
}
