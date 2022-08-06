using Godot;
using System;

public class Player : KinematicBody2D
{
    const float GRAVITY = 800f;
    const int JUMPABLE_FRAME_DEADLINE = 8;
    readonly float moveSpeed = 125f;
    Vector2 velocity = Vector2.Zero;
    float accelMultiplier = 7;
    float groundFrictionMultiplier = 7;
    bool isMoving = false;
    int jumpableFrameCount = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Update();
    }

    public override void _PhysicsProcess(float delta)
    {   
        if(IsOnFloor()) jumpableFrameCount = JUMPABLE_FRAME_DEADLINE;

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

        if(!IsOnFloor())velocity.y += GRAVITY * delta;

        if(jumpableFrameCount > 0)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                velocity.y -= 300f;
                jumpableFrameCount = 1;
            }
            jumpableFrameCount--;
        }

        if(velocity.Length() < 0.5f) velocity = Vector2.Zero;
        velocity = MoveAndSlide(velocity, Vector2.Up, false, 4, 1.0f);
    }

    public override void _Draw()
    {
        if(Game.IsDebug)
        {
            DrawLine(Vector2.Zero, velocity / 2, Color.Color8(255,0,0), 1);
            string velocityStr = String.Format("[{0:F}, {1:F}]", velocity.x, velocity.y);
            DrawString(Game.DebugFont, velocity / 2, velocityStr, new Color( 1, 1, 1, 1 ));
            if(jumpableFrameCount != 0)
                DrawLine(Vector2.Up * 25 + Vector2.Left * jumpableFrameCount * 2, Vector2.Up * 25 + Vector2.Right * jumpableFrameCount * 2, Color.Color8(100,255,100), 2);
        }
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
        //Testa
    }

    
}
