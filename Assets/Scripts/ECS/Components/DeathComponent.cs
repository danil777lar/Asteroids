using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public struct DeathComponent
{
    public bool enabled;
    public CollisionLog log;
    public List<string> deathTags;
}
