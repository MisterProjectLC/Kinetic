using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    GameTrigger gameTrigger;

    [SerializeField]
    AudioClip newSong;

    // Start is called before the first frame update
    void Start()
    {
        gameTrigger = GetComponent<GameTrigger>();
        gameTrigger.OnTriggerActivate += OnTriggerActivate;
    }


    void OnTriggerActivate()
    {
        MusicManager.MM.ChangeBaseSong(newSong);
    }
    
}
