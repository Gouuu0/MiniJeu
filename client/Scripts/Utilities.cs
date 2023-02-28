using Godot;
using System;

public static class Utilities
{
    public static string UP_KEY = "up";
    public static string DOWN_KEY = "down";
    public static string LEFT_KEY = "left";
    public static string RIGHT_KEY = "right";
    public static string SHIFT_KEY = "shift";
    public static string LEFT_CLICK_KEY = "leftclick";

    public static Vector2 SCREEN_SIZE = new Vector2(1920, 1080);
    public static float SCREEN_MARGIN = 30;

    public static RandomNumberGenerator rand = new RandomNumberGenerator();

    static Utilities()
    {
        rand.Randomize();
    }
}
