using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public EnemyData[] database;

    public EnemyData GetRandomEnemy()
    {
        return database[Random.Range(0, database.Length)];
    }
}
