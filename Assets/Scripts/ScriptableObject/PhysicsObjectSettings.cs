using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PhysicsObject")]
public class PhysicsObjectSettings : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _drag;
    [SerializeField] private float _angularDrag;
    [SerializeField] private string _tag;

    public Sprite Sprite => _sprite;
    public float Drag => _drag;
    public float AngularDrag => _angularDrag;
    public string Tag => _tag;
}
