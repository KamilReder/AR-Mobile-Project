using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneTracker : MonoBehaviour
{
    private ARPlaneManager _planeManager;
    
    public bool hasDetectedPlane;
    void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        _planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }

    private void OnDisable()
    {
        _planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        // args.added is a list of planes newly detected this frame.
        if (args.added.Count > 0)
        {
            hasDetectedPlane = true;
            Debug.Log($"[PlaneTracker] Detected {args.added.Count} new plane(s).");
        }
 
        if (args.updated.Count > 0)
        {
            // Left as a comment for later, if needed. Too noisy to fire every frame.
            // Debug.Log($"[PlaneTracker] Updated {args.updated.Count} plane(s).");
        }
 
        if (args.removed.Count > 0)
        {
            Debug.Log($"[PlaneTracker] Lost {args.removed.Count} plane(s).");
        }
    }
}
