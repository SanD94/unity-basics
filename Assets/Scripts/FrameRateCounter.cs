using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;
    
    public enum DisplayMode { FPS, MS }
    
    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;
    
    
    int frames;
    float duration, bestDuration = float.MaxValue, worstDuration = 0;

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames ++;
        duration += frameDuration;
        
        if (frameDuration < bestDuration) {
            bestDuration = frameDuration;
        }
        
        if (frameDuration > worstDuration) {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration) {
            var performance = GetFPS(displayMode);
            display.SetText(
                performance.Item4,
                performance.Item1,
                performance.Item2,
                performance.Item3
            );

            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }

    
    private Tuple<float, float, float, string> GetFPS(DisplayMode displayMode) 
    {
        Tuple<float, float, float, string> res = displayMode switch
        {
            DisplayMode.FPS => new(
                            1f / bestDuration,
                            frames / duration,
                            1f / worstDuration,
                            "FPS\n{0:0}\n{1:0}\n{2:0}"
                        ),
            DisplayMode.MS => new(
                            1000f * bestDuration,
                            1000f * duration / frames,
                            1000f * worstDuration,
                            "MS\n{0:1}\n{1:1}\n{2:1}"
                        ),
            _ => new(0f, 0f, 0f, ""),
        };
        return res;
    }

}
