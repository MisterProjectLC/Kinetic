using System.Collections.Generic;
using UnityEngine;


public class SavedLoadout
{
    [System.Serializable]
    public struct SavedAbility
    {
        public string name; public int loadout; public int slot; public bool inGame;
        public SavedAbility(string name, int loadout, int slot)
        {
            this.name = name; this.loadout = loadout; this.slot = slot; inGame = true;
        }

        public void SetInGame(bool t) { inGame = t; }
    }

    static List<SavedAbility> SpawnAbilities = new List<SavedAbility>();


    public static void AddSpawnAbility(SavedAbility ability)
    {
        SpawnAbilities.Add(ability);
    }

    public static void RemoveSpawnAbility(int loadoutNumber, int abilityNumber)
    {
        SpawnAbilities.RemoveAll(x => x.loadout == loadoutNumber && x.slot == abilityNumber);
    }

    public static void ClearSpawnAbilities()
    {
        SpawnAbilities.Clear();
    }

    public static void SetSpawnAbility(SavedAbility ability, int index)
    {
        SpawnAbilities[index] = ability;
    }

    public static int GetSpawnAbilityCount()
    {
        return SpawnAbilities.Count;
    }

    public static SavedAbility GetSpawnAbility(int index)
    {
        return SpawnAbilities[index];
    }

    public static SavedAbility GetSpawnAbility(string name)
    {
        return SpawnAbilities.Find(x => x.name == name);
    }

    public static int GetSpawnAbilityIndex(SavedAbility ability)
    {
        return SpawnAbilities.IndexOf(ability);
    }


    public static bool ExistsSpawnAbility(string name)
    {
        return SpawnAbilities.Exists(x => x.name == name);
    }
}
