using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStar", menuName = "Assets/NewStar")]

public class Star : ScriptableObject
{
    public int hip;
    public float dist;
    public float x_zero;
    public float y_zero;
    public float z_zero;
    public float mag;
    public float absmag;
    public float vx;
    public float vy;
    public float vz;
    public string spect;
}
