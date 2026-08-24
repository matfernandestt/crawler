using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementVector { get; private set; }
    public bool Strafing { get; private set; }
    public bool BlockMovement { get; private set; }
    
    private InputMap _input;
    private PlayerReferences _refs;
    
    private void Awake()
    {
        _refs = GetComponent<PlayerReferences>();

        _input = new InputMap();
        _input.Enable();
        
        _input.Player.Strafe.started += OnStrafeStart;
        _input.Player.Strafe.canceled += OnStrafeCanceled;
        _input.Player.DebugCombat.started += DebugCombat;
    }

    private void OnDestroy()
    {
        _input.Disable();
    }

    private void Update()
    {
        MovementVector = _input.Player.Move.ReadValue<Vector2>();
    }

    private void OnStrafeStart(InputAction.CallbackContext obj)
    {
        Strafing = true;
    }
    
    private void OnStrafeCanceled(InputAction.CallbackContext obj)
    {
        Strafing = false;
    }
    
    private void DebugCombat(InputAction.CallbackContext obj)
    {
        var combatManager = CombatManager.Instance;
        if (!combatManager.InCombat)
            combatManager.EnterCombat();
        // else
        //     combatManager.ExitCombat();
    }

    public void SetBlockMovement(bool blockMovement)
    {
        BlockMovement = blockMovement;
    }
}
