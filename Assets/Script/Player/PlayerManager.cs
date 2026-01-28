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
        if (Health >= 0 && Health <= 100)
            GameManager.Instance.WarpStage = 0;
        else if (Health > 100 && Health < 200)
            GameManager.Instance.WarpStage = 1;
        else
            GameManager.Instance.WarpStage = 2;
    }
}
