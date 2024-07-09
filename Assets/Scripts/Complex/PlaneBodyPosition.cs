using UnityEngine;
using System.Numerics;
using System;

public class PlaneBodyPosition : MonoBehaviour
{
    public GameObject sphereBody;
    public GameObject sphere;

    public bool isControlling = true;

    // For position on plane
    public Complex pos = new Complex(5, 0);
    bool at_infinity = false;

    MeshRenderer mr;
    Rigidbody rb;

    float getX()
    {
        return (float) pos.Real;
    }
    float getY()
    {
        return transform.position.y;
    }

    float getZ()
    {
        return (float) pos.Imaginary;
    }

    // Take spherical coords and update plane position
    void sphere_to_plane()
    {
        double r = sphere.transform.localScale.x/2;
        double cx = sphere.transform.position.x;
        double cy = sphere.transform.position.y;
        double cz = sphere.transform.position.z;
        double px = sphereBody.transform.position.x;
        double py = sphereBody.transform.position.y;
        double pz = sphereBody.transform.position.z;


        double posReal = (px - cx) * (cy + r) / (r + cy - py) + cx;
        double posImag = (pz - cz) * (cy + r) / (r + cy - py) + cz;

        pos = new Complex(posReal, posImag);
    }

    void update_plane_pos()
    {
        UnityEngine.Vector3 newPlayerPos = new UnityEngine.Vector3(getX(), getY(), getZ());
        transform.position = newPlayerPos;
    }

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!isControlling)
        {
            sphere_to_plane();
            update_plane_pos();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MobiusTransform(new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)), 0, 0, 1);
        }
    }

    void MobiusTransform(Complex a, Complex b, Complex c, Complex d)
    {
        if (c*pos + d == Complex.Zero) 
        {
            pos = Complex.Zero;
            at_infinity = true;
            mr.enabled = false;
        } else
        {
            mr.enabled = true;
            at_infinity = false;
            pos = (a * pos + b) / (c * pos + d);
        }
        update_plane_pos();
    }
}
