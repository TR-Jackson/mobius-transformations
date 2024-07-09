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

    public GameObject circle;
    public GameObject line;

    void Start()
    {
        beta = new Complex(beta_real, beta_imag);
    }

    void Update()
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
}
