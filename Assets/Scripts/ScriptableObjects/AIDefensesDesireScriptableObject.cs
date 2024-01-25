using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Defenses Desire", menuName = "Desire/DefensesDesireObject")]
public class AIDefensesDesireScriptableObject : ScriptableObject
{
    public float CalculateDesire(int amountofDozers, int existingTurrets, int enemyUnits)
    {
        if (amountofDozers == 0) return 0;
        return (1f / (existingTurrets + 1)) * (enemyUnits + 1);
    }
}
