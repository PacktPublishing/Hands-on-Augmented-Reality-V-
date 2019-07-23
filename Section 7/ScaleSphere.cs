using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSphere : MonoBehaviour
{


    public bool expandIt;
    private Vector3 originalSize = new Vector3(0.01f, 0.01f, 0.0001f);

    void Update()
    {
        if (expandIt)
            transform.localScale += new Vector3(0.001f, 0.001f, 0.00001f);
        else
        {
            if (transform.localScale != originalSize)
                transform.localScale = originalSize;
        }
    }
}
