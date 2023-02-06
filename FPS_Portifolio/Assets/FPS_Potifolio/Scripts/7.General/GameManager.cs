using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

public struct CustomTransformSave
{
    private Vector3 Position;
    private Vector3 Scale;
    private Vector3 Rotation;

    public CustomTransformSave(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Scale = scale;
    }

    public CustomTransformSave(GameObject objectToPass)
    {
        this.Position = objectToPass.transform.position;
        this.Rotation = objectToPass.transform.rotation.eulerAngles;
        this.Scale = objectToPass.transform.localScale;
    }
    public void SetPosition(GameObject objectToSet, CustomTransformSave positionSave)
    {
        objectToSet.transform.position = positionSave.Position;
        objectToSet.transform.rotation = Quaternion.Euler(positionSave.Rotation);
        objectToSet.transform.localScale = positionSave.Scale;
    }
}