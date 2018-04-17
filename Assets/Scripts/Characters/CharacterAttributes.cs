using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public CharacterPreset characterPreset;

    [Header("Basic stats")]
    [Range(1, 20)]
    public int strength = 10;
    public int maxHp;
    public float maxDmg;
    public int maxInventorySlots;
    public float baseSprintDuration;
    [Range(0.01f,2)]
    public float baseWeaponControl;
    [Range(0.01f, 100)]
    public float baseVisibility;            //Range of detection by enemies
    [Range(0.01f, 100)]
    public float baseVision;                //Range of LineOfSight

    [Header("Visible stats")]
    public int hp;
    public int dmg;
    public int inventorySlots;
    public float sprintDuration;
    public float weaponControl;
    public float nutrition = 5;

    [Header("__")]
    public float condition = 1f;
    public float wound;
    public float woundTreatment;
    public float disease;
    public float illnes;
    public float illnesTreatment;

    private void Start()
    {
        RecalculateStats();
    }
    public void RecalculateStats()
    {
        //Basic stats
        maxHp = strength * 10;
        maxDmg = strength * 2;
        maxInventorySlots =  strength * 1;

        //****************Visible stats*************************
        //Condition
        disease = illnes * 0.3f;

        //wounds
        //wound = maxHp - hp;
        wound = 1 - (hp / maxHp);


        //condition = 1 - (wound - (woundTreatment - disease));
         condition = 1 - wound;





        //Attributes
        hp = Mathf.RoundToInt(maxHp * condition);
        dmg = Mathf.RoundToInt(maxDmg * condition);
        inventorySlots = Mathf.RoundToInt(maxInventorySlots * (condition + (0.5f * (1 - condition))));
        sprintDuration = sprintDuration * (condition + (0.5f * (1 - condition)));
        weaponControl = baseWeaponControl * (condition + (0.5f * (1 - condition)));



        //testing nutrition
        nutrition = 5f;
    }

    public void CalculateIllnes(float locationToxicity, int numOfHits)
    {
        illnes = (illnes + locationToxicity + numOfHits) - illnesTreatment;
    }
}
