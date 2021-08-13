using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHolder : MonoBehaviour
{
    private static SettingsHolder _default;
    public static SettingsHolder Default => _default;

    public GameObject basePrefab;
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
    public GameObject deathParticlePrefab;
    public GameObject alienPrefab;

    public PhysicsObjectSettings playerSettings;
    public PhysicsObjectSettings alienSettings;
    public PhysicsObjectSettings asteroidSettings;
    public PhysicsObjectSettings lilAsteroidSettings;
    public PhysicsObjectSettings bulletSettings;

    private void Awake()
    {
        _default = this;
    }
}
