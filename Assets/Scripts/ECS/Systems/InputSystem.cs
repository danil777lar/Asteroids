using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class InputSystem : IEcsRunSystem
{
    private EcsFilter<PhysicsComponent, InputComponent> _filter = null;
    private EcsFilter<WeaponComponent> _weaponFilter = null;
    private EcsFilter<TurboParticlesComponent, InputComponent> _turboFilter = null;

    public void Run() 
    {
        foreach (int i in _filter) 
        {
            ref PhysicsComponent physics = ref _filter.Get1(i);
            if (Input.GetKey("w")) physics.speedToAdd += 10f * Time.deltaTime;
            if (Input.GetKey("d")) physics.angularSpeedToAdd -= 360f * Time.deltaTime;
            if (Input.GetKey("a")) physics.angularSpeedToAdd += 360f * Time.deltaTime;
        }

        foreach (int i in _weaponFilter) 
        {
            ref WeaponComponent weapon = ref _weaponFilter.Get1(i);
            weapon.bulletKeyPressed = Input.GetKey("space");
            weapon.laserKeyPressed = Input.GetKey("f");
        }

        foreach (int i in _turboFilter) 
        {
            ref TurboParticlesComponent parts = ref _turboFilter.Get1(i);
            List<ParticleSystem> particlesAll = new List<ParticleSystem>();
            List<ParticleSystem> particlesToPlay = new List<ParticleSystem>();

            particlesAll.AddRange(parts.forward);
            particlesAll.AddRange(parts.left);
            particlesAll.AddRange(parts.right);

            if (Input.GetKey("w")) particlesToPlay.AddRange(parts.forward);
            if (Input.GetKey("a")) particlesToPlay.AddRange(parts.left);
            if (Input.GetKey("d")) particlesToPlay.AddRange(parts.right);

            foreach (ParticleSystem p in particlesAll) 
            {
                if (!particlesToPlay.Contains(p)) p.Stop();
                else if (!p.isPlaying) p.Play();
            }
        }
    }
}
