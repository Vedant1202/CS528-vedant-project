//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class CSV_Reader : MonoBehaviour
//{
//    public TextAsset textAssetData;

//    [System.Serializable]
//    public class Star
//    {
//        public int hip;
//        public float dist;
//        public float x_zero;
//        public float y_zero;
//        public float z_zero;
//        public float mag;
//        public float absmag;
//        public float vx;
//        public float vy;
//        public float vz;
//        public string spec;
//    }

//    [System.Serializable]
//    public class StarList
//    {
//        public Star[] star;
//    }

//    public StarList myStarList = new StarList();

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    void ReadCSV()
//    {
//        string[] data = textAssetData.text.Split(new string[] {',', '\n'}, StringSplitOptions.None);

//        int tableSize = (data.Length / 4) - 1;

//        for(int i = 0; i < tableSize; i++)
//        {
//            myStarList.star[i] = new Star();
//            myStarList.star[i].hip = int.Parse(data[4 * (i + 1) + 1]);
//            myStarList.star[i].dist = float.Parse(data[4 * (i + 1) + 2]);
//            myStarList.star[i].x_zero = float.Parse(data[4 * (i + 1) + 3]);
//            myStarList.star[i].y_zero = float.Parse(data[4 * (i + 1) + 4]);
//            myStarList.star[i].z_zero = float.Parse(data[4 * (i + 1) + 5]);
//            myStarList.star[i].mag = float.Parse(data[4 * (i + 1) + 6]);
//            myStarList.star[i].absmag = float.Parse(data[4 * (i + 1) + 7]);
//            myStarList.star[i].vx = float.Parse(data[4 * (i + 1) + 8]);
//            myStarList.star[i].vy = float.Parse(data[4 * (i + 1) + 9]);
//            myStarList.star[i].vz = float.Parse(data[4 * (i + 1) + 10]);
//            myStarList.star[i].spec = data[4 * (i + 1) + 11];
//        }
//    }
//}
