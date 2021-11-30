using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public static ActorsManager AM;
    [HideInInspector]
    public static Dictionary<int, List<Actor>> Actors;

    [SerializeField]
    private string PlayerSerialized;
    public static Actor Player;

    [SerializeField]
    Actor Ninja;
    [SerializeField]
    Actor Vanguard;

    // Start is called before the first frame update
    void Awake()
    {
        if (AM)
            Destroy(gameObject);
        else
        {
            AM = this;

            if (PlayerSerialized != "" && Hermes.heroName == "")
                Hermes.heroName = PlayerSerialized;

            switch (Hermes.heroName)
            {
                default:
                    Player = Ninja;
                    break;

                case "Vanguard":
                    Player = Vanguard;
                    break;
            }

            Player.gameObject.SetActive(true);
            if (Hermes.SpawnPosition != Vector3.zero)
                Player.gameObject.transform.position = Hermes.SpawnPosition;
            Actors = new Dictionary<int, List<Actor>>();
        }
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
}
