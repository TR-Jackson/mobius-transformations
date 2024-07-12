using System.Numerics;
using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float alpha;
    public float beta_real;
    public float beta_imag;
    Complex beta;
    public float gamma;

    public GameObject sphere;

    public GameObject circle;
    public GameObject line;

    public GameObject tempp;
    public GameObject tempq;
    public GameObject tempc;
    public GameObject tempn;
    public GameObject tempk;

    private void SetSpherePlatform()
    {
        if (alpha != 0)
        {
            Complex centre = -beta / alpha;
            double r1 = Math.Sqrt(1 / alpha * (Math.Pow(beta.Magnitude, 2) - gamma));
            UnityEngine.Vector3 c1 = new UnityEngine.Vector3((float)centre.Real, 0, (float)centre.Imaginary);

            UnityEngine.Vector3 cs = sphere.transform.position;
            double rs = sphere.transform.localScale.x / 2;
            UnityEngine.Vector3 N = cs + new UnityEngine.Vector3(0, (float)rs, 0); // north pole

            UnityEngine.Vector3 p1 = c1 + new UnityEngine.Vector3((float)r1, 0, 0);
            UnityEngine.Vector3 q1 = c1 - new UnityEngine.Vector3((float)r1, 0, 0);
            UnityEngine.Vector3 k1 = c1 + new UnityEngine.Vector3(0, 0, (float)r1);

            double lambdap = -2 * rs * (p1.y - cs.y - rs) / (Math.Pow(p1.x - cs.x, 2) + Math.Pow(p1.y - cs.y - rs, 2) + Math.Pow(p1.z - cs.z, 2));
            double lambdaq = -2 * rs * (q1.y - cs.y - rs) / (Math.Pow(q1.x - cs.x, 2) + Math.Pow(q1.y - cs.y - rs, 2) + Math.Pow(q1.z - cs.z, 2));
            double lambdak = -2 * rs * (k1.y - cs.y - rs) / (Math.Pow(k1.x - cs.x, 2) + Math.Pow(k1.y - cs.y - rs, 2) + Math.Pow(k1.z - cs.z, 2));

            UnityEngine.Vector3 p2 = (p1 - N) * (float)lambdap + N;
            UnityEngine.Vector3 q2 = (q1 - N) * (float)lambdaq + N;
            UnityEngine.Vector3 k2 = (k1 - N) * (float)lambdak + N;

            UnityEngine.Vector3 c2 = (p2 - q2) * 0.5f + q2;
            UnityEngine.Vector3 n = UnityEngine.Vector3.Cross(p2 - c2, k2 - c2).normalized; // normal to circle on sphere
            double r2 = (0.5f * (p2 - q2)).magnitude;

            Debug.Log($"k={k2}, lambda = {lambdak}");

            tempc.transform.position = c2;
            tempp.transform.position = p2;
            tempq.transform.position = q2;
            tempn.transform.position = c2 - n;
            tempk.transform.position = k2; 
        }
    }
    private void SetPlanePlatform()
    {
        beta = new Complex(beta_real, beta_imag);

        if (alpha != 0) // platform is a circle
        {
            // for circle |z - c| = r
            double radius = Math.Sqrt(1 / alpha * (Math.Pow(beta.Magnitude, 2) - gamma));
            Complex centre = -beta / alpha;

            circle.transform.position = new UnityEngine.Vector3((float)centre.Real, 0f, (float)centre.Imaginary);
            // default cylinder radius is 0.5 so for radius of x set scale to 2x
            circle.transform.localScale = new UnityEngine.Vector3((float)(2 * radius), 0.1f, (float)(2 * radius));

            line.SetActive(false);
            circle.SetActive(true);
        }
        else // platform is a line
        {
            // for line ax + bz + c = 0
            double a = 2 * beta.Real;
            double b = 2 * beta.Imaginary;
            double c = gamma;

            double angle = Math.Atan2((float)b, (float)a) * Mathf.Rad2Deg;
            line.transform.rotation = UnityEngine.Quaternion.Euler(0, (float)angle, 0);

            line.SetActive(true);
            circle.SetActive(false);
        }
    }
    void Start()
    {
        beta = new Complex(beta_real, beta_imag);
    }

    void Update()
    {
        SetPlanePlatform();
        SetSpherePlatform();
    }
}
