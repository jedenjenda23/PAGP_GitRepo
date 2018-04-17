using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterCard : MonoBehaviour
{
    CharacterPreset myPreset;
    public Image characterImageUI;
    public Text characterNameUI;
    public Text characterDescriptionUI;
    public Text charConditionUI;
    public Text charHpUI;
    public Text charStrengthUI;
    public Text charNutritionUI;

    public void LoadCharacterGraphics(CharacterAttributes loadedAttributes)
    {
        CharacterPreset loadedPreset = loadedAttributes.characterPreset;
        myPreset = loadedPreset;

        characterImageUI.sprite = myPreset.characterImage;
        characterNameUI.text = myPreset.characterName;
        characterDescriptionUI.text = myPreset.characterDescription;

        charConditionUI.text = "Condition: " + loadedAttributes.condition;
        charHpUI.text = "Health: " + loadedAttributes.hp;
        charStrengthUI.text = "Strenght: " + loadedAttributes.strength;
        charNutritionUI.text = "Nutrition: " + loadedAttributes.nutrition;
    }
}
