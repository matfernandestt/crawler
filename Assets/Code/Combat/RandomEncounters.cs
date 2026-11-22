using UnityEngine;

public class RandomEncounters : MonoBehaviour
{
    [SerializeField] private int numberOfStepsBeforeChanceEncounter;
    [SerializeField, Range(0, 100)] private int chanceEncounter;

    private int _currentSteps;
    
    private void Start()
    {
        PlayerReferences.Instance.attributes.onProcessStep += OnProcessStep;
    }

    private void OnDestroy()
    {
        PlayerReferences.Instance.attributes.onProcessStep -= OnProcessStep;
    }

    private void OnProcessStep()
    {
        var combatManager = CombatManager.Instance;
        if (combatManager.InCombat) return;
        
        _currentSteps++;
        if (_currentSteps >= numberOfStepsBeforeChanceEncounter)
        {
            var chance = Random.Range(0, 100);
            if (chance < chanceEncounter)
            {
                _currentSteps = 0;
                combatManager.EnterCombat();
            }
        }
    }
}
