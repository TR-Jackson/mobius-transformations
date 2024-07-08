using System;
using System.Linq.Expressions;
using UnityEngine;

public class SphereBodyPosition : MonoBehaviour
{
    public double phi;
    public double theta;

    public GameObject planeBody;
    public GameObject sphere;

    float getRadius()
    {
        return sphere.transform.localScale.x / 2;
    }
    float getX()
    {
        return (float)(getRadius() * Math.Sin(theta) * Math.Cos(phi) + sphere.transform.position.x);
    }

    float getY()
    {
        return (float)(getRadius() * Math.Sin(theta) * Math.Sin(phi) + sphere.transform.position.y);
    }

    float getZ()
    {
        return (float)(getRadius() * Math.Cos(theta) + sphere.transform.position.z);
    }

    // Take the coords of the body on the plane and update the spherical coords to match
    void plane_to_sphere()
    {
        double m = planeBody.transform.position.x;
        double n = planeBody.transform.position.z;

        double r = sphere.transform.localScale.x / 2;
        double cx = sphere.transform.position.x;
        double cy = sphere.transform.position.y;
        double cz = sphere.transform.position.z;

        double a1 = cx - m;
        double a2 = m - cx;

        double b1 = cy + r;
        double b2 = -cy;

        double c1 = cz - n;
        double c2 = n - cz;

        double a = Math.Pow(a1, 2) + Math.Pow(b1, 2) + Math.Pow(c1, 2);
        double b = 2 * (a1 * a2 + b1 * b2 + c1 * c2);
        double c = Math.Pow(a2, 2) + Math.Pow(b2, 2) + Math.Pow(c2, 2) - Math.Pow(r, 2);

        double lambda1 = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
        double lambda2 = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);

        double tolerance = 1e-6;
        double lambda;

        if (Mathf.Abs((float)lambda1 - 1) < tolerance) lambda = lambda2;
        else if (Mathf.Abs((float)lambda2 - 1) < tolerance) lambda = lambda1;
        else
        {
            Debug.Log($"Invalid solutions {lambda1}, {lambda2} for a={a}, b={b}, c={c}");
            return;
        }

        double px = (cx - m) * lambda + m;
        double py = (cy + r) * lambda;
        double pz = (cz - n) * lambda + n;

        // replace with updating polar coords, end goal is function not to directly change position
        transform.position = new UnityEngine.Vector3((float)px, (float)py, (float)pz);
    }

    void Start()
    {
    }

    void Update()
    {
        plane_to_sphere();
        //transform.position = new UnityEngine.Vector3(getX(), getY(), getZ());
    }
}
