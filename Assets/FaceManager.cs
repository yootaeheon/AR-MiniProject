using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class FaceManager : MonoBehaviour
{
    [SerializeField] GameObject[] glassesPrefab;  // 안경 프리팹을 담을 배열 생성
    private GameObject curGlasses;                // 현재 안경

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
        if (args.added.Count > 0)  // 처음 얼굴을 인식 하였을 때
        {
            face = args.added[0];
            FitGlasses(glassesPrefab[0]);  // 0번째 안경을 기본 안경으로 생성
        }
        if (args.updated.Count > 0)
        {
            face = args.updated[0];  

            eyePos = face.transform.TransformPoint(face.vertices[6]);  // face tracking 정점에 6번(미간 살짝 아래)에 안경 프리팹을 생성
            curGlasses.transform.position = eyePos;                    // 계속 얼굴을 인식하며 안경을 얼굴에 맞게 위치와 회전을 Update
            curGlasses.transform.rotation = face.transform.rotation;
        }
    }

    private void FitGlasses(GameObject glassesPrefab)
    {
        if (curGlasses != null)  // 이미 착용한 안경이 있을 시 파괴하고 새로운 안경 생성
        {
            Destroy(curGlasses);
        }
        curGlasses = Instantiate(glassesPrefab, face.transform);
    }

    public void SelectGlasses(int index)  // UI버튼으로 index를 선택하여 원하는 안경 선택하여 피팅
    {
        if (index >= 0 && index < glassesPrefab.Length)
        {
            FitGlasses(glassesPrefab[index]);
        }
    }
}
