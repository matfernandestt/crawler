using System;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    
    private PlayerReferences _refs;
    
    public int CurrentHealth { get; private set; }
    public int CurrentMana { get; private set; }

    private int _healthStepsRemaining;
    private int _manaStepsRemaining;

    private int _activeHealthPotionHealing;
    private int _activeManaPotionHealing;
    
    public PlayerData GetData => data;

    public Action onRefreshAttributes;
    public Action onProcessStep;
    
    private void Awake()
    {
        _refs = GetComponent<PlayerReferences>();
    }

    private void Start()
    {
        SetupData();
    }

    private void SetupData()
    {
        SetCurrentHealth(data.health);
        SetCurrentMana(data.mana);
    }

    public void SetCurrentHealth(int value)
    {
        CurrentHealth = value;
        onRefreshAttributes?.Invoke();
    }

    public void SetCurrentMana(int value)
    {
        CurrentMana = value;
        onRefreshAttributes?.Invoke();
    }

    public void ReplenishHealthAndMana(int replenishHealthValue, int replenishManaValue)
    {
        SetCurrentHealth(CurrentHealth + replenishHealthValue);
        SetCurrentMana(CurrentMana + replenishManaValue);
    }

    public void ProcessStep()
    {
        if(_healthStepsRemaining > 0)
        {
            SetCurrentHealth(CurrentHealth + _activeHealthPotionHealing);
            _healthStepsRemaining--;
        }
        if (_manaStepsRemaining > 0)
        {
            SetCurrentMana(CurrentMana + _activeManaPotionHealing);
            _manaStepsRemaining--;
        }
        onProcessStep?.Invoke();
    }

    public void AddHealthSteps(int steps, int totalHealth)
    {
        _healthStepsRemaining = steps;
        _activeHealthPotionHealing = totalHealth;
    }
    
    public void AddManaSteps(int steps, int totalMana)
    {
        _manaStepsRemaining = steps;
        _activeManaPotionHealing = totalMana;
    }
}
