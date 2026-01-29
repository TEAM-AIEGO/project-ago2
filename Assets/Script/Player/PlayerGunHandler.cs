using UnityEngine;

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField] private TestGun testGun;
    [SerializeField] private SubWeapon subWeapon;
    //[SerializeField] private Transform firePoint;

    private bool isFirable;
    private bool isFiring;

    private void Start()
    {
        if (testGun == null)
        {
            Debug.LogError("testGun is Null");
        }

        //if (subWeapon == null)
        //{
        //    Debug.LogError("subWeapon is Null");
        //}

        isFirable = true;
    }

    void Update()
    {
        if (isFirable)
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

    public void OnUseSubWeapon()
    {
        //나중에 총 추상화가 완성되면 수정 할 것.
        testGun.gameObject.SetActive(false);
        isFirable = false;

        subWeapon.Use();
    }
}