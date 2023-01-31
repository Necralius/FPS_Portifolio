using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType { HitPart, DecalPart};
[Serializable]
public class GunParticle
{
    public GameObject HitParticle;
    public GameObject ImpactDecal;
    public string SurfaceTag;
}