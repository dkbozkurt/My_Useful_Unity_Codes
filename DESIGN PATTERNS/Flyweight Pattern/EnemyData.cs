// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// Flyweight Design Pattern
///
/// Ref: https://www.youtube.com/watch?v=fwgkEpxUifQ&t=58s
/// </summary>

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    private float _maxSpeed = 10f;
    private float _attackSpeed = 40f;
    private float _attackDamage = 10f;
    private float _attackMinInterval = 3f;

    private int _maxHp = 100;

    public float MaxSpeed => _maxSpeed;
    public float AttackSpeed => _attackSpeed;
    public float AttackDamage => _attackDamage;
    public float AttackMinInterval => _attackMinInterval;
    public int MaxHp => _maxHp;
}
