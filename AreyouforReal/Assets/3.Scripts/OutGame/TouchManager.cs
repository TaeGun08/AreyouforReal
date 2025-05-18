using UnityEngine.InputSystem.Controls;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Action<TouchControl> OnTouchStart;
    public Action<TouchControl> OnTouchMove;
    public Action<TouchControl> OnTouchEnd;

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                OnTouchStart?.Invoke(touch);
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                OnTouchMove?.Invoke(touch);
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended ||
                     touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                OnTouchEnd?.Invoke(touch);
            }
        }
    }

    private void OnMouseDown()
    {
        throw new NotImplementedException();
    }
}