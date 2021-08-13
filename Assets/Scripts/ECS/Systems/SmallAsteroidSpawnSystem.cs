using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class SmallAsteroidSpawnSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    private EcsFilter<DeathComponent, SmallAsteroidSpawnComponent, TransformComponent, PhysicsComponent> _filter = null;

    public void Run() 
    {
        foreach (int i in _filter)
        {
            ref DeathComponent deathComp = ref _filter.Get1(i);
            ref TransformComponent trans = ref _filter.Get3(i);
            ref PhysicsComponent physics = ref _filter.Get4(i);

            foreach (Collider2D collision in deathComp.log.collisions) 
            {
                if (deathComp.deathTags.Contains(collision.tag)) 
                {
                    int partsCount = Random.Range(2, 5);
                    for (int j = 0; j < partsCount; j++)
                    {
                        EcsEntity entity = _world.NewEntity();
                        GameObject spawnedObject = GameObject.Instantiate(SettingsHolder.Default.basePrefab);

                        SpriteRenderer sprite = spawnedObject.GetComponent<SpriteRenderer>();
                        sprite.sprite = SettingsHolder.Default.lilAsteroidSettings.Sprite;

                        Vector2 dir = physics.velocity.normalized;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                        angle += -45f + (90f / partsCount) * j;

                        spawnedObject.AddComponent<PolygonCollider2D>().isTrigger = true;
                        spawnedObject.name = SettingsHolder.Default.lilAsteroidSettings.name;
                        spawnedObject.tag = SettingsHolder.Default.lilAsteroidSettings.Tag;
                        spawnedObject.transform.position = trans.transform.position;
                        spawnedObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                        entity.Replace(new TransformComponent() { transform = spawnedObject.transform });
                        entity.Replace(new SpriteComponent() { sprite = sprite });
                        entity.Replace(new BoundsControllerComponent() { action = BoundsControllerComponent.ActionOnExit.Destroy });
                        entity.Replace(new DeathParticleComponent());
                        entity.Replace(new DeathComponent()
                        {
                            log = entity.Get<TransformComponent>().transform.GetComponent<CollisionLog>(),
                            deathTags = new List<string>() { "Bullet", "Laser" },
                        });
                        entity.Replace(new PhysicsComponent()
                        {
                            drag = SettingsHolder.Default.lilAsteroidSettings.Drag,
                            angularDrag = SettingsHolder.Default.lilAsteroidSettings.AngularDrag,
                            speedToAdd = Random.Range(5, 10),
                            angularSpeedToAdd = Random.Range(-720f, 720f)
                        });
                    }
                    return;
                }
            }
        }
    }
}
