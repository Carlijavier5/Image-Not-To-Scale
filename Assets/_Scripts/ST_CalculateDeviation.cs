using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ST_CalculateDeviation {
    public static float SCALE_FACTOR = 3;
    
    /// <summary>
    /// Returns a scale factor based on standard deviation
    /// </summary>
    /// <param name="idealValue"> ideal value </param>
    /// <param name="givenValue"> given value </param>
    /// <param name="minMaxRange"> x = min range, y = max range</param>
    /// <returns></returns>
    public static float CalculateScaleFactor(float idealValue, float givenValue, Vector2 minMaxRange) {
        float difference = Math.Abs(idealValue - givenValue);
        float magnitude = Math.Abs(minMaxRange.y - minMaxRange.x);
        float variance = difference / magnitude;
        return variance * SCALE_FACTOR;
    }
}
