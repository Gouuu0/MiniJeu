using Godot;
using System;

public class OneShotParticle : Particles2D
{
    public override void _Ready()
    {
        Emitting = true;
        OneShot = true;
    }

    public override void _Process(float delta)
    {
        if (!Emitting)
        {
            QueueFree();
        }
    }
}

