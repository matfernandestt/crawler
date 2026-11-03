using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        if (spawnPoints.Length == 0) return;
        var randomPoint =  spawnPoints[Random.Range(0, spawnPoints.Length)];
        PlayerReferences.Instance.movement.Teleport(randomPoint.position, randomPoint.rotation);
        PlayerReferences.OnPlayerReady?.Invoke();
    }
}
