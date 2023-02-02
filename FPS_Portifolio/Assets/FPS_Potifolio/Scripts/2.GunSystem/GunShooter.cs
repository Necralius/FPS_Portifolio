using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ShootType { AutoOrSemi, BurstShot, Shotgun};

[CreateAssetMenu(fileName = "New Gun Asset", menuName = "Portifolio_FPS/Items/Handables/New Gun Asset")]
[Serializable]
public class GunShooter : ScriptableObject
{
    public ShootType shootType;

    public float ShootDamage;
    public float RateOfFire;

    public float ShootRange;

    public GameObject[] MuzzleFlash = new GameObject[2];
    public RaycastHit hit;
    public LayerMask EnemyMask;

    #region - Another Gun Systems -
    [HideInInspector] public float ShootsPerTap;
    [HideInInspector] public float ShootSpread;
    #endregion

    public ParticlesDatabase particlesDatabase;
    public GameObject[] GetParticles(string SurfaceToCheck)
    {
        GameObject[] selectedParticles = new GameObject[2];
        foreach (GunParticle particle in particlesDatabase.particles)
        {
            if (particle.SurfaceTag == SurfaceToCheck)
            {
                selectedParticles[0] = particle.HitParticle; selectedParticles[1] = particle.ImpactDecal;
                return selectedParticles;
            }
            else continue;
        }
        return null;
    }
    public void InstantiateSelectedParticles(string SurfaceTag, Vector3 hitPosition, Vector3 rotation)
    {
        GameObject[] selectedParticles = GetParticles(SurfaceTag);

        Instantiate(selectedParticles[0], hitPosition, Quaternion.LookRotation(rotation));
        Instantiate(selectedParticles[1], hitPosition, Quaternion.LookRotation(rotation));
    }

    public void InstatiateParticles(string ParticleTag, Vector3 hitPosition, Vector3 rotation)
    {
        GameObject[] selectedParticles = null;

        foreach(DefaultParticle particle in particlesDatabase.defaultParticles) if (particle.ParticleTag == ParticleTag) selectedParticles = particle.ParticlesPack;

        Instantiate(selectedParticles[Random.Range(0, selectedParticles.Length)], hitPosition, Quaternion.LookRotation(rotation));
    }
}