using UnityEngine;

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField] private TestGun testGun;
    //[SerializeField] private Transform firePoint;

    private bool isFiring;

    private void Start()
    {
        
    }

    void Update()
    {
        if (isFiring)
            if (testGun.IsFireAble)
            {
                testGun.Fire();
            }
    }

    public void OnFire(bool isFiring)
    {
        this.isFiring = isFiring;
    }
}