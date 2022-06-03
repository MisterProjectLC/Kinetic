using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    GameObject Detail;
    PoolableEnum detailType;

    List<GameObject> Details = new List<GameObject>();

    Vector3 SpawnPosition;
    Vector3 DespawnPosition;
    Vector3 BeltSize;

    Movepad movepad;
    //Vector3 BeltDirection;

    float SpawnTime = 6f;

    Clock clock;


    private void Start()
    {
        detailType = Detail.GetComponent<Poolable>().Type;

        movepad = GetComponent<Movepad>();
        SpawnPosition = movepad.GetUpperExtremePoint();
        DespawnPosition = movepad.GetLowerExtremePoint();
        //BeltDirection = movepad.GetMoveDirection().normalized;
        BeltSize = new Vector3(transform.lossyScale.z, 1f, 1f);

        SpawnTime /= movepad.Speed;
        clock = new Clock(SpawnTime);
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
            Details[i].transform.localPosition += movepad.GetMoveDirection() * Time.deltaTime;
            if ((SpawnPosition - DespawnPosition).magnitude < (SpawnPosition - Details[i].transform.position).magnitude)
            {
                Destroy(Details[i]);
                //ObjectManager.OM.EraseObject(Details[i].GetComponent<Poolable>());
                Details.RemoveAt(i);
                i--;
            }
        }
            
        // Spawn Stuff
        if (clock.TickAndRing(Time.deltaTime)) {
            GameObject instance = ObjectManager.SpawnObjectFromPool(detailType, Detail);
            instance.transform.position = SpawnPosition;
            instance.transform.rotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            instance.transform.localScale = BeltSize;
            instance.transform.parent = transform;
            //GameObject instance = Instantiate(Detail, SpawnPosition, transform.rotation * Quaternion.Euler(0f, 90f, 0f), transform);
            Details.Add(instance);
        }
    }
}
