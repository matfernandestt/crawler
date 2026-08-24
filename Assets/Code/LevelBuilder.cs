using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private LevelQuadrant levelQuadrantPrefab;

    [SerializeField] private Vector2 levelSize;
    [SerializeField] private int quadrantSize;
    
    private List<LevelQuadrant> _levelQuadrants = new();

    private void Start()
    {
        for (var x = 0; x < levelSize.x; x++)
        {
            for (var z = 0; z < levelSize.y; z++)
            {
                var newQuadrant = Instantiate(levelQuadrantPrefab, new Vector3(x * quadrantSize, 0, z * quadrantSize), Quaternion.identity, transform);
                newQuadrant.DisableAllWalls();
                _levelQuadrants.Add(newQuadrant);
                if (x == 0)
                {
                    newQuadrant.ActivateCortinateWall(Coordinate.West, true);
                }
                if(z == 0)
                {
                    newQuadrant.ActivateCortinateWall(Coordinate.South, true);
                }

                if (z == levelSize.y - 1)
                {
                    newQuadrant.ActivateCortinateWall(Coordinate.North, true);
                }
                if (x == levelSize.x - 1)
                {
                    newQuadrant.ActivateCortinateWall(Coordinate.East, true);
                }
            }
        }
    }
}
