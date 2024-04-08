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
        selectedPersonality = agressivePersonality;
    }
    
    public void SetDefensivePersonality()
    {
        selectedPersonality = defensivePersonality;
    }
}
