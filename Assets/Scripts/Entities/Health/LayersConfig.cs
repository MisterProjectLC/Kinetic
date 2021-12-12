using UnityEngine;

[CreateAssetMenu(fileName = "LayersConfig", menuName = "ScriptableObjects/LayersConfig", order = 2)]
public class LayersConfig : ScriptableObject
{
    public LayerMask layers;
}
