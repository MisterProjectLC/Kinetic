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




    public void LoadScene()
    {
        if (!isLoaded)
        {
            //Loading the scene, using the gameobject name as it's the same as the name of the scene to load
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            //We set it to true to avoid loading the scene twice
            isLoaded = true;
        }
    }

    public void UnLoadScene()
    {
        if (isLoaded)
        {
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
        if (checkMethod != CheckMethod.Trigger)
        Gizmos.DrawSphere(transform.position, loadRange);
    }

}