
using UnityEngine;


public enum CameraType
{
    main,
    house,
    talk,
    highlight
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

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject houseCamera;
    [SerializeField] private GameObject highlightCamera;
    [SerializeField] private GameObject talkCamera;


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
        }

        return null;
    }
    


}
