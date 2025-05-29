using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;

    private Dictionary<string, AudioClip> bgmDict = new();

    [Header("Default Volumes")]
    [Range(0f, 1f)] public float defaultBgmVolume = 0.4f;

    private const string BgmVolumeKey = "BGM_VOLUME";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
        CacheClips();
    }

    private void LoadVolumes()
    {
        float bgmVol = PlayerPrefs.GetFloat(BgmVolumeKey, defaultBgmVolume);

        SetBgmVolume(bgmVol);
    }

    private void CacheClips()
    {
        foreach (var clip in bgmClips)
        {
            if (clip != null && !bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat(BgmVolumeKey, volume);
    }
    
    public void PlayBgm(string clipName, bool loop = true)
    {
        if (bgmDict.TryGetValue(clipName, out var clip))
        {
            if (bgmSource.clip != clip)
            {
                bgmSource.clip = clip;
                bgmSource.loop = loop;
                bgmSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"BGM clip not found: {clipName}");
        }
    }

    public void StopBgm()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }
}
