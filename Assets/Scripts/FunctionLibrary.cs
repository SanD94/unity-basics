using UnityEngine;

using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate Vector3 Function (float u, float v, float t);
    
    public enum FunctionName { Wave, MultiWave, Ripple }
    static Function[] functions = {Wave, MultiWave, Ripple};
    
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
    
}
