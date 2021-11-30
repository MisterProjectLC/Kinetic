using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    Vector3 MoveDirection;

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDirection * Time.deltaTime;
    }

    public void SetMoveDirection(Vector3 value)
    {
        MoveDirection = value;
    }
}
