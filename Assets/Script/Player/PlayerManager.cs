using UnityEngine;

public class PlayerManager : Unit
{
    private GameManager gameManager;
    protected override void Update()
    {
        base.Update();
        ChangeWarpStage();
    }

    void ChangeWarpStage()
    {
        if (gameManager)
        {
            if (health >= 0 && health <= 100)
            {
                gameManager.WarpStage = 0;
            }
            else if (health > 100 && health < 200)
            {
                gameManager.WarpStage = 1;
            }
            else
            {
                gameManager.WarpStage = 2;
            }
        }
        else
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (!gameManager)
            {
                Debug.LogError("GameManager not found! Warping will not work. Be sure to add one.");
            }
        }
        
    }
}