using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelExitSpawner : MonoBehaviour
{
    [SerializeField] private float minimumAcceptableRangeFromPlayer;
    [SerializeField] private GameObject[] exits;

    private void Awake()
    {
        PlayerReferences.OnPlayerReady += OnPlayerReady;
    }

    private void OnDestroy()
    {
        PlayerReferences.OnPlayerReady -= OnPlayerReady;
    }

    private void OnPlayerReady()
    {
        RandomizeExit();
    }

    private void RandomizeExit()
    {
        if (exits.Length == 0) return;
        var randomExit = Random.Range(0, exits.Length);
        while (Vector3.Distance(exits[randomExit].transform.position, PlayerReferences.Instance.transform.position) < minimumAcceptableRangeFromPlayer)
        {
            randomExit = Random.Range(0, exits.Length);
        }
        for (var i = 0; i < exits.Length; i++)
        {
            var exit = exits[i];
            exit.SetActive(i == randomExit);
        }
    }
}
