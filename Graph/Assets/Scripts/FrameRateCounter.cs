using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;

    public enum DisplayMode {FPS, MS}

    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int frames;
    float duration, bestDuration = float.MaxValue, worstDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;

        if (frameDuration < bestDuration) {
            bestDuration = frameDuration;
        }
        if (frameDuration > bestDuration) {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS) 
            {
                // To show the frame rate expresses as frames per second we have to display its inverse, so one divided by the frame duration.
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", 1f / bestDuration, frames/duration, 1f / worstDuration);
                
            }
            else 
            {
                // To show the frame rate expresses as frames per second we have to display its inverse, so one divided by the frame duration.
                display.SetText("MS\n{0:0}\n{1:0}\n{2:0}", 1000f * bestDuration, 1000f * duration / frames, 1000f * worstDuration);
                            
            }
            // reset
            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }

        
    }
}
