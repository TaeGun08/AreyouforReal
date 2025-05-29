using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoutZone : MonoBehaviour
{
    [SerializeField] private bool checkZone;

    private void OnTriggerEnter(Collider other)
    {
        if (checkZone == false) return;
            
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("ZoneCheck")))
        {
            IKnockout knockout = other.gameObject.GetComponentInParent<IKnockout>();
            knockout.RPC_Knockout();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (checkZone) return;
        
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("ZoneCheck")))
        {
            IKnockout knockout = other.gameObject.GetComponentInParent<IKnockout>();
            knockout.RPC_Knockout();
        }
    }
}
