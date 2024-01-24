using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Defenses Desire", menuName = "Desire/DefensesDesireObject")]
public class AIDefensesDesireScriptableObject : ScriptableObject
{
    public float CalculateDesire(int existingTurrets, int enemyUnits, AIPersonalityScriptableObject personality = null)
    {
        float desire = 0f;
        if (existingTurrets < enemyUnits * 0.5f)
        {
            desire = existingTurrets * enemyUnits * 0.01f * (personality != null ? personality.defensivenessModifier : 1);
                    
        }
        return desire;
    }
}
