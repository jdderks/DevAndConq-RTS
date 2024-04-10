using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalitySetter : MonoBehaviour
{
    [SerializeField] private AIPersonalityScriptableObject selectedPersonality;

    [SerializeField] private AIPersonalityScriptableObject agressivePersonality;
    [SerializeField] private AIPersonalityScriptableObject defensivePersonality;


    public void SetAgressivePersonality()
    {
        PlayerPrefs.SetFloat("Aggressiveness", agressivePersonality.aggressivenessModifier);
        PlayerPrefs.SetFloat("Defensiveness", agressivePersonality.defensivenessModifier);
        PlayerPrefs.Save();
    }
    
    public void SetDefensivePersonality()
    {
        PlayerPrefs.SetFloat("Aggressiveness", defensivePersonality.aggressivenessModifier);
        PlayerPrefs.SetFloat("Defensiveness", defensivePersonality.defensivenessModifier);
        PlayerPrefs.Save();
    }
}
