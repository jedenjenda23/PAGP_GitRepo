using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum factions { PlayerGroup, Civillist, Military, Mutant }
[CreateAssetMenu(menuName = "PAGP/CharacterPreset")]

public class CharacterPreset : ScriptableObject
{
  
    
    public enum genders { Male, Female, ApacheHellicopter }

    [Header("Character Basic Info")]
    public factions characterFaction;
    public factions[] rivalFactions;
    public genders characterGender;
    public Sprite characterImage;
    public string characterName;
    public int characterAge;
    public string characterDescription;


    public factions[] GetRivalFactions()
    {
        return rivalFactions;
    }

    public factions GetFaction()
    {
        return characterFaction;
    }
}
