//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DistanceFromSolHandler : MonoBehaviour
//{
//    private float distanceFromSol;
//    public string displayText = "Distance from sol is xxx parsecs";
//    public OmicronStatusGUI text;

//    // Start is called before the first frame update
//    void Start()
//    {

//        UpdateDisplayText(0);
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    public void UpdateDisplayText (float distanceFromSol)
//    {
//        this.distanceFromSol = distanceFromSol;
//        string[] parts = displayText.Split(new string[] { "xxx" }, StringSplitOptions.None);
//        text.tex = parts[0] + this.distanceFromSol.ToString() + parts[1];
//    }
//}
