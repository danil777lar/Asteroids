using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class ECSLoader : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _updateSystems;
    private EcsSystems _fixedUpdateSystems;
    private EcsSystems _lateUpdateSystems;

    public void Load()
    {
        if (_world == null)
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world)
                .Add(new LevelSystem())
                .Add(new SpawnSystem())
                .Add(new InputSystem())
                .Add(new FollowSystem())
                .Add(new BoundsControllerSystem())
                .Add(new WeaponSystem())
                .Add(new SmallAsteroidSpawnSystem())
                .Add(new DeathParticleSystem());

            _fixedUpdateSystems = new EcsSystems(_world)
                .Add(new PhysicsSystem());

            _lateUpdateSystems = new EcsSystems(_world)
                .Add(new DeathSystem());

            _updateSystems.Init();
            _fixedUpdateSystems.Init();
            _lateUpdateSystems.Init();
        }
    }

    private void Update()
    {
        _updateSystems?.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void LateUpdate()
    {
        _lateUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        _updateSystems?.Destroy();
        _fixedUpdateSystems?.Destroy();
        _lateUpdateSystems?.Destroy();
        _world?.Destroy();
    }
}
