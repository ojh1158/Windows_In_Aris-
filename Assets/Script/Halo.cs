using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Halo : MonoBehaviour
{
    public RectTransform arisRectTransform;
    public RectTransform haloRectTransform;

    public float speed;
    public int move;

    // Update is called once per frame
    private void Awake()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            haloRectTransform.localPosition += new Vector3(0, move);
            yield return new WaitForSeconds(speed);
            haloRectTransform.localPosition -= new Vector3(0, move);
            yield return new WaitForSeconds(speed);
        }
    }
}
