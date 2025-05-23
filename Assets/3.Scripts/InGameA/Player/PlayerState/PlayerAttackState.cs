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
        RPC_Attacking();
        yield return new WaitForSeconds(1f);
        playerController.ChangeState(State.Idle);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RPC_Attacking()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitCollider.bounds.center, hitCollider.radius, 
            LayerMask.GetMask("Player"));
            
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player.Object.Id == playerController.Object.Id) continue;
                
                 player.RPC_PlayerKnockout();
                 
                // 킬로그용
                // player.RPC_PlayerKnockout(playerController.Object);

                break;
            }
        }
    }
    
    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}
