using System.Collections.Generic;

/// At least two points are needed to interpolate something.
/// @class Lagrange polynomial interpolation.
/// The computed interpolation polynomial will be reffered to as L(x).
/// @example
/// var l = new Lagrange(0, 0, 1, 1);
/// var index = l.addPoint(0.5, 0.8);
/// console.log(l.valueOf(0.1));
/// 
/// l.changePoint(index, 0.5, 0.1);
/// console.log(l.valueOf(0.1));
public class LagrangeInterpolation
{
    List<float> xs, ys, ws;

    public LagrangeInterpolation(float x1, float y1, float x2, float y2)
    {
        this.xs = new List<float>() { x1, x2 };
        this.ys = new List<float>() { y1, y2 };
        this.ws = new List<float>();
        this.UpdateWeights();
    }

    public LagrangeInterpolation(float[] xs, float[] ys)
    {
        this.xs = new List<float>(xs);
        this.ys = new List<float>(ys);
        this.ws = new List<float>();
        this.UpdateWeights();
    }

    /// Adds a new point to the polynomial. L(x) = y
    /// @return {Number} The index of the added point. Used for changing the point. See changePoint.
    public float AddPoint(float x, float y)
    {
        this.xs.Add(x);
        this.ys.Add(y);
        this.UpdateWeights();
        return this.xs.Count - 1;
    }

    /// Changes a previously added point.
    public void ChangePoint(int index, float x, float y)
    {
        this.xs[index] = x;
        this.ys[index] = y;
        this.UpdateWeights();
    }

    /// Recalculate barycentric weights.
    void UpdateWeights()
    {
        this.ws = new List<float>();
        int k = this.xs.Count;
        float w;

        for (int j = 0; j < k; ++j)
        {
            w = 1;
            for (int i = 0; i < k; ++i)
            {
                if (i != j)
                {
                    w *= this.xs[j] - this.xs[i];
                }
            }
            this.ws.Add(1 / w);
        }
    }

    /// Calculate L(x)
    public float valueOf(float x)
    {
        float a = 0, b = 0, c = 0;

        for (var j = 0; j < this.xs.Count; ++j)
        {
            if (x != this.xs[j])
            {
                a = this.ws[j] / (x - this.xs[j]);
                b += a * this.ys[j];
                c += a;
            }
            else
            {
                return this.ys[j];
            }
        }

        return b / c;
    }
}
