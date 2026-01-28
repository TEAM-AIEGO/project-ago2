using UnityEngine;

public class PlayerManager : Unit
{
    protected override void Update()
    {
        base.Update();
        ChangeWarpStage();
    }

    void ChangeWarpStage()
    {
        if (health >= 0 && health <= 100)
            GameManager.Instance.WarpStage = 0;
        else if (health > 100 && health < 200)
            GameManager.Instance.WarpStage = 1;
        else
            GameManager.Instance.WarpStage = 2;
    }
}