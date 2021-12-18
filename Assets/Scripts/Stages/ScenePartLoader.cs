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
    List<string> PerpetualObjects = new List<string>();

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

    private void OnDisable()
    {
        SaveManager.Save(PerpetualObjects, gameObject.name);
    }

    private void OnDestroy()
    {
        SaveManager.Save(PerpetualObjects, gameObject.name);
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


    public void RegisterObject(string id, MySceneManager.Lifetime lifetime)
    {
        RegisteredObjects.Add(id);

        if (lifetime >= MySceneManager.Lifetime.ReturnOnQuit)
        {
            Debug.Log("Added to Perpetual: " + id);
            PerpetualObjects.Add(id);
        }
    }

    public bool CheckObject(string id)
    {
        return RegisteredObjects.Contains(id);
    }


    public void LoadScene()
    {
        if (isLoaded)
            return;
        isLoaded = true;

        // Load saved objects
        Debug.Log("LoadScene");
        if (RegisteredObjects.Count <= 0)
        {
            Debug.Log("Loading objects");
            RegisteredObjects = SaveManager.Load<List<string>>(gameObject.name);
            foreach (string ob in RegisteredObjects)
                Debug.Log("Loading objects: " + ob);
        }

        // Save checkpoint
        if (Checkpoint)
        {
            Hermes.SpawnPosition = CheckpointPosition.position;
            List<string> s = new List<string>(RequiredScenes);
            s.Insert(0, gameObject.name);
            Hermes.SpawnAreas = s;
        }

        SceneManager.LoadScene(gameObject.name, LoadSceneMode.Additive);
    }

    public void UnLoadScene()
    {
        if (!isLoaded)
            return;
        isLoaded = false;

        foreach (string ob in PerpetualObjects)
            Debug.Log("Saving objects: " + ob);
        SaveManager.Save(PerpetualObjects, gameObject.name);
        SceneManager.UnloadSceneAsync(gameObject.name);
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