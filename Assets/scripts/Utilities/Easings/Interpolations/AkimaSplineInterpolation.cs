using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class AkimaSplineInterpolation
{
    const float EPSILON = float.MinValue;

    /**
    * An univariate numeric function.
    */
    //static System.Func<float, float> UniFunction;

    /// <summary>
    /// Returns a function that computes a cubic spline interpolation for the data
    /// set using the Akima algorithm, as originally formulated by Hiroshi Akima in
    /// his 1970 paper "A New Method of Interpolation and Smooth Curve Fitting Based
    /// on Local Procedures."
    /// J. ACM 17, 4 (October 1970), 589-602. DOI=10.1145/321607.321609
    /// http://doi.acm.org/10.1145/321607.321609
    ///
    /// This implementation is based on the Akima implementation in the CubicSpline
    /// class in the Math.NET Numerics library. The method referenced is
    /// CubicSpline.InterpolateAkimaSorted.
    ///
    /// Returns a polynomial spline function consisting of n cubic polynomials,
    /// defined over the subintervals determined by the x values,
    /// x[0] < x[1] < ... < x[n-1].
    /// The Akima algorithm requires that n >= 5.
    ///
    /// @param xVals
    ///    The arguments of the interpolation points.
    /// @param yVals
    ///    The values of the interpolation points.
    /// @returns
    ///    A function which interpolates the dataset.
    /// </summary>
    public static System.Func<float, float> createAkimaSplineInterpolator(float[] xVals, float[] yVals)
    {
        List<float>[] segmentCoeffs = computeAkimaPolyCoefficients(xVals, yVals);
        float[] xValsCopy = new float[xVals.Length];
        xVals.CopyTo(xValsCopy, 0); // clone to break dependency on passed values
        return (float x) => {
            return PolynomialRoutines.evaluatePolySegment(xValsCopy, segmentCoeffs, x);
        };
    }

    /**
    * Computes the polynomial coefficients for the Akima cubic spline
    * interpolation of a dataset.
    *
    * @param xVals
    *    The arguments of the interpolation points.
    * @param yVals
    *    The values of the interpolation points.
    * @returns
    *    Polynomial coefficients of the segments.
    */
    static List<float>[] computeAkimaPolyCoefficients(float[] xVals, float[] yVals)
    {
        if (xVals.Length != yVals.Length) {
            Debug.Log("[LogLabel.Easing] Dimension mismatch for xVals and yVals.");
        }
        if (xVals.Length < 5) {
            Debug.Log("[LogLabel.Easing] Number of points is too small (it's needed, at least, 5 points).");
        }
        
        UtilitiesInterpolation.MathArrays_checkOrder(xVals);
        int n = xVals.Length - 1; // number of segments

        float[] differences = new float[n];
        float[] weights = new float[n];

        for (int i = 0; i < n; i++) {
            differences[i] = (yVals[i + 1] - yVals[i]) / (xVals[i + 1] - xVals[i]);
        }

        for (int i = 1; i < n; i++) {
            weights[i] = Mathf.Abs(differences[i] - differences[i - 1]);
        }

        // Prepare Hermite interpolation scheme.
        float[] firstDerivatives = new float[n + 1];

        for (int i = 2; i < n - 1; i++) {
            float wP = weights[i + 1];
            float wM = weights[i - 1];
            if (Mathf.Abs(wP) < EPSILON && Mathf.Abs(wM) < EPSILON) {
                float xv  = xVals[i];
                float xvP = xVals[i + 1];
                float xvM = xVals[i - 1];
                firstDerivatives[i] = (((xvP - xv) * differences[i - 1]) + ((xv - xvM) * differences[i])) / (xvP - xvM);
            } else {
                firstDerivatives[i] = ((wP * differences[i - 1]) + (wM * differences[i])) / (wP + wM);
            }
        }

        firstDerivatives[0]     = differentiateThreePoint(xVals, yVals, 0, 0, 1, 2);
        firstDerivatives[1]     = differentiateThreePoint(xVals, yVals, 1, 0, 1, 2);
        firstDerivatives[n - 1] = differentiateThreePoint(xVals, yVals, n - 1, n - 2, n - 1, n);
        firstDerivatives[n]     = differentiateThreePoint(xVals, yVals, n    , n - 2, n - 1, n);

        return computeHermitePolyCoefficients(xVals, yVals, firstDerivatives);
    }

    /**
    * Three point differentiation helper, modeled off of the same method in the
    * Math.NET CubicSpline class.
    *
    * @param xVals
    *    x values to calculate the numerical derivative with.
    * @param yVals
    *    y values to calculate the numerical derivative with.
    * @param indexOfDifferentiation
    *    Index of the elemnt we are calculating the derivative around.
    * @param indexOfFirstSample
    *    Index of the first element to sample for the three point method.
    * @param indexOfSecondsample
    *    index of the second element to sample for the three point method.
    * @param indexOfThirdSample
    *    Index of the third element to sample for the three point method.
    * @returns
    *    The derivative.
    */
    static float differentiateThreePoint(float[] xVals, float[] yVals, int indexOfDifferentiation, 
            int indexOfFirstSample, int indexOfSecondsample, int indexOfThirdSample)
    {
        float x0 = yVals[indexOfFirstSample];
        float x1 = yVals[indexOfSecondsample];
        float x2 = yVals[indexOfThirdSample];

        float t  = xVals[indexOfDifferentiation] - xVals[indexOfFirstSample];
        float t1 = xVals[indexOfSecondsample]    - xVals[indexOfFirstSample];
        float t2 = xVals[indexOfThirdSample]     - xVals[indexOfFirstSample];

        float a = (x2 - x0 - (t2 / t1 * (x1 - x0))) / (t2 * t2 - t1 * t2);
        float b = (x1 - x0 - a * t1 * t1) / t1;

        return (2 * a * t) + b;
    }

    /**
    * Computes the polynomial coefficients for the Hermite cubic spline interpolation
    * for a set of (x,y) value pairs and their derivatives. This is modeled off of
    * the InterpolateHermiteSorted method in the Math.NET CubicSpline class.
    *
    * @param xVals
    *    x values for interpolation.
    * @param yVals
    *    y values for interpolation.
    * @param firstDerivatives
    *    First derivative values of the function.
    * @returns
    *    Polynomial coefficients of the segments.
    */
    static List<float>[] computeHermitePolyCoefficients(float[] xVals, float[] yVals, float[] firstDerivatives) 
    {
        if (xVals.Length != yVals.Length || xVals.Length != firstDerivatives.Length) {
            Debug.Log("[LogLabel.Easing] Dimension mismatch");
        }
        if (xVals.Length < 2) {
            Debug.Log("[LogLabel.Easing] Not enough points.");
        }
        int n = xVals.Length - 1; // number of segments

        List<float>[] segmentCoeffs = new List<float>[n];
        for (int i = 0; i < n; i++) {
            float w = xVals[i + 1] - xVals[i];
            float w2 = w * w;

            float yv  = yVals[i];
            float yvP = yVals[i + 1];

            float fd  = firstDerivatives[i];
            float fdP = firstDerivatives[i + 1];

            float[] coeffs = new float[4];
            coeffs[0] = yv;
            coeffs[1] = firstDerivatives[i];
            coeffs[2] = (3 * (yvP - yv) / w - 2 * fd - fdP) / w;
            coeffs[3] = (2 * (yv - yvP) / w + fd + fdP) / w2;
            segmentCoeffs[i] = PolynomialRoutines.trimPoly(coeffs).ToList();
        }
        return segmentCoeffs;
    }


}