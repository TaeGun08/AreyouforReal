using UnityEngine.InputSystem.Controls;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    private Vector2 swipeStart;
    private Vector2 swipeEnd;
    
    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f;
    
    private void Awake()
    {
        Instance = this;
    }

    public Action OnTouchStart;
    public Action OnTouchMove;
    public Action OnTouchEnd;

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                OnTouchStart?.Invoke();
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                OnTouchMove?.Invoke();
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended ||
                     touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                OnTouchEnd?.Invoke();
            }
        }
    }
    
    /*private void FixedUpdate()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
                HandleMouseInput();
        #else
                HandleTouchInput();
        #endif
    }*/
    
    private bool HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            Vector2 swipe = swipeEnd - swipeStart;
            if (swipe.magnitude < minSwipeDistance) return true;
        }
        return true;
    }
    
    private bool HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                swipeStart = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                swipeEnd = t.position;
                Vector2 swipe = swipeEnd - swipeStart;
                if (swipe.magnitude < minSwipeDistance) return true;;
            }
            
            return true;
        }
        
        return false;
    }
}