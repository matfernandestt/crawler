using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Sprite icon;
    public Texture2D normalMap;
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
    
    public SkillEffectType[] immunities;
    
    public ItemData[] possibleDropsOnDefeat;

    [SerializeField, TextArea(15, 20)] private string description;
}
