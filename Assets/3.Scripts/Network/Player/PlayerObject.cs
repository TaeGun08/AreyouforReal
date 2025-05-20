using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using UnityEngine;

// 여기서 움직임제어
public class PlayerObject : NetworkBehaviour
{
    public static PlayerObject Local { get; private set; }
    
    [Header("References")]
    [SerializeField] private NetworkCharacterController characterController;
    
    [Space]
    
    [Header("Player")]
    [SerializeField] private float speed = 5f;
    
    private ChangeDetector changeDetector;
    private Vector3 forward = Vector3.forward;
    
    public override void Spawned()
    {
        // [Network]통해 변경 감지해서 동기화 해주는 역할
        // 아래 에러 상관없음 [Network] 속성이 하나도 없다고 경고 뜨는것
        // Change detector cannot be bound to a behaviour with zero network properties.
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        
        // Host처리
        if (Object.HasStateAuthority)
        {
            //PlayerRegistry.Instance.AddPlayer(Runner, Object.InputAuthority, this);
        }
        
        // Client처리
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PlayerInputData data))
        {
            if (data.Direction.sqrMagnitude > 0f)
            {
                forward = data.Direction;
            }
            
            data.Direction.Normalize();
            characterController.Move(5f * data.Direction * Runner.DeltaTime);
        }
    }
}
