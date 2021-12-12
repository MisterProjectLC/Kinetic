using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "ScriptableObjects/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    public float MoveSpeed = 5f;
    public float GravitySpeed = 0f;
    public float MaxLifetime = 2f;
    public float Radius = 20f;
}
