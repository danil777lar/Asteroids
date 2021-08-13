using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class FollowSystem : IEcsRunSystem
{
    private EcsFilter<FollowComponent, PhysicsComponent, TransformComponent> _filter = null;

    public void Run() 
    {
        foreach (int i in _filter) 
        {
            ref FollowComponent follow = ref _filter.Get1(i);
            ref PhysicsComponent physics = ref _filter.Get2(i);
            ref TransformComponent trans = ref _filter.Get3(i);

            if (!follow.target) 
            {
                _filter.GetEntity(i).Del<FollowComponent>();
                continue;
            }


            if (_filter.GetEntity(i).Has<TurboParticlesComponent>()) 
            {
                List<ParticleSystem> parts = _filter.GetEntity(i).Get<TurboParticlesComponent>().forward;
                foreach (ParticleSystem part in parts)
                    part.Play();
            }
            Vector2 diff = (follow.target.position - trans.transform.position).normalized;
            float targetAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            trans.transform.rotation = Quaternion.Lerp(trans.transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);

            physics.speedToAdd += 10f * Time.deltaTime;
        }
    }
}
