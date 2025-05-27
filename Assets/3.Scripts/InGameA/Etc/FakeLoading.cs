using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private float loadDuration = 2f;

    private void OnEnable()
    {
        StartLoading();
    }

    private async Task StartLoading()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        
        slider.value = 0;
        DOTween.To(() => slider.value, x => slider.value = x, 1f, loadDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(UpdateProgressText)
            .OnComplete(() =>
            {
                progressText.text = "Complete!";
                tcs.SetResult(true);
            });
        
        await tcs.Task;
    }
    
    private void UpdateProgressText()
    {
        int progress = Mathf.RoundToInt(slider.value * 100);
        progressText.text = $"{progress}%";
    }
}
