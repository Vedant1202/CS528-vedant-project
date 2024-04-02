using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChanger : MonoBehaviour
{

    // Reference to the material attached to the cube
    public Material cubeMaterial;

    // Shader to be applied
    public Shader newShader1;
    public Shader newShader2;
    public Shader newShader3;
    public Color newColor1;
    public Color newColor2;
    public Color newColor3;

    public string type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeMaterialShader();
    }

    // Function to change the shader of the material
    public void ChangeMaterialShader()
    {
        if (cubeMaterial != null)
        {
            if (type == "a")
            {
                //cubeMaterial.shader = newShader1;
                cubeMaterial.SetColor("_Color", newColor1);
            }
            else if (type == "b")
            {
                //cubeMaterial.shader = newShader2;
                cubeMaterial.SetColor("_Color", newColor2);
            }
            else if (type == "c")
            {
                //cubeMaterial.shader = newShader3;
                cubeMaterial.SetColor("_Color", newColor3);
            }
        }
        else
        {
            Debug.LogError("Cube material or new shader is not assigned!");
        }
    }

    // Called whenever the value of a public variable is changed in the Unity Editor
    private void OnValidate()
    {
        ChangeMaterialShader();
    }
}
