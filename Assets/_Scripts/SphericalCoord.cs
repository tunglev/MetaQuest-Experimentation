using System;
using UnityEngine;

[Serializable]
public class SphericalCoord
{
    public float r { get; private set; }
    public float theta { get; private set; }
    public float phi { get; private set; }

    public SphericalCoord(float r, float theta, float phi)
    {
        SetR(r);
        SetTheta(theta);
        SetPhi(phi);
    }
    public void SetR(float val)
    {
        if (r < 0) throw new ArgumentOutOfRangeException("r should be non negative");
        r = val;
    }

    public void SetTheta(float val)
    {
        if (val < 0 || val > Mathf.PI * 2) throw new ArgumentOutOfRangeException();
        theta = val;
    }
    public void SetPhi(float val)
    {
        if (val < 0 || val > Mathf.PI * 2) throw new ArgumentOutOfRangeException();
        phi = val;
    }

    public Vector3 ToCartesian()
    {
        float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = r * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = r * Mathf.Cos(phi);
        return new Vector3(x, y, z);
    }

    public override string ToString()
    {
        return $"(r: {r}m, theta: {theta * Mathf.Rad2Deg}deg, phi: {phi * Mathf.Rad2Deg}deg)";
    }
}
