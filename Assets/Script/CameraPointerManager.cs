using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;

    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 4.5f;
    [Range(0, 1)]
    [SerializeField] private float disPointerObject = 0.95f;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;

    [HideInInspector]
    public Vector3 hitPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection += HandleGazeSelection;
        }
    }

    private void OnDestroy()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection -= HandleGazeSelection;
        }
    }

    private void HandleGazeSelection()
    {
        if (_gazedAtObject != null)
        {
            _gazedAtObject.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point;

            if (_gazedAtObject != hit.transform.gameObject)
            {
                _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }

            if (hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
            }
        }
        else
        {
            _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
            GazeManager.Instance.CancelGazeSelection();
            PointerOutGaze();
        }
    }

    private void PointerOnGaze(Vector3 point)
    {
        pointer.SetActive(true);
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, point);
        pointer.transform.localScale = Vector3.one * scaleFactor;
        pointer.transform.position = CalculatePointerPosition(transform.position, point, disPointerObject);
    }

    private void PointerOutGaze()
    {
        pointer.SetActive(false);
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);
        return new Vector3(x, y, z);
    }
}
