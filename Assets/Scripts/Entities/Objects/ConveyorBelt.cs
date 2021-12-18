using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    private GameObject Detail;
    List<GameObject> Details = new List<GameObject>();

    Vector3 SpawnPosition;
    Vector3 DespawnPosition;

    Movepad movepad;
    Vector3 BeltDirection;

    float SpawnTime = 5f;
    float clock = 0f;


    private void Start()
    {
        movepad = GetComponent<Movepad>();
        SpawnPosition = movepad.GetUpperExtremePoint();
        DespawnPosition = movepad.GetLowerExtremePoint();
        BeltDirection = movepad.GetMoveDirection().normalized;

        SpawnTime = 5f/movepad.Speed;
        /*
        if (Vector3.Dot(BeltDirection, (DespawnPosition - SpawnPosition).normalized) < 0)
        {
            Vector3 helper = SpawnPosition;
            SpawnPosition = DespawnPosition;
            DespawnPosition = helper;
        }
        */
    }


    // Update is called once per frame
    void Update()
    {
        // Move and delete Stuff
        for (int i = 0; i < Details.Count; i++) {
            Details[i].transform.position += movepad.GetMoveDirection() * Time.deltaTime;
            if ((SpawnPosition - DespawnPosition).magnitude < (SpawnPosition - Details[i].transform.position).magnitude)
            {
                Destroy(Details[i]);
                //ObjectManager.OM.EraseObject(Details[i].GetComponent<Poolable>());
                Details.RemoveAt(i);
                i--;
            }
        }
            
        // Spawn Stuff
        clock += Time.deltaTime;
        if (clock > SpawnTime) {
            clock = 0f;

            //GameObject instance = ObjectManager.OM.SpawnObjectFromPool(Detail.GetComponent<Poolable>().Type, Detail);
            //instance.transform.position = SpawnPosition;
            //instance.transform.rotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            //instance.transform.parent = transform;
            GameObject instance = Instantiate(Detail, SpawnPosition, transform.rotation * Quaternion.Euler(0f, 90f, 0f), transform);
            Details.Add(instance);
        }
    }
}
