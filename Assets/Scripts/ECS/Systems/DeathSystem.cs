using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Leopotam.Ecs;

public class DeathSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    private EcsFilter<DeathComponent, TransformComponent> _filter = null;

    public void Run()
    {
        foreach (int i in _filter)
        {
            ref DeathComponent trigger = ref _filter.Get1(i);
            ref TransformComponent trans = ref _filter.Get2(i);

            if (!trigger.enabled) 
            {
                trigger.enabled = true;
                continue;
            }

            foreach (Collider2D collision in trigger.log.collisions)
            {
                if (!collision) continue;
                if (trigger.deathTags.Contains(collision.tag))
                {
                    string selfTag = trans.transform.gameObject.tag;
                    GameObject.Destroy(trans.transform.gameObject);
                    _filter.GetEntity(i).Destroy();
                    if (selfTag == "Player")
                        UIManager.Default.CurentState = UIManager.State.End;

                    break;
                }
            }
            trigger.log?.collisions.Clear();
        }
    }
}
