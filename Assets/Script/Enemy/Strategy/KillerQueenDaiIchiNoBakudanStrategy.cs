using System;
using UnityEngine;

public class KillerQueenDaiIchiNoBakudanStrategy : IMUDAMUDAMUDAStrategy
{
    private event Action<Vector3, Vector3, float> OnLaunchProjectile;
    private Transform launch;
    private float damage;
    private float flightTime;

    public KillerQueenDaiIchiNoBakudanStrategy(float flightTime, float damage, Transform launch, Action<Vector3, Vector3, float> action)
    {
        this.flightTime = flightTime;
        this.damage = damage;
        this.launch = launch;
        OnLaunchProjectile += action;
    }

    public void Attacking(EnemyBase enemy, Transform target)
    {
        Vector3 targetPos = target.position;

        Vector3 g = Physics.gravity;

        Vector3 toTarget = targetPos - launch.position;

        Vector3 vel = (toTarget - 0.5f * g * flightTime * flightTime) / flightTime;

        OnLaunchProjectile?.Invoke(launch.position, vel, damage);
    }
}