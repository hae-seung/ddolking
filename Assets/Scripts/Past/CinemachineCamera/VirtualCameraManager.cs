
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public enum CameraType
{
    main,
    house,
    talk,
    highlight,
    dungeon
}

public class VirtualCameraManager : MonoBehaviour
{
    public static VirtualCameraManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        TurnOffCamers();
        houseCamera.SetActive(true);
    }

    private void TurnOffCamers()
    {
        mainCamera.SetActive(false);
        houseCamera.SetActive(false);
        highlightCamera.SetActive(false);
        talkCamera.SetActive(false);
    }

    [SerializeField] private List<GameObject> cameras;
    
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject houseCamera;
    [SerializeField] private GameObject highlightCamera;
    [SerializeField] private GameObject talkCamera;
    [SerializeField] private GameObject dungeonCamera;


    public GameObject GetCamera(CameraType type)
    {
        switch (type)
        {
            case CameraType.main :
                return mainCamera;
            case CameraType.house:
                return houseCamera;
            case CameraType.highlight:
                return highlightCamera;
            case CameraType.talk:
                return talkCamera;
            case CameraType.dungeon:
                return dungeonCamera;
        }

        return null;
    }

    public CinemachineVirtualCamera GetCamera()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i].activeSelf)
                return cameras[i].GetComponent<CinemachineVirtualCamera>();
        }

        return null;
    }

}
