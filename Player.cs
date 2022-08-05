using Godot;
using System;

public class Player : KinematicBody2D
{
    const float GRAVITY = 800f;
    readonly float moveSpeed = 100f;
    Vector2 velocity = Vector2.Zero;
    float accelMultiplier = 7;
    float groundFrictionMultiplier = 7;
    bool isMoving = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {   
        if(!isMoving && IsOnFloor())
        {
            var friction = getFriction(delta);
            velocity += friction - GetFloorNormal() * delta;
        }
        
        

        isMoving = false;
        var moveInput = Input.GetAxis("ui_left","ui_right");
        
        if(moveInput != 0)
        {
            isMoving = true;
            var xTargetSpeed = moveInput * moveSpeed;
            float xAccel = getAcceleration(xTargetSpeed, delta);
            velocity.x += xAccel;
        }

        if(!IsOnFloor()) velocity.y += GRAVITY * delta;

        if(Input.IsActionJustPressed("ui_select") && IsOnFloor())
        {
            velocity.y -= 300f;
        }

        velocity = MoveAndSlide(velocity, Vector2.Up, false, 4, 1.0f);
    }

    float getAcceleration(float targetSpeed, float delta)
    {
        // 속도 차이가 클수록 가속도가 커짐, 현재 속력이 목표 속력보다 크다면 가속안함.

        var speedDifference = targetSpeed - velocity.x;
        if(Mathf.Sign(speedDifference) != Mathf.Sign(targetSpeed) && targetSpeed == 0)
        {
            return 0;
        }
        else
        {
            return speedDifference * Mathf.Min(accelMultiplier * delta , 1);
        }
    }

    Vector2 getFriction(float delta)
    {
        return -velocity * Mathf.Min(groundFrictionMultiplier * delta , 1);
        //Test
    }
}
