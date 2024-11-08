using UnityEngine;

public static class VectorAngleUtils
{
    /// <summary>
    /// Gets the horizontal (yaw) angle difference between two vectors
    /// </summary>
    public static float GetHorizontalAngleDifference(Vector3 fromVector, Vector3 toVector)
    {
        // Project vectors onto the horizontal plane (XZ)
        Vector3 fromHorizontal = new Vector3(fromVector.x, 0, fromVector.z);
        Vector3 toHorizontal = new Vector3(toVector.x, 0, toVector.z);

        // Calculate the signed angle between the projected vectors
        float angle = Vector3.SignedAngle(fromHorizontal, toHorizontal, Vector3.up);

        // Normalize angle to -180 to 180 range
        return NormalizeAngle(angle);
    }

    /// <summary>
    /// Gets the vertical (pitch) angle difference between two vectors
    /// </summary>
    public static float GetVerticalAngleDifference(Vector3 fromVector, Vector3 toVector)
    {
        // Calculate the angles between each vector and the horizontal plane
        float fromVerticalAngle = Vector3.Angle(fromVector, Vector3.up) - 90;
        float toVerticalAngle = Vector3.Angle(toVector, Vector3.up) - 90;

        // Return the difference
        return NormalizeAngle(toVerticalAngle - fromVerticalAngle);
    }

    /// <summary>
    /// Normalizes an angle to be between -180 and 180 degrees
    /// </summary>
    private static float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}