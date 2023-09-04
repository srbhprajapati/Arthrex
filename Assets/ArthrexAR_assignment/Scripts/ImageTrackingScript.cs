using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingScript : MonoBehaviour
{

    public GameObject CustomArtInstance;
    public GameObject CustomArtBackground;
    
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += TrackedImageManager_trackedImagesChanged;
    }


    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= TrackedImageManager_trackedImagesChanged;
    }


    private void TrackedImageManager_trackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage trackedImage in obj.added)
        {
            Vector3 position = trackedImage.transform.position;
            Quaternion rotation = trackedImage.transform.rotation;
            CustomArtInstance.transform.position = position;
            CustomArtInstance.transform.rotation = Quaternion.identity;
            CustomArtInstance.transform.Rotate(rotation.eulerAngles);
            CustomArtInstance.SetActive(true);

            CustomArtBackground.transform.position = position;
            CustomArtBackground.transform.rotation = Quaternion.identity;
            CustomArtBackground.transform.Rotate(rotation.eulerAngles);
            CustomArtBackground.SetActive(true);
        }
        foreach (ARTrackedImage trackedImage in obj.updated)
        {
            Vector3 position = trackedImage.transform.position;
            Quaternion rotation = trackedImage.transform.rotation;
            CustomArtInstance.transform.position = position;
            CustomArtInstance.transform.rotation = Quaternion.identity;
            CustomArtInstance.transform.Rotate(rotation.eulerAngles);
            CustomArtInstance.SetActive(true);

            CustomArtBackground.transform.position = position;
            CustomArtBackground.transform.rotation = Quaternion.identity;
            CustomArtBackground.transform.Rotate(rotation.eulerAngles);
            CustomArtBackground.SetActive(true);
        }
        foreach (ARTrackedImage trackedImage in obj.removed)
        {
            CustomArtInstance.SetActive(false);
            CustomArtBackground.SetActive(false);
        }
    }

}
