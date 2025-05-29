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
        LayerMask layerMask = LayerMask.GetMask("Player") 
                              | LayerMask.GetMask("AI");
        
        Collider[] hitColliders = Physics.OverlapSphere(hitCollider.bounds.center, hitCollider.radius, 
            layerMask);
            
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                IKnockout networkObject = collider.GetComponent<IKnockout>();
                if (networkObject.NetworkObj.Id == playerController.NetworkObj.Id) continue;
                
                SoundManager.Instance.PlaySfx("maou_se_battle16"); //반복 x
                
                networkObject.RPC_Knockout();
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
