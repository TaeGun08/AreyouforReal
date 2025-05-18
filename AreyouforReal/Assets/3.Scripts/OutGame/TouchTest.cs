using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TouchTest : MonoBehaviour
{
    void Start()
    {
        TouchManager.Instance.OnTouchStart += HandleTouchStart;
        TouchManager.Instance.OnTouchMove += HandleTouchMove;
        TouchManager.Instance.OnTouchEnd += HandleTouchEnd;
    }

    void HandleTouchStart(TouchControl touch)
    {
        Debug.Log($"Touch Started at: {touch.position.ReadValue()}");
    }

    void HandleTouchMove(TouchControl touch)
    {
        Debug.Log($"Touch Moved to: {touch.position.ReadValue()}");
    }

    void HandleTouchEnd(TouchControl touch)
    {
        Debug.Log($"Touch Ended at: {touch.position.ReadValue()}");
    }

}
