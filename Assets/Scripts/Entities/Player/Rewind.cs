using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : Ability
{
    struct PosNRot
    {
        public Vector3 position;
        public Quaternion rotation;

        public PosNRot(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
    }

    [SerializeField]
    int SecondsBack = 5;
    float clock = 0f;
    bool running = false;

    PlayerCharacterController player;
    List<PosNRot> lastTransforms;

    // Start is called before the first frame update
    void Start()
    {
        lastTransforms = new List<PosNRot>(SecondsBack);
        player = GetComponentInParent<PlayerCharacterController>();
        lastTransforms.Add(new PosNRot(player.transform));

        OnUpdate += Updating;
    }

    void Updating()
    {
        if (running)
            return;

        clock += Time.deltaTime;
        if (clock > 1f)
        {
            clock = 0f;
            lastTransforms.Insert(0, new PosNRot(player.transform));
            if (lastTransforms.Count > SecondsBack)
                lastTransforms.RemoveAt(lastTransforms.Count-1);
        }
    }

    public override void Execute()
    {
        StartCoroutine(ExecuteRewind());
    }

    IEnumerator ExecuteRewind()
    {
        PosNRot previousTransform = new PosNRot(player.transform);
        running = true;

        foreach (PosNRot transform in lastTransforms)
        {
            for (int i = 0; i < 10; i++)
            {
                player.transform.position = Vector3.Lerp(previousTransform.position, transform.position, i*0.1f);
                player.transform.rotation = Quaternion.Lerp(previousTransform.rotation, transform.rotation, i * 0.1f);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            previousTransform = transform;
        }

        running = false;
    }

}
