using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class DeathParticleSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    private EcsFilter<DeathParticleComponent, DeathComponent, TransformComponent, SpriteComponent, PhysicsComponent> _filter = null;

    public void Run()
    {
        foreach (int i in _filter)
        {
            ref DeathComponent deathComp = ref _filter.Get2(i);
            ref TransformComponent trans = ref _filter.Get3(i);
            ref SpriteComponent sprite = ref _filter.Get4(i);
            ref PhysicsComponent physics = ref _filter.Get5(i);

            foreach (Collider2D collision in deathComp.log.collisions)
            {
                if (deathComp.deathTags.Contains(collision.tag))
                {
                    GameObject spawnedObject = GameObject.Instantiate(SettingsHolder.Default.deathParticlePrefab);
                    spawnedObject.transform.position = trans.transform.position;
                    ParticleSystem particleSystem = spawnedObject.GetComponent<ParticleSystem>();

                    List<Vector2> pixels = GetPixelPositions(sprite.sprite.sprite.texture, trans.transform);
                    particleSystem.Emit(pixels.Count);
                    particleSystem.Play();
                    ParticleSystem.Particle[] parts = new ParticleSystem.Particle[pixels.Count];
                    particleSystem.GetParticles(parts);

                    Vector2 nearestPoint = pixels[0];
                    for (int p = 0; p < pixels.Count; p++)
                    {
                        float nearestDistance = Vector2.Distance(collision.transform.position, nearestPoint);
                        float curentDistance = Vector2.Distance(collision.transform.position, pixels[p]);
                        if (curentDistance < nearestDistance)
                            nearestPoint = pixels[p];
                    }

                    for (int p = 0; p < pixels.Count; p++) 
                    {
                        parts[p].position = pixels[p];
                        parts[p].velocity = physics.velocity;

                        Vector2 explode = (Vector2)parts[p].position - nearestPoint;
                        Vector2 explodeVelocity = explode.normalized * Random.Range(0.5f, 1.5f);
                        if (explode.magnitude < 0.5f)
                            parts[p].velocity = explodeVelocity;
                        else
                            parts[p].velocity += (parts[p].position - trans.transform.position).normalized * Random.Range(0f, 0.5f);
                    }
                    particleSystem.SetParticles(parts);

                    GameObject.Destroy(spawnedObject, particleSystem.main.duration);
                }
            }
        }
    }

    private List<Vector2> GetPixelPositions(Texture2D texture, Transform trans)  
    {
        List<Vector2> pixels = new List<Vector2>();

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y).a != 0f) 
                {
                    Vector2 pos = Vector2.zero;
                    pos.x += (1f / 16f) * x - 0.5f;
                    pos.y += (1f / 16f) * y - 0.5f;
                    pixels.Add(trans.TransformPoint(pos));
                }
            }
        }
        return pixels;
    }
}
