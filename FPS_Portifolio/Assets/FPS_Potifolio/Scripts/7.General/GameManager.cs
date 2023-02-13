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

    [Header("Main Game Elements Storage")]
    public GameObject crossHair;
    public GameObject gunName;
    public GameObject gunState;

    public void PlayShootSound(AudioClip ClipToPlay, float MinValue, float MaxValue)
    {
        EffectsSource.volume = Random.Range(0.8f, 1f); EffectsSource.pitch = Random.Range(MinValue, MaxValue); EffectsSource.PlayOneShot(ClipToPlay);
    }
    public void PlaySound(AudioClip AudioToPlay, AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.EffectAudio: EffectsSource.PlayOneShot(AudioToPlay);
                break;
            case AudioType.MusicAudio: MusicSource.PlayOneShot(AudioToPlay);
                break;
            case AudioType.VoiceAudio: VoiceSource.PlayOneShot(AudioToPlay);
                break;
        }
    }
}