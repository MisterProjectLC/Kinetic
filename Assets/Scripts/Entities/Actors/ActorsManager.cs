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

    public static Actor Player;
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

            Player = ClassToActor[Hermes.CurrentClass];

            Player.gameObject.SetActive(true);
            PlayerCamera = Player.GetComponentInChildren<Camera>();
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
            Player.gameObject.transform.position = Hermes.SpawnPosition;
    }


    public void RegisterActor(Actor actor)
    {
        if (!Actors.ContainsKey(actor.Affiliation))
            Actors.Add(actor.Affiliation, new List<Actor>());

        Actors[actor.Affiliation].Add(actor);
    }


    public Actor GetPlayer()
    {
        return Player;
    }

    public Camera GetPlayerCamera()
    {
        return PlayerCamera;
    }
}
