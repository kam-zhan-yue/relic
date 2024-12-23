using UnityEngine;

public static class Easings
{
    public static float OutExpo(float x)
    {
        return x >= 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}