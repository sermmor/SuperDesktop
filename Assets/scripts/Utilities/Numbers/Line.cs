using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line
{
    Vector2 _pointInit;
    Vector2 _pointEnd;

    /// <summary>
    /// Pendiente de la recta
    /// </summary>
    public float Slope { get; private set; }

    /// <summary>
    /// Cantidad que se añade en la ecuación de la recta.
    /// </summary>
    public float B { get; private set; }

    /// <summary>
    /// Toda línea está compuesta por dos puntos (da igual quién sea pointInit y pointEnd, el orden no influye).
    /// </summary>
    public Line(Vector2 pointInit, Vector2 pointEnd)
    {
        _pointInit = pointInit;
        _pointEnd = pointEnd;

        if (_pointEnd.x != _pointInit.x)
            Slope = (_pointEnd.y - _pointInit.y) / (_pointEnd.x - _pointInit.x);
        else
            Slope = float.PositiveInfinity;

        // Para calcular la B, aplicamos y = m*x + b
        if (Slope == float.PositiveInfinity)
            B = float.PositiveInfinity;
        else
            B = pointInit.y - (Slope * pointInit.x);
    }

    /// <summary>
    /// Dada la X, calcula Y y la devuelve.
    /// </summary>
    public float GetY(float x)
    {
        if (Slope != float.PositiveInfinity)
            return Slope * x + B;
        else
            return float.PositiveInfinity;
    }

    /// <summary>
    /// Dada la Y, calcula X y la devuelve.
    /// </summary>
    public float GetX(float y)
    {
        // y = Slope * x + B  =>  y - B = Slope * x  =>  x = (y - B) / Slope
        if (Slope != float.PositiveInfinity)
            return (y - B) / Slope;
        else
            return 0; // Recuerda: "lo que sea" dividido entre infinito es igual a cero.
    }

    /// <summary>
    /// Comprueba si el punto pasado como parámetro se encuentra en la línea.
    /// </summary>
    public bool IsPointInLine(Vector2 point)
    {
        if (Slope == float.PositiveInfinity)
            return point.y == _pointInit.y && point.x == 0;
        else
            return point.y == (Slope * point.x) + B;
    }

    /// <summary>
    /// Devuelve el punto de intersección entre dos rectas.
    /// </summary>
    /// <returns>El punto de intersección entre dos rectas. </returns>
    public static Vector2 IntersectionPointBetweenToLines(Line l1, Line l2)
    {
        // Este problema es equivalente a resolver un sistema de ecuaciones con ambas líneas (recuerda que tenemos su b y su pendiente).
        /* y = l1.Slope * x + l1.B 
           y = l2.Slope * x + l2.B */
        
        Vector2 point = Vector2.zero;

        if (l1.Slope == float.PositiveInfinity || l2.Slope == float.PositiveInfinity)
            point.x = 0;
        else
            point.x = (l2.B - l1.B) / (l1.Slope - l2.Slope);

        // Una vez tenemos la solución para la x la aplicamos a una de las ecuaciones de la recta para optener la y.
        if (l1.Slope != float.PositiveInfinity)
            point.y = l1.Slope * point.x + l1.B;
        else if (l2.Slope != float.PositiveInfinity)
            point.y = l2.Slope * point.x + l2.B;
        else
            point.y = float.PositiveInfinity; // Son palalelas.


        return point;
    }
}

