using System;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    
    private PlayerReferences _refs;
    
    public int CurrentHealth { get; private set; }
    public int BonusHealth { get; private set; }
    
    public int CurrentAccuracy { get; private set; }
    public int BonusSpeed { get; private set; }
    
    public int CurrentEvasion { get; private set; }
    public int BonusEvasion { get; private set; }
    
    public PlayerData GetData => data;

    public Action OnRefreshHealth;
    public Action OnDeath;
    
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
        CurrentHealth = data.health;
        CurrentAccuracy = data.accuracy;
        CurrentEvasion = data.evasion;
    }

    public void TakeDamage(int damage)
    {
        if (BonusHealth >= damage)
        {
            BonusHealth -= damage;
        }
        else
        {
            var remainingDamage = damage - BonusHealth;
            BonusHealth = 0;
            CurrentHealth = Mathf.Max(0, CurrentHealth - remainingDamage);
        }

        if (CurrentHealth > 0)
            OnRefreshHealth?.Invoke();
        else
            OnDeath?.Invoke();
    }

    public void AddBonusEvasion(int addedEvasion)
    {
        BonusEvasion += addedEvasion;
    }
}
