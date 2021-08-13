using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class SpawnSystem : IEcsRunSystem
{
    private EcsFilter<TransformComponent, SpawnComponent, PhysicsComponent> _filter = null;

    public void Run() 
    {
        foreach (int i in _filter) 
        {
            ref TransformComponent trans = ref _filter.Get1(i);
            ref SpawnComponent spawnInfo = ref _filter.Get2(i);
            ref PhysicsComponent physics = ref _filter.Get3(i);

            if (spawnInfo.spawnOutside) trans.transform.position = GetRandomPosition(false);
            else trans.transform.position = Camera.main.ViewportToWorldPoint(Vector2.one / 2);

            if (spawnInfo.addSpeed) 
            {
                Vector2 diff = ((Vector3)GetRandomPosition(true) - trans.transform.position).normalized;
                float targetAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
                trans.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
                physics.speedToAdd = 5f;
            }

            if (spawnInfo.addAngularSpeed)
                physics.angularSpeedToAdd = Random.Range(-360, 360);

            _filter.GetEntity(i).Del<SpawnComponent>();
        }
    }

    private Vector2 GetRandomPosition(bool inside) 
    {
        Vector2 cameraMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 cameraMax = Camera.main.ViewportToWorldPoint(Vector2.one);
        Vector2 pos = Vector2.zero;

        if (inside) 
        {
            pos.x = Random.Range(cameraMin.x, cameraMax.x);
            pos.y = Random.Range(cameraMin.y, cameraMax.y);
        }
        else 
        {
            if (Random.Range(0, 2) == 1)
            {
                pos.x = Random.Range(cameraMin.x, cameraMax.x);
                pos.y = Random.Range(0, 2) == 1 ? cameraMin.y - 1f : cameraMax.y + 1f;
            }
            else
            {
                pos.y = Random.Range(cameraMin.y, cameraMax.y);
                pos.x = Random.Range(0, 2) == 1 ? cameraMin.x - 1f : cameraMax.x + 1f;
            }
        }

        return pos;
    }
}
