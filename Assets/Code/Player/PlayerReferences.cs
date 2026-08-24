using System;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public PlayerInput input;
    public PlayerMovement movement;
    public PlayerAnimations animations;
    public PlayerAttributes attributes;
    public Camera cam;
    
    public static PlayerReferences Instance;

    private void Awake()
    {
        Instance = this;
    }
}
