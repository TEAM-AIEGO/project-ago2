using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField] private TestGun testGun;
    [SerializeField] private SubWeapon subWeapon;
    //[SerializeField] private Transform firePoint;

    [SerializeField] private CameraShake cameraShake;

    private UnityEvent subWeaponUsingComplete;

    private bool isFirable;
    private bool isFiring;

    private void Awake()
    {
        if (testGun == null)
        {
            Debug.LogError("testGun is Null");
        }

        if (subWeapon == null)
        {
            Debug.LogError("subWeapon is Null");
        }

        isFirable = true;

        Initialized();
    }

    public void Initialized()
    {
        subWeaponUsingComplete = new UnityEvent();
        subWeapon.Initialize(subWeaponUsingComplete);
        subWeaponUsingComplete.AddListener(UsingComplete);
    }

    private void Update()
    {
        if (isFirable)
            if (isFiring)
                if (testGun.IsFireAble)
                {
                    testGun.Fire();
                    cameraShake?.AddRecoil(new Vector2(Random.Range(-0.025f, 0.025f), 0.1f));
                }
    }

    public void Fire(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    public void UseSubWeapon()
    {
        if (!subWeapon.IsReady) 
            return;

        //���߿� �� �߻�ȭ�� �ϼ��Ǹ� ���� �� ��.
        testGun.gameObject.SetActive(false);
        isFirable = false;
        
        subWeapon.Use();
    }

    private void UsingComplete()
    {
        testGun.gameObject.SetActive(true);
        isFirable = true;
    }
}