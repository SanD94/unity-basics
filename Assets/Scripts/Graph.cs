using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    
    [SerializeField, Range(10, 200)]
    int resolution = 10;
    
    [SerializeField]
    FunctionLibrary.FunctionName function;
    
    public enum TransitionMode { Cycle, Random }
    
    [SerializeField]
    TransitionMode transitionMode;
    
    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;
    
    Transform[] points;
    
    float duration;
    bool inTransition;
    FunctionLibrary.FunctionName transitionFunction;

    void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i] = Instantiate(pointPrefab);
            point.SetParent(transform, false);
            point.localScale = scale;
        }
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
        
        if (inTransition)
        {
            UpdateFunctionTransition();
        } 
        else
        {
            UpdateFunction();
        }
    }

    void UpdateFunction() 
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution) {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);
        }

    } 
    void UpdateFunctionTransition() 
    {
        FunctionLibrary.Function 
            from = FunctionLibrary.GetFunction(transitionFunction),
            to = FunctionLibrary.GetFunction(function);

        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution) {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(
                u, v, time,
                from, to, progress
            );
        }

    } 
    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
}
