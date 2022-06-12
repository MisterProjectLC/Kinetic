using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    /*struct Shake
    {
        public Shake(float duration, float intensity)
        {
            this.duration = duration;
            this.intensity = intensity;
        }

        public float duration;
        public float intensity;
    }

    List<Shake> shakes = new List<Shake>();*/

    Vector3 originalPosition;

    float shakeDuration = 0f;
    float shakeIntensity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Shaking();
    }

    void Shaking()
    {
        if (shakeDuration <= 0f)
        {
            shakeIntensity = 0f;
            return;
        }

        shakeDuration -= Time.deltaTime;
        if (shakeDuration <= 0f)
            transform.localPosition = originalPosition;
        else
        {
            Vector3 randomCircle = Random.insideUnitCircle * Random.Range(0f, shakeIntensity);
            transform.localPosition = originalPosition + randomCircle;
        }
    }


    public void Shake(float intensity = 1f, float duration = 0.5f)
    {
        if (shakeIntensity < intensity)
            shakeIntensity = intensity;
        shakeDuration = duration;
    }
}
