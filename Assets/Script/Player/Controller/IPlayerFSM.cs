using UnityEngine;

public interface IPlayerFSM
{
    void Enter();
    void Update();
    void Exit();
}

public class PlayerStateMachine
{
    public IPlayerFSM CurrentState { get; private set; }

    public void ChangeState(IPlayerFSM newState)
    {
        Debug.Log("Changing state to: " + newState.GetType().Name);
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}

public class PlayerFSMBase : IPlayerFSM
{
    protected PlayerController player;

    public PlayerFSMBase(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}

public class PlayerDefaultState : PlayerFSMBase
{
    public PlayerDefaultState(PlayerController player) : base(player) { }

    public override void Enter() 
    {

    }

    public override void Update() 
    {
        //Debug.Log("Default State Update");
        player.PlayerMovement.TranslationPosition(player.Movement);
        //Debug.Log($"Movement : {player.Movement}");

    }

    public override void Exit() 
    {

    }
}

public class PlayerJumpState : PlayerFSMBase
{
    private bool isJumping;

    public PlayerJumpState(PlayerController player, bool isJumping) : base(player) 
    { 
        this.isJumping = isJumping;
    }

    public override void Enter() 
    {
        player.PlayerJumpAction.Jump(isJumping);
    }

    public override void Update() 
    {
        player.PlayerMovement.TranslationPosition(player.Movement);

        if (player.PlayerGroundChecker.IsGrounded())
        {
            player.PlayerStateMachine.ChangeState(new PlayerDefaultState(player));
        }
    }

    public override void Exit() 
    {

    }
}

public class PlayerSlideState : PlayerFSMBase
{
    public PlayerSlideState(PlayerController player) : base(player) { }

    public override void Enter() 
    {
        player.PlayerSlideAction.Slide(player.Movement);
    }

    public override void Update() 
    {
        if (player.PlayerSlideAction.CheckSlideEnded())
        {
            player.PlayerStateMachine.ChangeState(new PlayerDefaultState(player));
        }
    }

    public override void Exit() 
    {

    }
}

public class PlayerGroundPoundState : PlayerFSMBase
{
    public PlayerGroundPoundState(PlayerController player) : base(player) { }

    public override void Enter() 
    {
        player.PlayerGroundPoundAction.GroundPound();
    }

    public override void Update() 
    {
        if (player.IsGrounded)
        {
            player.PlayerStateMachine.ChangeState(new PlayerDefaultState(player));
        }
    }

    public override void Exit() 
    {
        player.PlayerGroundPoundAction.GroundPoundKaboom();
        
    }
}

public class PlayerDiedState : PlayerFSMBase
{
    public PlayerDiedState(PlayerController player) : base(player) { }

    public override void Enter() 
    {

    }

    public override void Update() 
    {

    }

    public override void Exit() 
    {

    }
}