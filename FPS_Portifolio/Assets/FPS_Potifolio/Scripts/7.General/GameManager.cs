using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType { EffectAudio, MusicAudio, VoiceAudio};
public class GameManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static GameManager Instance;
    public void Awake() => Instance = this;
    #endregion

    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    public AudioSource VoiceSource;

    public void PlayShootSound(AudioClip ClipToPlay, float MinValue, float MaxValue)
    {
        EffectsSource.volume = Random.Range(0.8f, 1f); EffectsSource.pitch = Random.Range(MinValue, MaxValue); EffectsSource.PlayOneShot(ClipToPlay);
    }
}