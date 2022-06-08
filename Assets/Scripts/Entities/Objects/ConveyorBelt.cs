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

    Slidepad movepad;
    //Vector3 BeltDirection;

    float SpawnTime = 6f;

    Clock clock;


    private void Start()
    {
        detailType = Detail.GetComponent<Poolable>().Type;

        movepad = GetComponent<Slidepad>();
        Debug.Log(movepad);
        SpawnPosition = movepad.GetUpperExtremePoint();
        DespawnPosition = movepad.GetLowerExtremePoint();
        BeltSize = new Vector3(transform.lossyScale.z, 1f, 1f);

        SpawnTime /= movepad.Speed;
        clock = new Clock(SpawnTime);
    }


    // Update is called once per frame
    void Update()
    {
        // Move and delete Stuff
        for (int i = 0; i < Details.Count; i++) {
            Details[i].transform.position += movepad.moveVector * Time.deltaTime;
            if ((SpawnPosition - DespawnPosition).magnitude < (SpawnPosition - Details[i].transform.position).magnitude)
            {
                Destroy(Details[i]);
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
            Details.Add(instance);
        }
    }
}
