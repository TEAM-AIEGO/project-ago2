using UnityEngine;

public interface IKnockable
{
    public void TakeKnockback(Vector3 force, float duration);
    public void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration);
}
