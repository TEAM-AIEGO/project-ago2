using UnityEngine;
using UnityEngine.Events;

public class PlayerGunHandler : MonoBehaviour, IWarpObserver
{
    [SerializeField] private Gun gun;
    [SerializeField] private SubWeapon[] subWeapons;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private WarpSystemManager warpSystemManager;

    private UnityEvent subWeaponUsingComplete;

    private bool isFirable;
    private bool isFiring;
    private bool isUsable;

    private void Awake()
    {
        if (gun == null)
        {
            Debug.LogError("Gun is Null");
        }

        if (subWeapons == null)
        {
            Debug.LogError("subWeapon is Null");
        }

        if (warpSystemManager == null)
        {
            Debug.LogError("WarpSystemManager is Null");
        }

        isFirable = true;
        isUsable = true;

        Initialized();

    }

    public void Initialized()
    {
        warpSystemManager.RegisterWarpObserver(this);

        gun.OnGunChange(0);

        subWeaponUsingComplete = new UnityEvent();

        for (int i = 0; i < subWeapons.Length; i++)
        {
            if (subWeapons[i] == null)
            {
                Debug.LogError("subWeapon[" + i + "] is Null");
            }

            subWeapons[i].Initialize(subWeaponUsingComplete);
        }

        subWeaponUsingComplete.AddListener(UsingComplete);
        UnLockSubWeapon(2);
    }

    private void Update()
    {
        if (isFirable)
            if (isFiring)
                if (gun.IsFireAble)
                {
                    gun.Fire();
                }
    }

    public void Fire(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    public void UnLockSubWeapon(int index)
    {
        if (subWeapons[index] != null)
        {
            Debug.Log("Unlocking SubWeapon Index: " + index);
            subWeapons[index].UnLock();
        }
    }

    public void UseSubWeapon(string path)
    {
        if (!isUsable)
            return;

        switch (path)
        {
            case "/Keyboard/e":
                if (!subWeapons[0].IsUnlocked)
                {
                    Debug.Log(subWeapons[0].IsUnlocked);
                    return;
                }

                if (subWeapons[0].IsReady)
                {
                    subWeapons[0].Use();
                    OnActiveSubWeapon();
                }
                break;
            case "/Keyboard/f":
                if (!subWeapons[1].IsUnlocked) 
                    return;

                if (subWeapons[1].IsReady)
                {
                    subWeapons[1].Use();
                    OnActiveSubWeapon();
                }
                break;
            case "/Keyboard/g":
                //if (!subWeapons[2].IsUnlocked) 
                //    return;

                if (subWeapons[2].IsReady)
                {
                    subWeapons[2].Use();
                    OnActiveSubWeapon();
                }
                break;
            default:
                Debug.LogWarning("No SubWeapon Bound to " + path);
                break;
        }
    }

    private void OnActiveSubWeapon()
    {
        gun.gameObject.SetActive(false);
        isFirable = false;
        isUsable = false;
    }

    private void UsingComplete()
    {
        gun.gameObject.SetActive(true);
        isFirable = true;
        isUsable = true;
    }

    public void OnWarpStageChanged(int newStage)
    {
        gun.OnGunChange(newStage);
    }
}