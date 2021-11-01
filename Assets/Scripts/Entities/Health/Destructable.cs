using System.Collections;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private GameObject BrokenModelPrefab;

    [SerializeField]
    private GameObject OriginalModel;

    private Health m_Health;

    // Start is called before the first frame update
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
            GameObject newBroken = Instantiate(BrokenModelPrefab);
            newBroken.transform.position = OriginalModel.transform.position;
            newBroken.transform.rotation = OriginalModel.transform.rotation;
            foreach (Rigidbody rigidbody in newBroken.GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.AddForce(Random.insideUnitSphere * Random.Range(0.05f, 0.5f), ForceMode.Impulse);
            }
        }

        if (GetComponent<Poolable>())
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
        else
            StartCoroutine("Destruct");
    }

    IEnumerator Destruct()
    {
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        }
        else
            yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
}
