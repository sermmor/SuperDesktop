using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EasingBuilder
{
    /// * @Precondición Los números de la nube de puntos deben estar ordenados en el eje X y como mínimo la nube de puntos debe tener 2 puntos.
	public static System.Func<float, float> BuildLagrange(Vector2[] cloudOfPoints)
    {
        float[] xVals = (from point in cloudOfPoints select point.x).ToArray();
        float[] yVals = (from point in cloudOfPoints select point.y).ToArray();
        LagrangeInterpolation lagrange = new LagrangeInterpolation(xVals, yVals);
        return (float x) => lagrange.valueOf(x);
    }

    /// * @Precondición Los números de la nube de puntos deben estar ordenados en el eje X y como mínimo la nube de puntos debe tener 5 puntos.
    public static System.Func<float, float> BuildAkimaSpline(Vector2[] cloudOfPoints)
    {
        float[] xVals = (from point in cloudOfPoints select point.x).ToArray();
        float[] yVals = (from point in cloudOfPoints select point.y).ToArray();
        return AkimaSplineInterpolation.createAkimaSplineInterpolator(xVals, yVals);
    }

    /// <sumary>
    /// Los puntos están en formato (tiempo, VALOR) siendo VALOR un porcentaje (en tanto por uno) 
    /// de desplazamiento, velocidad o rotación (el tiempo debe ser también un porcentaje de tanto por uno).
    /// Los porcentajes de tanto por uno irán del 0 al 1, siendo el tiempo SIEMPRE 0 en el mínimo y 1 en el máximo.
    /// * Para ajustar un buen easing usar esto: https://cubic-bezier.com/
    /// </sumary>
    public static System.Func<float, float> BuildBezier(Vector2 pointMin, Vector2 pointMax) 
        => BezierCurve.CreateBezier(pointMin.x, pointMin.y, pointMax.x, pointMax.y);
}

