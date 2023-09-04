using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlingScript : MonoBehaviour
{

    public GameObject CustomArtInstance;

    private Vector2 lastMousePosition;
    private int inputGridSize = 50;
    private float acceleration = 1.0f;
    private float scalingFactor = 4.0f;

    private float[] positions;
    private float[] velocities;
    private float offsetToCenterOfPixel;
    
    private Texture2D inputTexture;
    private RenderTexture inputRenderTexture;

    void Start()
    {
        positions = new float[inputGridSize * inputGridSize];
        velocities = new float[inputGridSize * inputGridSize];

        //Set Initial Animation
        for(int x=0; x< inputGridSize; x++)
        {
            for (int y = 0; y < inputGridSize; y++)
            {
                float positionInFloat = (float)x / inputGridSize;
                positions[x * inputGridSize + y] = positionInFloat;
                velocities[x * inputGridSize + y] = 0.0f;
            }
        }

        offsetToCenterOfPixel = (2.0f / inputGridSize) / 2.0f;


        inputRenderTexture = new RenderTexture(inputGridSize, inputGridSize, 0, RenderTextureFormat.RFloat);
        inputTexture = new Texture2D(inputGridSize, inputGridSize, TextureFormat.RFloat, true);
        
        UpdateInputTextures();
    }

    void Update()
    {
        bool isTouchPressed = false;
        float xTouchInTextureSpace = 0.0f;
        float yTouchInTextureSpace = 0.0f;

        if (Input.touchCount > 0 && lastMousePosition != Input.GetTouch(0).position)
        {
            RaycastHit hit;
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 hitPointLocal = hit.transform.InverseTransformPoint(hit.point);

                //Scale by factor of 2. Plane is in (-0.5,0.5) space 
                xTouchInTextureSpace = 2.0f * hitPointLocal.z;
                yTouchInTextureSpace = 2.0f * hitPointLocal.x;
                isTouchPressed = true;
            }
            
            lastMousePosition = touch.position;
        }


        UpdatePositionAndVelocity(isTouchPressed, xTouchInTextureSpace, yTouchInTextureSpace, Time.deltaTime);
        UpdateInputTextures();
    }


    private void UpdatePositionAndVelocity(bool isTouchPressed, float touchPositionX, float touchPositionY,float time)
    {
        for (int x = 0; x < inputGridSize; x++)
        {
            for (int y = 0; y < inputGridSize; y++)
            {

                float position = positions[x * inputGridSize + y];
                position *= scalingFactor;
                float velocity = velocities[x * inputGridSize + y];

                //Texture Coordinates in -1,1 space
                float xCoord = 2.0f * (float)x / inputGridSize - 1.0f + offsetToCenterOfPixel;
                float yCoord = 2.0f * (float)y / inputGridSize - 1.0f + offsetToCenterOfPixel;

                float distance = Mathf.Sqrt((touchPositionX - xCoord) * (touchPositionX - xCoord) +
                                      (touchPositionY - yCoord) * (touchPositionY - yCoord));
                float amplitude = (isTouchPressed && distance < 0.2f) ? 5.0f / ((distance + 1.0f) * (distance + 1.0f)) : 0.0f;

                //Using a combination of velocity and acceleration push on particles
                velocity += (amplitude - acceleration) * time + amplitude * 0.06f * (1.0f - position / scalingFactor);
                position = position + velocity * time + 0.5f * (-acceleration) * time * time;


                position /= scalingFactor;
                if (position > 1.0f) position = 1.0f;

                if (position < 0.0f)
                {
                    velocity = 0.0f;
                    position = 0.0f;
                }

                positions[x * inputGridSize + y] = position;
                velocities[x * inputGridSize + y] = velocity;
            }
        }
    }

    private void UpdateInputTextures()
    {
        inputTexture.SetPixelData(positions, 0);
        inputTexture.Apply();

        Graphics.Blit(inputTexture, inputRenderTexture);

        if (CustomArtInstance != null && CustomArtInstance.GetComponent<ProceduralCustomArtScript>() != null)
        {
            CustomArtInstance.GetComponent<ProceduralCustomArtScript>().UpdateInputTexture(ref inputRenderTexture);
        }
    }
}
