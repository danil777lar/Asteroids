using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class WeaponSystem : IEcsRunSystem
{
    private EcsFilter<WeaponComponent, TransformComponent> _filter = null;
    private EcsWorld _world = null;

    public void Run()
    {
        foreach (int i in _filter)
        {
            ref WeaponComponent weapon = ref _filter.Get1(i);
            ref TransformComponent trans = ref _filter.Get2(i);

            weapon.bulletLastShoot += Time.deltaTime;
            weapon.laserLastShoot += Time.deltaTime;

            if (weapon.bulletLastShoot >= weapon.bulletDelay && weapon.bulletKeyPressed)
            {
                weapon.bulletLastShoot = 0f;
                SpawnBullet(trans.transform);
            }
        }
    }

    private void SpawnBullet(Transform parentTransform) 
    {
        EcsEntity entity = _world.NewEntity();
        GameObject spawnedObject = GameObject.Instantiate(SettingsHolder.Default.bulletPrefab);
        spawnedObject.name = SettingsHolder.Default.bulletSettings.name;
        spawnedObject.tag = SettingsHolder.Default.bulletSettings.Tag;
        spawnedObject.transform.position = parentTransform.position;
        spawnedObject.transform.rotation = parentTransform.rotation;
        SpriteRenderer sprite = spawnedObject.GetComponent<SpriteRenderer>();
        sprite.sprite = SettingsHolder.Default.bulletSettings.Sprite;
        spawnedObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        entity.Replace(new TransformComponent() { transform = spawnedObject.transform });
        entity.Replace(new SpriteComponent() { sprite = sprite });
        entity.Replace(new BoundsControllerComponent() { action = BoundsControllerComponent.ActionOnExit.Destroy });
        entity.Replace(new PhysicsComponent()
        {
            drag = SettingsHolder.Default.bulletSettings.Drag,
            angularDrag = SettingsHolder.Default.bulletSettings.AngularDrag,
            speedToAdd = 25f
        });
        entity.Replace(new DeathComponent()
        {
            log = entity.Get<TransformComponent>().transform.GetComponent<CollisionLog>(),
            deathTags = new List<string>() { "Enemy" }
        });
    }
}
