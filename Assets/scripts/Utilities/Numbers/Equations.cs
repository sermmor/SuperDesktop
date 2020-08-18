using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Equations
{
    /// <summary>
    /// Devuelve las y resultantes de la fórmula de círculo: (x - h)^2 + (y - k)^2 = r^2
    /// </summary>
    /// <param name="x">Variable x de la fórmula</param>
    /// <param name="radius">Radio del círculo</param>
    /// <param name="pointCenter">Centro del círculo</param>
    /// <returns>Variable y de la fórmula (solución positiva y negativa), si la fórmula no tiene solución, devolverá vacío.</returns>
    public static float[] CircleFormula(float x, float radius, Vector2 pointCenter)
    {
        float h = pointCenter.x, k = pointCenter.y;
        float toSqrt = (4f * Mathf.Pow(k, 2f)) + (2f * Mathf.Pow(radius, 2f) - (2f * Mathf.Pow(x - h, 2f)) - (2f * Mathf.Pow(k, 2f)));

        if (toSqrt < 0)
            return new float[] { };

        return new[] { k + (Mathf.Sqrt(toSqrt) / 2f), k - (Mathf.Sqrt(toSqrt) / 2f) };
    }

    /// <summary>
    /// Devuelve las y resultantes de la fórmula de círculo: (x - h)^2 + (y - k)^2 = r^2
    /// </summary>
    /// <param name="x">Variable x de la fórmula</param>
    /// <param name="radius">Radio del círculo</param>
    /// <param name="pointCenter">Centro del círculo</param>
    /// <returns>Variable y de la fórmula, si la fórmula no tiene solución, devolverá vacío.</returns>
    public static float[] CircleFormula(float x, float radius, Vector3 pointCenter)
    {
        return CircleFormula(x, radius, new Vector2(pointCenter.x, pointCenter.y));
    }

    public static float GetMiddlePointBetweenToLimits(float x1, float x2)
    {
        if (x1 < x2)
            return ((x2 - x1) / 2f) + x1;
        else
            return ((x1 - x2) / 2f) + x2;

    }
}

