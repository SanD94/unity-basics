using System.Net.NetworkInformation;
using NUnit.Framework.Internal.Commands;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

using static UnityEngine.Mathf;
public static class FunctionLibrary
{
    public delegate Vector3 Function (float u, float v, float t);
    
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus}
    static Function[] functions = {Wave, MultiWave, Ripple, Sphere, Torus};

    public static int FunctionCount => functions.Length;
    
    public static Function GetFunction (FunctionName name) 
    {
        return functions[(int)name];
    }
    
    public static FunctionName GetNextFunctionName (FunctionName name)
    {
        return (FunctionName)(((int)name + 1) % functions.Length);
    }
    
    public static FunctionName GetRandomFunctionNameOtherThan (FunctionName name) 
    {
		var choice = (FunctionName)Random.Range(1, functions.Length);
		return choice == name ? 0 : choice;
	}
    
    public static Vector3 Morph(
        float u, float v, float t, 
        Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(
            from(u,v,t), 
            to(u,v,t), 
            SmoothStep(0f, 1f, progress)
        );
    }

    public static Vector3 Wave(float u, float v, float t) 
    {
        float w = Sin(PI * (u + v + t));

        return new(u, w, v);
    }

    public static Vector3 MultiWave(float u, float v, float t) 
    {
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
        // u XZ plane scale [-1, 1] - [-pi, pi]
        // v XY plane scale [-1, 1] - [-pi/2, pi/2]
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        return r * new Vector3(
            Cos(PI * 0.5f * v) * Cos(PI * u),
            Sin(PI * 0.5f * v),
            Cos(PI * 0.5f * v) * Sin(PI * u)
        );
    }
    
    public static Vector3 Torus (float u, float v, float t) 
    {
        // u XZ plane scale [-1, 1]
        // v XY plane scale [-1, 1]
        // r1 major radius
        // r2 minor radius
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        return new(
            (r1 + r2 * Cos(PI * v)) * Cos(PI * u),
            r2 * Sin(PI * v),
            (r1 + r2 * Cos(PI * v)) * Sin(PI * u)
        );
    }
}
