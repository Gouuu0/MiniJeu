using Godot;
using System;

public class MobArenaPlayer : MobArenaShooter
{
    public static MobArenaPlayer Instance;

    [Export] float SHOOT_PUSHBACK_FORCE = 3;
    [Export] float SLIP_FORCE = 1.2f;
    [Export] float SHIFT_SPEED_DIVIDOR = 4;

    public override void _Ready()
    {
        Instance = this;

        base._Ready();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        InputMovements();
        if (Input.IsActionPressed(Utilities.LEFT_CLICK_KEY) && shootTimer > TIME_BETWEEN_SHOOT) Shoot();

        LookAt(GetGlobalMousePosition());
        PositionChange();
    }

    private void InputMovements()
    {
        float lSpeedToGive = speed;
        if (Input.IsActionPressed(Utilities.SHIFT_KEY)) lSpeedToGive /= SHIFT_SPEED_DIVIDOR;

        if (Input.IsActionPressed(Utilities.UP_KEY))
        {
            velocity.y -= lSpeedToGive;
        }
        if (Input.IsActionPressed(Utilities.DOWN_KEY))
        {
            velocity.y += lSpeedToGive;
        }
        if (Input.IsActionPressed(Utilities.RIGHT_KEY))
        {
            velocity.x += lSpeedToGive;
        }
        if (Input.IsActionPressed(Utilities.LEFT_KEY))
        {
            velocity.x -= lSpeedToGive;
        }
    }

    protected override void Shoot()
    {
        velocity += new Vector2(-SHOOT_PUSHBACK_FORCE, 0).Rotated(Rotation);
        shootTimer = 0;
        base.Shoot();
    }


    private void PositionChange()
    {
        if (GlobalPosition.y + velocity.y < 0 + Utilities.SCREEN_MARGIN ||
                GlobalPosition.y + velocity.y > Utilities.SCREEN_SIZE.y - Utilities.SCREEN_MARGIN)
            velocity.y = 0;

        if (GlobalPosition.x + velocity.x > Utilities.SCREEN_SIZE.x - Utilities.SCREEN_MARGIN ||
            GlobalPosition.x + velocity.x < 0 + Utilities.SCREEN_MARGIN)
            velocity.x = 0;

        Position += velocity;

        velocity /= SLIP_FORCE;
    }
}

