using NUnit.Framework.Internal.Commands;
using UnityEngine;

using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate Vector3 Function (float u, float v, float t);
    
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere }
    static Function[] functions = {Wave, MultiWave, Ripple, Sphere};
    
    public static Function GetFunction (FunctionName name) {
        return functions[(int)name];
    }

    public static Vector3 Wave(float u, float v, float t) {
        float w = Sin(PI * (u + v + t));

        return new(u, w, v);
    }

    public static Vector3 MultiWave(float u, float v, float t) {
        float w = Sin(PI * (u + 0.5f * t));
        w += 0.5f * Sin(2f * PI * (v + t));
        w += Sin(PI * (u + v + 0.25f * t));
        w *= 1f / 2.5f;

        return new(u, w, v);
    }
    
    public static Vector3 Ripple(float u, float v, float t) {
        float d = Sqrt(u * u + v * v);
        float w = Sin(PI * (4f * d - t));
        w /= 1f + 10f * d;

        return new(u, w, v);
    }
    
    public static Vector3 Sphere (float u, float v, float t) {
        // u XZ plane scale [-1, 1]
        // v XY plane scale [-1, 1]
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        return r * new Vector3(
            Cos(PI * 0.5f * v) * Cos(PI * u),
            Sin(PI * 0.5f * v),
            Cos(PI * 0.5f * v) * Sin(PI * u)
        );
    }
    
}
