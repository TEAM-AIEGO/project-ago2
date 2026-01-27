using UnityEngine;

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField] private TestGun testGun;
    //[SerializeField] private Transform firePoint;

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnFire(bool isFiring)
    {
        if (testGun.IsFireAble)
        {
            testGun.Fire();
        }
    }
}