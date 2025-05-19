using Fusion;
using UnityEngine;

namespace DefaultNamespace
{
    public struct PlayerInputData : INetworkInput
    {
        public const byte ATTACK_BUTTON = 1;
        
        public readonly NetworkButtons Buttons;
        public Vector3 Direction;
    }
}