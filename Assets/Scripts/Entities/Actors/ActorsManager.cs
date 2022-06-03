using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public static ActorsManager AM;
    [HideInInspector]
    public static Dictionary<int, List<Actor>> Actors;

    [SerializeField]
    Hermes.PlayerClass ActiveClass;
    [SerializeField]
    SerializableDictionary<Hermes.PlayerClass, Actor> ClassToActor;

    public GameObjectReference PlayerReference;
    static Camera PlayerCamera;

    // Start is called before the first frame update
    void Awake()
    {
        if (AM)
            Destroy(gameObject);
        else
        {
            AM = this;

            if (ActiveClass != Hermes.PlayerClass.Null && Hermes.CurrentClass == Hermes.PlayerClass.Null)
                Hermes.CurrentClass = ActiveClass;

            PlayerReference.Reference = ClassToActor[Hermes.CurrentClass].gameObject;
            Debug.Log(PlayerReference.Reference.name);

            PlayerReference.Reference.SetActive(true);
            PlayerCamera = PlayerReference.Reference.GetComponentInChildren<Camera>();
            Actors = new Dictionary<int, List<Actor>>();
        }
    }

    private void Start()
    {
        StartCoroutine(InitialSpawnPosition());
    }

    IEnumerator InitialSpawnPosition()
    {
        yield return new WaitForSeconds(0.1f);
        if (Hermes.SpawnPosition != Vector3.zero)
            PlayerReference.Reference.transform.position = Hermes.SpawnPosition;
    }


    public void RegisterActor(Actor actor)
    {
        if (!Actors.ContainsKey(actor.Affiliation))
            Actors.Add(actor.Affiliation, new List<Actor>());

        Actors[actor.Affiliation].Add(actor);
    }


    public Actor GetPlayer()
    {
        return PlayerReference.Reference.GetComponent<Actor>();
    }

    public Camera GetPlayerCamera()
    {
        return PlayerCamera;
    }
}
