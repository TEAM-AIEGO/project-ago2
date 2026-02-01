using UnityEngine;

public class IGermanysTechnologyStrategy : IMUDAMUDAMUDAStrategy
{
    public void Attacking(EnemyBase enemy, Transform target)
    {
        // Germany's Technology specific attack implementation
        // For example, it could shoot projectiles or use a special ability
        Debug.Log("Germany's Technology is attacking the target!");
    }

}