using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class LevelSystem : IEcsInitSystem, IEcsRunSystem
{

    private float _spawnCycleTime;
    private EcsWorld _world;
    private Transform _playerTransform;

    public void Init()
    {
        EcsEntity player = InstantiatePhysicsObject(SettingsHolder.Default.playerSettings, SettingsHolder.Default.playerPrefab);
        Transform playerTransform = player.Get<TransformComponent>().transform;
        ParticleSystem leftForward = playerTransform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem rightForward = playerTransform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem rightBack = playerTransform.GetChild(2).GetComponent<ParticleSystem>();
        ParticleSystem leftBack = playerTransform.GetChild(3).GetComponent<ParticleSystem>();

        player.Replace(new InputComponent());
        player.Replace(new BoundsControllerComponent() { action = BoundsControllerComponent.ActionOnExit.Teleport });
        player.Replace(new WeaponComponent() { bulletDelay = 0.2f, laserDelay = 5f });
        player.Replace(new DeathComponent() 
        { 
            log = playerTransform.GetComponent<CollisionLog>(),
            deathTags = new List<string>(){ "Enemy" }
        });
        player.Replace(new TurboParticlesComponent()
        {
            forward = new List<ParticleSystem>() { rightBack, leftBack },
            left = new List<ParticleSystem>() { rightBack, leftForward },
            right = new List<ParticleSystem>() { rightForward, leftBack }
        });

        _playerTransform = player.Get<TransformComponent>().transform;
    }

    public void Run()
    {
        _spawnCycleTime += Time.deltaTime;
        if (_spawnCycleTime >= 1f)
        {
            _spawnCycleTime = 0f;
            if (Random.Range(0, 2) == 1)
            {
                if (Random.Range(0, 10) == 9 && _playerTransform)
                {
                    EcsEntity alien = InstantiatePhysicsObject(SettingsHolder.Default.alienSettings, SettingsHolder.Default.alienPrefab);
                    ParticleSystem forward = alien.Get<TransformComponent>().transform.GetChild(0).GetComponent<ParticleSystem>();
                    alien.Replace(new FollowComponent() { target = _playerTransform });
                    alien.Replace(new SpawnComponent() { spawnOutside = true });
                    alien.Replace(new DeathComponent()
                    {
                        log = alien.Get<TransformComponent>().transform.GetComponent<CollisionLog>(),
                        deathTags = new List<string>() { "Bullet", "Laser" }
                    });
                    alien.Replace(new TurboParticlesComponent()
                    {
                        forward = new List<ParticleSystem>() { forward }
                    });
                }
                else
                {
                    EcsEntity asteroid = InstantiatePhysicsObject(SettingsHolder.Default.asteroidSettings);
                    asteroid.Replace(new SpawnComponent() { spawnOutside = true, addSpeed = true, addAngularSpeed = true });
                    asteroid.Replace(new BoundsControllerComponent() { action = BoundsControllerComponent.ActionOnExit.Destroy });
                    asteroid.Replace(new SmallAsteroidSpawnComponent());
                    asteroid.Replace(new DeathComponent()
                    {
                        log = asteroid.Get<TransformComponent>().transform.GetComponent<CollisionLog>(),
                        deathTags = new List<string>() { "Bullet", "Laser" },
                    });
                }
            }
        }
    }

    private EcsEntity InstantiatePhysicsObject(PhysicsObjectSettings settings, GameObject prefab = null) 
    {
        EcsEntity entity = _world.NewEntity();
        GameObject spawnedObject = GameObject.Instantiate(prefab ? prefab : SettingsHolder.Default.basePrefab);
        spawnedObject.name = settings.name;
        spawnedObject.tag = settings.Tag;
        SpriteRenderer sprite = spawnedObject.GetComponent<SpriteRenderer>();
        sprite.sprite = settings.Sprite;
        spawnedObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        entity.Replace(new TransformComponent() { transform = spawnedObject.transform });
        entity.Replace(new SpriteComponent() { sprite = sprite });
        entity.Replace(new PhysicsComponent() { drag = settings.Drag, angularDrag = settings.AngularDrag });
        entity.Replace(new DeathParticleComponent());

        return entity;
    }
}
