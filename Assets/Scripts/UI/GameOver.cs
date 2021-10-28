using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Image background;
    [SerializeField]
    Text text;

    AudioSource audioSource;
    float timeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ActorsManager.AM.GetPlayer().GetComponent<Health>().OnDie += StartGameOver;
        audioSource = GetComponent<AudioSource>();
    }

    void StartGameOver()
    {
        StartCoroutine("RunGameOver");
    }

    IEnumerator RunGameOver()
    {
        audioSource.Play();

        float timer = 1f;
        while (timer > 0f)
        {
            float increment = Time.unscaledDeltaTime / 3f;
            yield return new WaitForSecondsRealtime(increment);
            background.color += new Color(0f, 0f, 0f, increment);
            text.color += new Color(0f, 0f, 0f, increment);
            timer -= increment;
            timeScale -= increment;
            if (timeScale >= 0f)
                Time.timeScale = timeScale;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
