using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour 
{
    public bool scrolling, paralax;
    public float imageSize;
    public float viewZone = 1.15f;
    public float paralaxSpeed;
    public float paralaxSmooting;

    Transform   cameraTransform;
    Transform[] imageTransforms;

    int leftIndex;
    int rightIndex;
    int childCount;


    float lastCameraX;

    // Use this for initialization
    void Start () 
    {
        childCount = transform.childCount;
 
        cameraTransform = Camera.main.transform;
        imageTransforms = new Transform[childCount];

        lastCameraX = cameraTransform.position.x;

        for (int i = 0; i < childCount; i++)
            imageTransforms[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(paralax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            //transform.position += Vector3.right * (deltaX * (paralaxSpeed / paralaxSmooting));
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x < (imageTransforms[leftIndex].position.x + viewZone))
                ScrollLeft();
            if (cameraTransform.position.x > (imageTransforms[rightIndex].position.x - viewZone))
                ScrollRight();
        }

    }

    void ScrollLeft()
    {
        float x = imageTransforms[leftIndex].position.x - imageSize;
        float y = imageTransforms[leftIndex].position.y;

        imageTransforms[rightIndex].position = new Vector3(x, y, 0);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = childCount - 1;
    }

    void ScrollRight()
    {
        float x = imageTransforms[rightIndex].position.x + imageSize;
        float y = imageTransforms[rightIndex].position.y;

        imageTransforms[leftIndex].position = new Vector3(x, y, 0);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == childCount)
            leftIndex = 0;
    }
}
