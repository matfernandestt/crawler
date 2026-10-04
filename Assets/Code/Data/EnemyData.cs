using UnityEngine;

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
    public int speed;

    public SkillData[] skills;
}
