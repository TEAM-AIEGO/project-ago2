using UnityEngine;
using UnityEngine.Events;

public class BlackHoleGun : SubWeapon
{
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private Transform projectileLaunchPoint;

    protected override void Update()
    {
        base.Update();
    }

    public override void UnLock()
    {
        base.UnLock();
    }

    // 오버라이드 된 함수들이 다른 서브웨폰과 동일한 코드를 사용하고 있음
    // 서브웨폰 클래스에서 기본 구현으로 만들어 중복 제거를 해야 할 필요가 있어보임
    public override void Initialize(UnityEvent completeEvent)
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }

        onSubWeaponUseComplete = completeEvent;

        delayTimer = 0f;
        isLaunching = false;
        isInAftereffect = false;
    }

    public override void Use()
    {
        base.Use();
    }

    protected override void Fire()
    {
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint = Vector3.zero;

        aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;

        direction = (aimPoint - projectileLaunchPoint.position).normalized;

        BlackHoleProjectile blackHole = objectPool.SpawnBlackHoleProjectile(projectileLaunchPoint.position);
        //emitter.PlayFollow("BlackHole_Gun_Fire", playerTransform);
        blackHole.OnLaunched(direction);
        blackHole.OnBlackHoleTick += PlayHitMarker;

        cameraShake?.AddRecoil(new Vector2(Random.Range(-300f, 300f), 500f));
    }

    public void PlayHitMarker()
    {
        uiManager.ShowHitMarker();
    }
}
