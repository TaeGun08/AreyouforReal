using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override State CurrentState => State.Attack;
    
    [SerializeField] private SphereCollider hitCollider;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitCollider.bounds.center, hitCollider.radius, 
            LayerMask.GetMask("Player"));
            
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (Object.HasInputAuthority) continue;
                collider.GetComponent<PlayerController>().PlayerKnockout();
                break;
            }
        }
        
        yield return new WaitForSeconds(1f);
        playerController.ChangeState(State.Idle);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}
