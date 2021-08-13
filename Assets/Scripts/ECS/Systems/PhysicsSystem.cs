using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class PhysicsSystem : IEcsRunSystem
{
    private EcsFilter<TransformComponent, PhysicsComponent> _filter = null;

    public void Run() 
    {
        foreach (int i in _filter)
        {
            AddSpeed(i);
            CalculatePhysics(i);
        }
    }

    private void CalculatePhysics(int i) 
    {
        ref TransformComponent trans = ref _filter.Get1(i);
        ref PhysicsComponent physics = ref _filter.Get2(i);

        physics.velocity = physics.velocity * (1f - Time.fixedDeltaTime * physics.drag);
        physics.angularVelocity = physics.angularVelocity * (1f - Time.fixedDeltaTime * physics.angularDrag);

        trans.transform.position += (Vector3)physics.velocity * Time.fixedDeltaTime;
        trans.transform.rotation *= Quaternion.Euler(0f, 0f, physics.angularVelocity * Time.fixedDeltaTime);
    }

    private void AddSpeed(int i)
    {
        ref TransformComponent trans = ref _filter.Get1(i);
        ref PhysicsComponent physics = ref _filter.Get2(i);

        physics.velocity += (Vector2)trans.transform.TransformDirection(Vector2.up) * physics.speedToAdd;
        physics.speedToAdd = 0f;
        physics.angularVelocity += physics.angularSpeedToAdd;
        physics.angularSpeedToAdd= 0f;
    }
}
