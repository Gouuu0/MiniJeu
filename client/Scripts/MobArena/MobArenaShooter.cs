using Godot;
using System;

public class MobArenaShooter : MobArenaEntities
{
    [Export] PackedScene BULLET_SCENE_PATH;
    [Export] NodePath GUN_POSITION_PATH;
    [Export] protected int TIME_BETWEEN_SHOOT = 4;
    [Export] protected float BULLET_DAMAGE;
    [Export] protected float BULLET_SPEED;

    protected int shootTimer = 0;

    Node2D gunPosition;

    public override void _Ready()
    {
        gunPosition = GetNode<Node2D>(GUN_POSITION_PATH);
        base._Ready();
    }

    public override void _Process(float delta)
    {
        shootTimer++;
        base._Process(delta);
    }

    protected virtual void Shoot()
    {
        //Projectiles lBullet;
        //foreach (Node2D pNode in gunPosition.GetChildren())
        //{
        //    lBullet = BULLET_SCENE_PATH.Instance<Projectiles>();
        //    lBullet.startPos = pNode.GlobalPosition;
        //    lBullet.direction = pNode.GlobalRotation;
        //    lBullet.damage = BULLET_DAMAGE;
        //    lBullet.speed = BULLET_SPEED;
        //}
    }
}
