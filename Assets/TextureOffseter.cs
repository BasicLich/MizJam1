using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffseter : MonoBehaviour
{
    public float offsetX, offsetY;
    public float frequencyOsc = 1f;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        rend.material.mainTextureOffset += new Vector2(Mathf.Sin(Time.time * frequencyOsc) * offsetX, offsetY) * Time.deltaTime;
    }
}
