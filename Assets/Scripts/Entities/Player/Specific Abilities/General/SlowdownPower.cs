using UnityEngine;
using UnityEngine.Events;

public class SlowdownPower : MonoBehaviour
{
    PlayerInputHandler input;
    StyleMeter styleMeter;

    bool slowdown = false;
    public UnityAction OnUpdate;

    [SerializeField]
    float slowedTimeSpeed = 0.25f;

    float previousTimeSpeed = 1f;

    private void Start()
    {
        input = GetComponentInParent<PlayerInputHandler>();
        styleMeter = GetComponentInParent<StyleMeter>();
        styleMeter.OnDeplete += () => setSlowdown(false);
    }

    private void Update()
    {
        if (input.GetSlowtime())
            setSlowdown(!slowdown);

        if (slowdown)
            styleMeter.SpendJuice(Time.deltaTime/slowedTimeSpeed);
    }


    void setSlowdown(bool slowdown)
    {
        if (slowdown)
            previousTimeSpeed = Time.timeScale;

        this.slowdown = slowdown;
        Time.timeScale = slowdown ? slowedTimeSpeed : previousTimeSpeed;
    }
}