using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.XR.ARSubsystems;

public class PlaceItemOnPlane : MonoBehaviour
{
    [SerializeField]
    public GameObject m_ObjectToPlacePrefab;
    public GameObject[] m_SubObjectPrefab = new GameObject[3];
    private GameObject instancedGO;
    private GameObject [] subInstancedGO = new GameObject[2];//twice because there are 2 of each sub: tribal position and dismembered position
    [HideInInspector]
    public bool ObjectInstanced;
    Camera cam;
    public enum ArtState { TribalArt, DismemberedArt}
    public ArtState currentArtState;

    public float findingSquareDist = 0.5f;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private float startTime;
    private float timePassed;
    public string topArtName;
    public string middleArtName;
    public string bottomArtName;
   
    public void OnEnable()
    {
        currentArtState= ArtState.TribalArt;
        ObjectInstanced = false;
        cam = Camera.main;
        
        //sliding background
        startTime = Time.time;
    }

    public void InstantiateMainArtHolder()
    {
        ObjectInstanced = true;
        topArtName = m_SubObjectPrefab[0].gameObject.name;
        topArtName = m_SubObjectPrefab[1].gameObject.name;
        topArtName = m_SubObjectPrefab[2].gameObject.name;

        for (int i = 0; i < m_SubObjectPrefab.Length; i++)
        {
            subInstancedGO[0] = Instantiate(m_SubObjectPrefab[i], new Vector3(0,0,0), m_SubObjectPrefab[i].transform.rotation);
            subInstancedGO[1] = Instantiate(m_SubObjectPrefab[i], new Vector3(0, 0, 0), m_SubObjectPrefab[i].transform.rotation);
            //Debug.Log("Instanced object " + subInstancedGO[0].gameObject.name + " and " + subInstancedGO[1].gameObject.name);
            if (i == 0)
            {
                subInstancedGO[0].transform.parent = GameObject.FindWithTag("StickTop").transform;
                subInstancedGO[1].transform.parent = GameObject.FindWithTag("FrontArtHolder").transform;

                if(currentArtState==ArtState.TribalArt)
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("FrontArtHolder"));
                else
                {
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTop"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBar"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottomCube"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTopCapsule"));
                }       
            }
                    
            if (i == 1)
            {
                subInstancedGO[0].transform.parent = GameObject.FindWithTag("StickMiddle").transform;
                subInstancedGO[1].transform.parent = GameObject.FindWithTag("RightArtHolder").transform;

                if (currentArtState == ArtState.TribalArt)
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("RightArtHolder"));
                else
                {
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickMiddle"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBar"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottomCube"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTopCapsule"));
                }
            }
                    
            if (i == 2)
            {
                subInstancedGO[0].transform.parent = GameObject.FindWithTag("StickBottom").transform;
                subInstancedGO[1].transform.parent = GameObject.FindWithTag("LeftArtHolder").transform;

                if (currentArtState == ArtState.TribalArt)
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("LeftArtHolder"));
                else
                {
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottom"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBar"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottomCube"));
                    TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTopCapsule"));
                }
                    
            }
            //Debug.Log("Placing newly instanced art object");
            subInstancedGO[0].transform.localPosition = new Vector3(0, 0, 0);
            subInstancedGO[1].transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SwitchStickToDismembered()
    {
        if (currentArtState == ArtState.DismemberedArt)
            currentArtState = ArtState.TribalArt;
        else
            currentArtState = ArtState.DismemberedArt;

        if (ObjectInstanced)
        {
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTop"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickMiddle"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottom"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("FrontArtHolder"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("RightArtHolder"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("LeftArtHolder"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBar"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickBottomCube"));
            TraverseOnOffMeshRenderer(GameObject.FindWithTag("StickTopCapsule"));
        }
    }
    void Update()
    {
        if (ObjectInstanced == false)
        {
            Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);
            Ray ray = cam.ScreenPointToRay(center);

            if (ARCommonCached.ARRaycastManagerScript.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Debug.Log(" At PlaceItemOnPlane inside Raycast");
                Pose hitPose = s_Hits[0].pose;
                ObjectInstanced = true;
                ARCommonCached.GetMyAnalysticsScript.InstancedArtCount();//record analytics art was instantiated
                instancedGO = Instantiate(m_ObjectToPlacePrefab, hitPose.position, m_ObjectToPlacePrefab.transform.rotation);
                //Debug.Log("Instantiate: hitpose " + hitPose.position);
                InstantiateMainArtHolder();
            }
        }

        if (Input.touchCount>0)
        {
            if (Input.GetMouseButton(0) || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
            
            Ray ray = cam.ScreenPointToRay(touch.position);

                if (ARCommonCached.ARRaycastManagerScript.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                Pose hitPose = s_Hits[0].pose;
                //Vector3 hitPosePosition = hitPose.position;

                //check if user clicking on UI so object does not get placed on UI area
                bool UIInUse = ARCommonCached.GetUIControlsInUseScript.checkUIUse();
                if (!UIInUse)
                {
                    ARCommonCached.GetARSessionOriginScript.MakeContentAppearAt(instancedGO.transform, hitPose.position, instancedGO.transform.rotation);
                        //Debug.Log("Moving: hitpose " +// hitPose.position);
                    }
                ARCommonCached.GetUIControlsInUseScript.UIIsInUse = false;
            }
            }
        }        
    }

    void TraverseOnOffMeshRenderer(GameObject obj)
    {        
        if (obj.GetComponent<MeshRenderer>() != null)
            obj.GetComponent<MeshRenderer>().enabled = !obj.GetComponent<MeshRenderer>().enabled;

        if (obj.GetComponent<BoxCollider>() != null)
            obj.GetComponent<BoxCollider>().enabled = !obj.GetComponent<BoxCollider>().enabled;

        foreach (Transform child in obj.transform)
        {
            //Debug.Log("child " + child);
            TraverseOnOffMeshRenderer(child.gameObject);
        }
    }
}