using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public static ActorsManager AM;
    [HideInInspector]
    public static Dictionary<int, List<Actor>> Actors;

    [SerializeField]
    private Actor PlayerSerialized;
    public static Actor Player;


    // Start is called before the first frame update
    void Awake()
    {
        if (AM)
            Destroy(gameObject);
        else
        {
            AM = this;
            Player = PlayerSerialized;
            Actors = new Dictionary<int, List<Actor>>();
        }
    }


    public void RegisterActor(Actor actor)
    {
        if (!Actors.ContainsKey(actor.Affiliation))
            Actors.Add(actor.Affiliation, new List<Actor>());

        Actors[actor.Affiliation].Add(actor);
    }
}
