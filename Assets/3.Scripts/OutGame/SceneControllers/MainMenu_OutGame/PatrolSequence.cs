using System;
using UnityEngine;
using DG.Tweening;

public class PatrolSequence : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveDuration = 8f;
    [SerializeField] private float rotateDuration = 0.1f;

    private Vector3 startPoint;
    private bool isRightRotation = true;

    private void Awake()
    {
        startPoint = transform.position;
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }
    
    void Start()
    {
        Sequence patrolSequence = DOTween.Sequence().SetTarget(this).SetAutoKill(true);

        patrolSequence
            .Append(transform.DOMove(endPoint.position, moveDuration).SetEase(Ease.Linear))
            .AppendCallback(RotateY)
            .Append(transform.DOMove(startPoint, moveDuration).SetEase(Ease.Linear))
            .AppendCallback(RotateY)
            .SetLoops(-1); // 무한 반복
    }

    private void RotateY()
    {
        float angle = isRightRotation ? -90f : 90f  ;

        transform.DORotate(new Vector3(0f, angle, 0f), rotateDuration)
            .SetEase(Ease.InOutSine);

        isRightRotation = !isRightRotation;
    }
}