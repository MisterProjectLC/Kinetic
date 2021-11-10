using UnityEngine;

public class LoadoutSlot : MonoBehaviour
{
    public int AbilityNumber = 0;
    public int LoadoutNumber = 0;

    public LoadoutSlot NextSlot;

    public void SetAbility(GameObject ability, LoadoutSlot slot, bool activating)
    {
        if (LoadoutNumber >= 0)
            ActorsManager.Player.GetComponent<LoadoutManager>().SetAbility(activating ? ability : null, slot.LoadoutNumber, slot.AbilityNumber);
        else
            ActorsManager.Player.GetComponent<LoadoutManager>().SetPassive(ability, activating);
    }
}
