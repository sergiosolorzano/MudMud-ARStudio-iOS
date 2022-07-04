using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ARCommonCached : MonoBehaviour {

    public static GameObject GetARSessionOriginGO;
    public static ARSessionOrigin GetARSessionOriginScript;
    public static ARRaycastManager ARRaycastManagerScript;
    public static GameObject GetARSessionGO;
    public static ARSession GetARSessionScript;
    public static ARPlaneManager GetARPlaneManagerScript;
    public static UIControlsInUse GetUIControlsInUseScript;
    public static ArtItemLevitate GetArtItemLevitateScript;
    public static Canvas GetOnGameCanvas;
    public static Canvas SelectionCanvas;
    public static ARPointCloudManager GetARPointCloudManagerScript;
    //public static GameObject GetTargetGO;
    public static PlaceItemOnPlane GetPlaceItemOnPlaneScript;
    public static ARTarget GetARTargetScript;
    public static GameObject GetTutorialAnimObject;
    public static Image GetTutorialAnimImage;
    public static SpriteAnim GetSpriteAnimScript;
    public static Button GetPlaceMeButtonObject;
    public static Light GetLightObject;
    public static Text GetPhotoMssg;
    public static MyAnalystics GetMyAnalysticsScript;

    void OnEnable()
    {
        //Standard
        if (GameObject.FindWithTag("ARSession") != null)
        {
            GetARSessionGO= GameObject.FindWithTag("ARSession");
        }
        else
        {
            //Debug.Log("Can't find ARSessionGO");
        }

        if (GetARSessionGO.GetComponent<ARSession>() != null)
        {
            GetARSessionScript = GetARSessionGO.GetComponent<ARSession>();
        }
        else
        {
            //Debug.Log("Can't find ARSessionScript");
        }

        if (GameObject.FindWithTag("Root") != null)
        {
            GetARSessionOriginGO = GameObject.FindWithTag("Root");
        }
        else
        {
            //Debug.Log("Can't find Root");
        }

        if (GetARSessionOriginGO.GetComponent<ARRaycastManager>() != null)
        {
            ARRaycastManagerScript = GameObject.FindWithTag("Root").GetComponent<ARRaycastManager>();
        }
        else
        {
            //Debug.Log("Can't find ARRaycastManagerScript");
        }

        if (GetARSessionOriginGO.GetComponent<ARSessionOrigin>() != null)
        {
            GetARSessionOriginScript = GameObject.FindWithTag("Root").GetComponent<ARSessionOrigin>();
        }
        else
        {
            //Debug.Log("Can't find ARSessionOriginScript");
        }

        if (GameObject.FindWithTag("ObjectManager").GetComponent<UIControlsInUse>() != null)
        {
            GetUIControlsInUseScript = GameObject.FindWithTag("ObjectManager").GetComponent<UIControlsInUse>();
            GetUIControlsInUseScript.enabled = false;
        }
        else
        {
            //Debug.Log("Can't find UIControlsInUse");
        }

        if (GameObject.FindWithTag("ObjectManager").GetComponent<MyAnalystics>() != null)
        {
            GetMyAnalysticsScript = GameObject.FindWithTag("ObjectManager").GetComponent<MyAnalystics>();
        }
        else
        {
            //Debug.Log("Can't find MyAnalytics");
        }

        if (GameObject.FindWithTag("ObjectManager").GetComponent<ArtItemLevitate>() != null)
            GetArtItemLevitateScript = GameObject.FindWithTag("ObjectManager").GetComponent<ArtItemLevitate>();
        else
        {
            //Debug.Log("Can't find ArtItemLevitate");
        }

        if (GameObject.FindWithTag("ObjectManager").GetComponent<PlaceItemOnPlane>() != null)
        {
            GetPlaceItemOnPlaneScript = GameObject.FindWithTag("ObjectManager").GetComponent<PlaceItemOnPlane>();
        }
        else
        {
            //Debug.Log("Can't find PlaceItemOnPlane");
        }

        if (GetARSessionOriginGO.GetComponent<ARPlaneManager>() != null)
        {
            GetARPlaneManagerScript = GetARSessionOriginGO.GetComponent<ARPlaneManager>();
        }
        else
        {
            //Debug.Log("Can't find ARPlaneManager");
        }

        if (GetARSessionOriginGO.GetComponent<ARPointCloudManager>() != null)
        {
            GetARPointCloudManagerScript = GetARSessionOriginGO.GetComponent<ARPointCloudManager>();
        }
        else
        {
            //Debug.Log("Can't find Found ARPointCloudManager");
        }

        if (GameObject.FindWithTag("ObjectManager").GetComponent<ARTarget>() != null)
        {
            GetARTargetScript = GameObject.FindWithTag("ObjectManager").GetComponent<ARTarget>();
        }
        else
        {
            //Debug.Log("Can't find ARTarget");
        }

        foreach(Transform child in Camera.main.transform)
        {
            if (child.gameObject.tag=="SelectionCanvas")
            {
                SelectionCanvas = child.gameObject.GetComponent<Canvas>();
            }
        }

        if (GameObject.FindWithTag("TutorialAnim") != null)
        {
            GetTutorialAnimObject = GameObject.FindWithTag("TutorialAnim");
        }
        else
        {
            //Debug.Log("Can't find TutorialAnim");
        }

        if (GetTutorialAnimObject.GetComponent<Image>() != null)
        {
            GetTutorialAnimImage = GetTutorialAnimObject.GetComponent<Image>();
        }
        else
        {
            //Debug.Log("Can't find GetTutorialAnimImage");
        }

        if (GetTutorialAnimObject.GetComponent<SpriteAnim>() != null)
        {
            GetSpriteAnimScript = GetTutorialAnimObject.GetComponent<SpriteAnim>();
        }
        else
        {
            //Debug.Log("Can't find SpriteAnim");
        }

        if (GetTutorialAnimObject.GetComponent<SpriteAnim>() != null)
        {
            GetSpriteAnimScript = GetTutorialAnimObject.GetComponent<SpriteAnim>();
        }
        else
        {
            //Debug.Log("Can't find SpriteAnimScript");
        }

        if (GameObject.FindWithTag("MyLight") != null)
        {
            GetLightObject = GameObject.FindWithTag("MyLight").GetComponent<Light>();
        }
        else
        {
            //Debug.Log("Can't find MyLight component");
        }

        if (GameObject.FindWithTag("PhotoMssg") != null)
        {
            GetPhotoMssg = GameObject.FindWithTag("PhotoMssg").GetComponent<Text>();
        }
        else
        {
            //Debug.Log("Can't find MyLight component");
        }

    }
}
