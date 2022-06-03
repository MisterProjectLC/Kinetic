using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectReference", menuName = "References/GameObject")]
public class GameObjectReference : ScriptableObject
{
    [SerializeField]
    bool disabled;
    public bool Disabled { get { return disabled; } }
    public GameObject Reference;
}
