using Godot;
using System;

public class Player : CharacterBody
{
    readonly float moveSpeed = 125f;
    readonly float jumpPower = 300f;
    AnimationPlayer animationPlayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("Idle");
        animationPlayer.PlaybackSpeed = 0.5f;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Update();
        if(Input.IsActionJustPressed("jump")) Jump(jumpPower);
        MoveVelocity.x = Input.GetAxis("left", "right") * moveSpeed;
    }
}
