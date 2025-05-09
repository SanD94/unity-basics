using System;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField]
    ComputeShader computeShader;
    
    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");
    
    const int maxResolution = 1000;
    
    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;
    
    [SerializeField, Range(10, maxResolution)]
    int resolution = 10;
    
    [SerializeField]
    FunctionLibrary.FunctionName function;
    
    public enum TransitionMode { Cycle, Random }
    
    [SerializeField]
    TransitionMode transitionMode;
    
    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;
    
    float duration;
    bool inTransition;
    FunctionLibrary.FunctionName transitionFunction;

    
    ComputeBuffer positionsBuffer;
    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4); // Vector3
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }


    void Update()
    {
        duration += Time.deltaTime;
        if (inTransition)
        {
            if (duration >= functionDuration)
            {
                duration -= transitionDuration;
                inTransition = false;
            }

        }
        else if (duration >= functionDuration) 
        {
            duration -= functionDuration;
            inTransition = true;
            transitionFunction = function;
            PickNextFunction();
        }
        
        UpdateFunctionOnGPU();
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
    
    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        
        computeShader.SetBuffer(0, positionsId, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
		material.SetFloat(stepId, step);
        
        Bounds bounds = new(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        RenderParams rp = new(material) {
            worldBounds = bounds
        };

        Graphics.RenderMeshPrimitives(rp, mesh, 0, resolution * resolution);

    }
}
