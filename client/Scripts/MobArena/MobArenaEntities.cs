using Godot;
using System;

public class MobArenaEntities : Node2D
{
    [Export] PackedScene EXPLOSION_SCENE;
    [Export] public int TEAM = 0;
    [Export] protected float BASE_SPEED = 2.5f;
    [Export] protected float MAX_HEALTH;
    string HITBOX_PATH = "HitBox";

    protected Vector2 velocity;
    protected Area2D hitBox;
    protected float speed;
    protected float health;

    public Action Act;

    public override void _Ready()
    {
        speed = BASE_SPEED;
        health = MAX_HEALTH;
        hitBox = GetNode<Area2D>(HITBOX_PATH);
        hitBox.Connect("area_entered", this, nameof(AreaEntered));

        Act = DoIdle;
    }

    public override void _Process(float delta)
    {
        Act();
    }

    protected virtual void DoIdle()
    {

    }

    protected virtual void DoBasePatern()
    {

    }

    protected virtual void AreaEntered(Area2D pArea)
    {
        Node2D lNode = (Node2D)pArea.GetParent();

        if (lNode is Camera2D) Act = DoBasePatern;

        if (lNode is MobArenaEntities && ((MobArenaEntities)lNode).TEAM != 0 && ((MobArenaEntities)lNode).TEAM != TEAM)
        {
            if (!(lNode is Projectiles))
                Death();
            else
                TakeHit(((Projectiles)lNode).damage);

        }
    }

    protected virtual void TakeHit(float pDamage)
    {
        health -= pDamage;
        if (health <= 0) Death();
    }

    protected virtual void Death()
    {
        //OneShotParticle lExplosion = EXPLOSION_SCENE.Instance<OneShotParticle>();
        //lExplosion.GlobalPosition = GlobalPosition;

        QueueFree();
    }
}

