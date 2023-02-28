using Godot;
using System;

public class Projectiles : MobArenaEntities
{
    [Export] public float LINEAR_ACCELERATION = 1;

    public float speed;
    public float damage;
    public Vector2 startPos;
    public Texture texture;
    public float direction;

    public override void _Ready()
    {
        GlobalPosition = startPos;
        GlobalRotation = direction;
        velocity = new Vector2(speed, 0).Rotated(Rotation);
        base._Ready();
    }

    public override void _Process(float delta)
    {
        Position += velocity;
        velocity *= LINEAR_ACCELERATION;

        if (GlobalPosition.y + velocity.y < 0 - Utilities.SCREEN_MARGIN ||
                GlobalPosition.y + velocity.y > Utilities.SCREEN_SIZE.y + Utilities.SCREEN_MARGIN ||
                GlobalPosition.x + velocity.x > Utilities.SCREEN_SIZE.x + Utilities.SCREEN_MARGIN ||
                GlobalPosition.x + velocity.x < 0 - Utilities.SCREEN_MARGIN)
            QueueFree();


        base._Process(delta);
    }
}
