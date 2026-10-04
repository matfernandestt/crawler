using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public Sprite sprite;
    public string playerName;
    
    public int health;
    public int mana;

    public int attack;
    public int defense;

    public int specialAttack;
    public int specialDefense;

    public int evasion;
    public int accuracy;
    public int speed;

    public SkillData[] skills;
}