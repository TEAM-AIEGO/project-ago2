using UnityEngine;

public class PlayerManager : Unit
{
    void Start()
    {
        
    }

    void Update()
    {
        
        
    }

    void OnWarpingStagesChange()
    {
        for (int i = 0; i < 3; i++)
            GameManager.Instance.isWarpingStages[i] = false;

        if (Health >= 0 && Health <= 100)
            GameManager.Instance.isWarpingStages[0] = true;
        else if (Health > 100 && Health < 200)
            GameManager.Instance.isWarpingStages[1] = true;
        else
            GameManager.Instance.isWarpingStages[2] = true;
    }

    void OnGetDamage(int damage)
    {
        Health -= damage;
        OnWarpingStagesChange();
        if (Health <= 0)
        {
            // dead
        }
    }
}
