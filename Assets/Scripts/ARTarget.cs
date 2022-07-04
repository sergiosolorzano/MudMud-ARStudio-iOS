using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

public class ARTarget : MonoBehaviour {

    Coroutine switchingMaterials;

    public enum FocusState
    {
        Initializing,
        Finding,
        Found
    }

    public GameObject findingTarget;
    public GameObject foundTarget;
    public Image findingTargetImage;
    public Image foundTargetImage;


    Camera cam;

    //for editor version
    public float maxRayDistance = 30.0f;
    private LayerMask collisionLayerMask;
    private float findingSquareDist = 0.5f;

    private FocusState targetState;
    public FocusState TargetState
    {
        get
        {
            return targetState;
        }
        set
        {
            targetState = value;
            foundTargetImage.enabled=(targetState == FocusState.Found);
            findingTargetImage.enabled =(targetState != FocusState.Found);
        }
    }

    bool trackingInitialized;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public void OnEnable()
    {
        findingTargetImage.enabled =(true);
        foundTargetImage.enabled =(true);

        int layerIndex = LayerMask.NameToLayer("ARLivePlane");
        collisionLayerMask = 1 << layerIndex;
        TargetState = FocusState.Initializing;
        trackingInitialized = true;

        cam = Camera.main;

    }

    void Start()
    {
        
    }
    public void OnDisable()
    {
        try
        {
            findingTargetImage.enabled = false;
        }
        catch
        {
            //Debug.Log("findTarget not there");
        }
        try
        {
            foundTargetImage.enabled = false;
        }
        catch
        {
            //Debug.Log("foundTarget not here");
        }
    }
    
    void Update()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);
        Ray ray = cam.ScreenPointToRay(center);

        if (ARCommonCached.ARRaycastManagerScript.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            TargetState = FocusState.Found;
            foundTargetImage.enabled = true;
            findingTargetImage.enabled = false;
            return;
        }
        else
        {
            foundTargetImage.enabled = false;
            findingTargetImage.enabled = true;
        }

        //if you got here, we have not found a plane, so if camera is facing below horizon, display the focus "finding" square
        if (trackingInitialized)
        {
            TargetState = FocusState.Finding;
            foundTargetImage.enabled = false;
            findingTargetImage.enabled = true;
        }
    }
}
