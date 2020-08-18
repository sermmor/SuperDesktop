using UnityEngine;

public static class UtilitiesInterpolation
{
    //--- Utility functions --------------------------------------------------------

    // Checks that the given array is sorted in strictly increasing order.
    // Corresponds to org.apache.commons.math3.util.MathArrays.checkOrder().
    public static void MathArrays_checkOrder(float[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] <= a[i - 1])
            {
                Debug.Log("[LogLabel.Easing] Non-monotonic sequence exception.");
            }
        }
    }
}