using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Sprite icon;
    public string enemyName;
    public int health;
    public int mana;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int evasion;
    public int accuracy;

    public Action OnDeath;

    private int _currentHealth;
    private int _currentMana;
    private int _currentAttack;
    private int _currentDefense;
    private int _currentSpecialAttack;
    private int _currentSpecialDefense;
    private int _currentEvasion;
    private int _currentAccuracy;

    public void InitializeEnemy()
    {
        _currentHealth = health;
        _currentMana = mana;
        _currentAttack = attack;
        _currentDefense = defense;
        _currentSpecialAttack = specialAttack;
        _currentSpecialDefense = specialDefense;
        _currentEvasion = evasion;
        _currentAccuracy = accuracy;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnDeath?.Invoke();
        }
    }
}
