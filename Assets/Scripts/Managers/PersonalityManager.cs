using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class PersonalityManager : MonoBehaviour
{
    public AIPersonalityScriptableObject GetRandomPersonality(bool includeoverpowered = false)
    {
        List<AIPersonalityScriptableObject> possiblePersonalities = new List<AIPersonalityScriptableObject>();

        possiblePersonalities.Add(basePersonality);
        possiblePersonalities.Add(offensivePersonality);
        possiblePersonalities.Add(defensivePersonality);

        if (includeoverpowered)
        {
            possiblePersonalities.Add(overpoweredPersonality);
        }

        int randomIndex = Random.Range(0, possiblePersonalities.Count);
        return possiblePersonalities[randomIndex];
    }


    [SerializeField] private AIPersonalityScriptableObject basePersonality;
    [SerializeField] private AIPersonalityScriptableObject offensivePersonality;
    [SerializeField] private AIPersonalityScriptableObject defensivePersonality;
    [SerializeField] private AIPersonalityScriptableObject overpoweredPersonality;
}
