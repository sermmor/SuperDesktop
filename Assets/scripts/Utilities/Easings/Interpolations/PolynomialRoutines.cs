using System;
using System.Collections.Generic;
using UnityEngine;

public static class PolynomialRoutines
{
    // Evaluates the polynomial of the segment corresponding to the specified x value.
    public static float evaluatePolySegment(float[] xVals, List<float>[] segmentCoeffs, float x)
    {
        int i = Array.BinarySearch(xVals, x);
        if (i < 0)
        {
            i = -i - 2;
        }
        i = Mathf.Max(0, Mathf.Min(i, segmentCoeffs.Length - 1));
        return evaluatePoly(segmentCoeffs[i], x - xVals[i]);
    }

    // Evaluates the value of a polynomial.
    // c contains the polynomial coefficients in ascending order.
    public static float evaluatePoly(List<float> c, float x)
    {
        int n = c.Count;
        if (n == 0)
        {
            return 0;
        }
        float v = c[n - 1];
        for (int i = n - 2; i >= 0; i--)
        {
            v = x * v + c[i];
        }
        return v;
    }

    // Trims top order polynomial coefficients which are zero.
    public static float[] trimPoly(float[] c)
    {
        int n = c.Length;
        while (n > 1 && c[n - 1] == 0)
        {
            n--;
        }
        return (n == c.Length) ? c : c.SubArray(0, n);
    }
}
