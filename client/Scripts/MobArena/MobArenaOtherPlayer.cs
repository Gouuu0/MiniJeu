using Godot;
using System;

public class MobArenaOtherPlayer : Node2D
{
    public Tween moveTween = new Tween();
    public Tween rotationTween = new Tween();

    public int id;

    public override void _Ready()
    {
        AddChild(moveTween);
        AddChild(rotationTween);
    }
}
