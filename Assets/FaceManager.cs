using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class FaceManager : MonoBehaviour
{
    public static FaceManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] GameObject[] glassesPrefab;
    private GameObject curGlasses;

    [SerializeField] ARFaceManager faceManager;

    Vector3 eyePos;
    ARFace face;

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
        if (args.added.Count > 0)
        {
            face = args.added[0];
            FitGlasses(glassesPrefab[0]);
        }
        if (args.updated.Count > 0)
        {
            face = args.updated[0];

            eyePos = face.transform.TransformPoint(face.vertices[6]);
            curGlasses.transform.position = eyePos;
            curGlasses.transform.rotation = Quaternion.identity;
            Debug.Log($"{eyePos}, {Quaternion.identity}");
            
        }
        //remove
    }

    private void FitGlasses(GameObject glassesPrefab)
    {
        if (curGlasses != null)
        {
            Destroy(curGlasses);
        }
        curGlasses = Instantiate(glassesPrefab, face.transform);
        Debug.Log($"{glassesPrefab}, »ı¼ºµÊ!");
    }

    public void SelectGlasses(int index)
    {
        if (index >= 0 && index < glassesPrefab.Length)
        {
            FitGlasses(glassesPrefab[index]);
        }
    }
}
