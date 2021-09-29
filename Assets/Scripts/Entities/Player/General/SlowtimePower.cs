using UnityEngine;
using UnityEngine.Events;

public class SlowtimePower : MonoBehaviour
{
    PlayerInputHandler input;
    bool slowdown = false;
    public UnityAction OnUpdate;

    public float JuiceLeft { get; private set; }

    [SerializeField]
    public float JuiceMax { get; private set; } = 10f;

    [SerializeField]
    float slowedTimeSpeed = 0.25f;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        setJuiceLeft(JuiceMax);
    }

    private void Update()
    {
        if (input.GetSlowtime())
            setSlowdown(!slowdown);

        if (slowdown)
            if (JuiceLeft > 0f)
                setJuiceLeft(JuiceLeft - Time.deltaTime/slowedTimeSpeed);
            else
                setSlowdown(false);


        else if (JuiceLeft < JuiceMax)
            setJuiceLeft(JuiceLeft + Time.deltaTime);
    }


    void setSlowdown(bool slowdown)
    {
        this.slowdown = slowdown;
        Time.timeScale = slowdown ? slowedTimeSpeed : 1f;
    }

    void setJuiceLeft(float juiceLeft)
    {
        JuiceLeft = juiceLeft;
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }
}