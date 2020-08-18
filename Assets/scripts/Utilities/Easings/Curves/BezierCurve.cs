using UnityEngine;

public static class BezierCurve
{
    // These values are established by empiricism with tests (tradeoff: performance VS precision)
    const int NEWTON_ITERATIONS = 4;
    const float NEWTON_MIN_SLOPE = 0.001f;
    const float SUBDIVISION_PRECISION = 0.0000001f;
    const int SUBDIVISION_MAX_ITERATIONS = 10;

    const int kSplineTableSize = 11;
    const float kSampleStepSize = 1.0f / (kSplineTableSize - 1.0f);

    const bool float32ArraySupported = true;

    static float A (float aA1, float aA2) => 1.0f - 3.0f * aA2 + 3.0f * aA1;
    static float B (float aA1, float aA2) => 3.0f * aA2 - 6.0f * aA1;
    static float C (float aA1) => 3.0f * aA1;

    // Returns x(t) given t, x1, and x2, or y(t) given t, y1, and y2.
    static float calcBezier (float aT, float aA1, float aA2) => ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;

    // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
    static float getSlope(float aT, float aA1, float aA2) => 3.0f * A(aA1, aA2) * aT * aT + 2.0f * B(aA1, aA2) * aT + C(aA1);

    static float binarySubdivide(float aX, float aA, float aB, float mX1, float mX2) {
        float currentX, currentT;
        int i = 0;
        do
        {
            currentT = aA + (aB - aA) / 2.0f;
            currentX = calcBezier(currentT, mX1, mX2) - aX;
            if (currentX > 0.0)
            {
                aB = currentT;
            }
            else
            {
                aA = currentT;
            }
        } while (Mathf.Abs(currentX) > SUBDIVISION_PRECISION && ++i < SUBDIVISION_MAX_ITERATIONS);
        return currentT;
    }

    static float newtonRaphsonIterate(float aX, float aGuessT, float mX1, float mX2)
    {
        for (int i = 0; i < NEWTON_ITERATIONS; ++i)
        {
            float currentSlope = getSlope(aGuessT, mX1, mX2);
            if (currentSlope == 0.0f)
            {
                return aGuessT;
            }
            float currentX = calcBezier(aGuessT, mX1, mX2) - aX;
            aGuessT -= currentX / currentSlope;
        }
        return aGuessT;
    }

    static float LinearEasing(float x) => x;

    public static System.Func<float, float> CreateBezier(float mX1, float mY1, float mX2, float mY2)
    {
        if (!(0 <= mX1 && mX1 <= 1 && 0 <= mX2 && mX2 <= 1))
        {
            Debug.Log("[LogLabel.Easing] bezier x values must be in [0, 1] range");
        }

        if (mX1 == mY1 && mX2 == mY2)
        {
            return LinearEasing;
        }

        // Precompute samples table
        float[] sampleValues = new float[kSplineTableSize];
        for (int i = 0; i < kSplineTableSize; ++i)
        {
            sampleValues[i] = calcBezier(i * kSampleStepSize, mX1, mX2);
        }

        System.Func<float, float> getTForX = (float aX) =>
        {
            float intervalStart = 0.0f;
            int currentSample = 1;
            int lastSample = kSplineTableSize - 1;

            for (; currentSample != lastSample && sampleValues[currentSample] <= aX; ++currentSample)
            {
                intervalStart += kSampleStepSize;
            }
            --currentSample;

            // Interpolate to provide an initial guess for t
            float dist = (aX - sampleValues[currentSample]) / (sampleValues[currentSample + 1] - sampleValues[currentSample]);
            float guessForT = intervalStart + dist * kSampleStepSize;

            float initialSlope = getSlope(guessForT, mX1, mX2);
            if (initialSlope >= NEWTON_MIN_SLOPE)
            {
                return newtonRaphsonIterate(aX, guessForT, mX1, mX2);
            }
            else if (initialSlope == 0.0f)
            {
                return guessForT;
            }
            else
            {
                return binarySubdivide(aX, intervalStart, intervalStart + kSampleStepSize, mX1, mX2);
            }
        };

        // BezierEasing
        return (float x) =>
        {
            // Because JavaScript number are imprecise, we should guarantee the extremes are right.
            if (x == 0)
            {
                return 0;
            }
            if (x == 1)
            {
                return 1;
            }
            return calcBezier(getTForX(x), mY1, mY2);
        };
    }
}