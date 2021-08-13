using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TurboParticlesComponent
{
    public List<ParticleSystem> forward;
    public List<ParticleSystem> left;
    public List<ParticleSystem> right;

    public TurboParticlesComponent(int arg = 0) 
    {
        forward = new List<ParticleSystem>();
        left = new List<ParticleSystem>();
        right = new List<ParticleSystem>();
    }
}
