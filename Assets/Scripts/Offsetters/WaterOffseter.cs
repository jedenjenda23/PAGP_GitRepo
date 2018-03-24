using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOffseter : MonoBehaviour
{
    public float waterSpeed;

    Material myMaterial;
	// Use this for initialization
	void Start ()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float newOffset = Time.time * waterSpeed;
        Vector2 newVector = new Vector2(newOffset, 0);

        myMaterial.SetTextureOffset("_MainTex", newVector);
        myMaterial.SetTextureOffset("_DetailAlbedoMap", -newVector);

    }
}
