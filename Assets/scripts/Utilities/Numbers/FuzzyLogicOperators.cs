using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FuzzyLogicOperators
{
    /// <summary>
    /// Devuelve si value1 es similar a value2, esto es si value1 está entre los valores value2 - limitFactor 
    /// y value2 + limitFactor.
    /// </summary>
    public static bool AreSimilar(float value1, float value2, float limitFactor)
    {
        return (value2 - limitFactor) <= value1 && value1 <= (value2 + limitFactor);
    }

    /// <summary>
    /// Devuelve si value1 es similar a value2, esto es si value1 está entre los valores value2 - limitFactor 
    /// y value2 + limitFactor.
    /// </summary>
    public static bool AreSimilar(Vector2 value1, Vector2 value2, float limitFactor)
    {
        return AreSimilar(value1.x, value2.x, limitFactor) && AreSimilar(value1.y, value2.y, limitFactor);
    }

    /// <summary>
    /// Devuelve si value1 es similar a value2, esto es si value1 está entre los valores value2 - limitFactor 
    /// y value2 + limitFactor.
    /// </summary>
    public static bool AreSimilar(Vector2 value1, Vector3 value2, float limitFactor)
    {
        return AreSimilar(value1.x, value2.x, limitFactor) && AreSimilar(value1.y, value2.y, limitFactor);
    }
}
