using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceManager : MonoBehaviour
{
    [SerializeField] ARFaceManager faceManager;
    [SerializeField] GameObject sunGlasses;
    ARFace face;
    private GameObject eyeTracker;

    private void Awake()
    {
        eyeTracker = Instantiate(sunGlasses);
    }
    private void OnEnable()
    {
        faceManager.facesChanged += OnFaceChange;
    }

    private void OnDisable()
    {
        faceManager.facesChanged -= OnFaceChange;
    }

    private void OnFaceChange(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            face = args.updated[0];

            Vector3 eyePos = face.transform.TransformPoint(face.vertices[6]);
            eyeTracker.transform.position = eyePos;
        }
    }

    public void ChaneMaterial(Material material)
    {
        face.GetComponent<Renderer>().material = material; //안경을 material로 가능한가?
    }
}
