using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger,
    Button
}
public class ScenePartLoader : MonoBehaviour
{
    private Transform player;
    public CheckMethod checkMethod;
    public float loadRange;
    //Scene state
    private bool isLoaded;
    private bool shouldLoad;

    [SerializeField]
    bool Checkpoint;
    [SerializeField]
    Transform CheckpointPosition;

    [SerializeField]
    List<string> RequiredScenes;

    List<string> RegisteredObjects = new List<string>();

    void Start()
    {
        player = ActorsManager.AM.GetPlayer().transform;

        //verify if the scene is already open to avoid opening a scene twice
        if (SceneManager.sceneCount > 0)
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                    break;
                }
            }
    }

    void Update()
    {
        //Checking which method to use
        switch (checkMethod) {
            case CheckMethod.Distance:
                DistanceCheck();
                break;

            case CheckMethod.Trigger:
                TriggerCheck();
                break;
        }

    }

    void DistanceCheck()
    {
        //Checking if the player is within the range
        if (Vector3.Distance(player.position, transform.position) < loadRange)
            LoadScene();
        else
            UnLoadScene();
    }

    void TriggerCheck()
    {
        //shouldLoad is set from the Trigger methods
        if (shouldLoad)
            LoadScene();
        else
            UnLoadScene();
    }


    public void RegisterObject(string id)
    {
        RegisteredObjects.Add(id);
    }

    public bool CheckObject(string id)
    {
        return RegisteredObjects.Contains(id);
    }


    public void LoadScene()
    {
        if (!isLoaded)
        {
            //RegisteredObjects = SaveManager.Load<List<string>>(gameObject.name);
            if (RegisteredObjects == default(List<string>))
                RegisteredObjects = new List<string>();
            isLoaded = true;

            if (Checkpoint)
            {
                Hermes.SpawnPosition = CheckpointPosition.position;
                List<string> s = new List<string>(RequiredScenes);
                s.Insert(0, gameObject.name);
                Hermes.SpawnAreas = s;
            }

            SceneManager.LoadScene(gameObject.name, LoadSceneMode.Additive);
        }
    }

    public void UnLoadScene()
    {
        if (isLoaded)
        {
            //SaveManager.Save(RegisteredObjects, gameObject.name);
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            shouldLoad = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            shouldLoad = false;
    }


    private void OnDrawGizmos()
    {
        if (checkMethod == CheckMethod.Distance)
        Gizmos.DrawSphere(transform.position, loadRange);
    }

    public List<string> GetRequiredScenes()
    {
        return RequiredScenes;
    }

}