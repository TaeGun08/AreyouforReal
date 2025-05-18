using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private float loadDuration = 2f;

    void Start()
    {
        StartLoading();
    }

    public void StartLoading()
    {
        slider.value = 0;
        DOTween.To(() => slider.value, x => slider.value = x, 1f, loadDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(UpdateProgressText)
            .OnComplete(OnLoadingComplete);
    }

    private void UpdateProgressText()
    {
        int progress = Mathf.RoundToInt(slider.value * 100);
        progressText.text = $"{progress}%";
    }

    private void OnLoadingComplete()
    {
        Debug.Log("Loading Complete!");
        progressText.text = "Complete!";
    }
}
