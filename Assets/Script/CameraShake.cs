using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float applySpeed;
    [SerializeField] private float returnSpeed;

    private Vector2 currentRecoil;
    private Vector2 targetRecoil;

    void Update()
    {
        targetRecoil = Vector2.Lerp(targetRecoil, Vector2.zero, returnSpeed * Time.deltaTime);

        currentRecoil = Vector2.Lerp(currentRecoil, targetRecoil, applySpeed * Time.deltaTime);
    }

    public void AddRecoil(Vector2 recoil)
    {
        targetRecoil += recoil;
    }

    public Vector2 GetRecoil()
    {
        return currentRecoil;
    }
}
