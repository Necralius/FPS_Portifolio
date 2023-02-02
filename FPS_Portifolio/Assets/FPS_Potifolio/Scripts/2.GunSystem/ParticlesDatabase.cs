using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Main ParticlesDatabase", menuName = "Portifolio_FPS/Databases/Create ParticleDatabase")]
public class ParticlesDatabase : ScriptableBaseModel
{
    public List<GunParticle> particles;
    public List<DefaultParticle> defaultParticles;
}