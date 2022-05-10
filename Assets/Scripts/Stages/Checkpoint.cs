using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        if (MySceneManager.MSM.GetPartLoader(gameObject.scene.name).HasCheckpoint())
        {
            Hermes.SpawnPosition = transform.position;
        }
    }
}
