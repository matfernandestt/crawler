using System;
using UnityEngine;

public class LevelQuadrant : MonoBehaviour
{
    [SerializeField] private GameObject westWall;
    [SerializeField] private GameObject eastWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject northWall;

    public void DisableAllWalls()
    {
        westWall.SetActive(false);
        eastWall.SetActive(false);
        southWall.SetActive(false);
        northWall.SetActive(false);
    }

    public void ActivateCortinateWall(Coordinate coordinate, bool activeStatus)
    {
        switch (coordinate)
        {
            case Coordinate.North:
                northWall.SetActive(activeStatus);
                break;
            case Coordinate.East:
                eastWall.SetActive(activeStatus);
                break;
            case Coordinate.South:
                southWall.SetActive(activeStatus);
                break;
            case Coordinate.West:
                westWall.SetActive(activeStatus);
                break;
        }
    }
}

public enum Coordinate
{
    North,
    East,
    South,
    West
}
