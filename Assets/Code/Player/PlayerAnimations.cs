using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private static readonly int AnimHash_Moving = Animator.StringToHash("Moving");
    private static readonly int AnimHash_Attack = Animator.StringToHash("Attack");
    private static readonly int AnimHash_TakeDamage = Animator.StringToHash("TakeDamage");

    public void SetMovement(bool isMoving)
    {
        anim.SetBool(AnimHash_Moving, isMoving);
    }

    public void SetAttack()
    {
        anim.SetTrigger(AnimHash_Attack);
    }
    
    public void SetTakeDamage()
    {
        anim.SetTrigger(AnimHash_TakeDamage);
    }
}
