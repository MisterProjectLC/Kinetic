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
        gameTrigger.SubscribeToTriggerActivate(OnTriggerActivate);
    }


    void OnTriggerActivate()
    {
        MusicManager.MM.ChangeBaseSong(newSong);
    }
    
}
