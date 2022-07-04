using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIControlsInUse : MonoBehaviour
{
    GraphicRaycaster m_RaycasterUI;
    PointerEventData m_PointerEventDataUI;
    EventSystem m_EventSystemUI;

    //private bool sliderIsInUse;
    private bool uIIsInUse;

    public Text touchPlaneMssg;

    public Sprite tribalSprite;
    public Sprite dismemberedSprite;
    public Button artTypeButton;

    public bool UIIsInUse
    {
        get
        {
            return uIIsInUse;
        }
        set
        {
            uIIsInUse= value;
        }
    }

    private void OnEnable()
    {
        //Any UI touch listener
        //Fetch the Raycaster from the GameObject (the Canvas)
        if (ARCommonCached.SelectionCanvas != null && ARCommonCached.SelectionCanvas.GetComponent<GraphicRaycaster>() != null)
            m_RaycasterUI = ARCommonCached.SelectionCanvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        if (ARCommonCached.SelectionCanvas && ARCommonCached.SelectionCanvas.GetComponent<EventSystem>() != null)
            m_EventSystemUI = ARCommonCached.SelectionCanvas.GetComponent<EventSystem>();

        //Register for new planes
        ARCommonCached.GetARPlaneManagerScript.planesChanged += this.RenderPlanes;
    }
    void Start()
    {
        OnEnable();
    }

    public void SwitchArtTypeButton()
    {
        //Debug.Log("At switch Art Type bttn");
        if(ARCommonCached.GetPlaceItemOnPlaneScript.currentArtState==PlaceItemOnPlane.ArtState.TribalArt)
            artTypeButton.image.sprite = tribalSprite;
        else
            artTypeButton.image.sprite = dismemberedSprite;
    }

    public void OnDisable()
    {

        ARCommonCached.GetARPlaneManagerScript.planesChanged -= this.RenderPlanes;
    }

    IEnumerator FirstTapOnPlaneMssg()
    {
        touchPlaneMssg.enabled = true;
        yield return new WaitForSeconds(3f);
        touchPlaneMssg.enabled = false;
    }

    void Traverse(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            Traverse(child.gameObject);
        }
    }

    public void RenderPlanes(ARPlanesChangedEventArgs obj)
    {
        foreach (var plane in obj.added)
        {
            //Debug.Log(" At RenderPlanes and found a plane " + plane.name + " trackableID:" + plane.trackableId);
            plane.gameObject.SetActive(true);
        }
    }

    public void DoRenderPlanes()
    {
        foreach (var plane in ARCommonCached.GetARPlaneManagerScript.trackables)
        {
            //Debug.Log(" At DoRender and found a plane " + plane.name + " trackableID:" + plane.trackableId);
            plane.gameObject.SetActive(true);
        }
    }

    public void UnRenderPlanes()
    {
        foreach (var plane in ARCommonCached.GetARPlaneManagerScript.trackables)
        {
            //Debug.Log(" At UnRender and found a plane " + plane.name + " trackableID:" + plane.trackableId);
            plane.gameObject.SetActive(false);
        }
    }

    public bool checkUIUse()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetMouseButton(0) || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);

                //Set up the new Pointer Event
                m_PointerEventDataUI = new PointerEventData(m_EventSystemUI);
                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventDataUI.position = touch.position;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();
                //Raycast using the Graphics Raycaster and mouse click position
                m_RaycasterUI.Raycast(m_PointerEventDataUI, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                /*foreach (RaycastResult result in results)
                {
                    //if (result.gameObject.name.Equals("ScaleBackground") || result.gameObject.name.Equals("ScaleFill") || result.gameObject.name.Equals("ScaleHandle"))
                    //SliderIsInUse = true;
                }*/
                if (results.Count > 0)
                    UIIsInUse = true;

                return UIIsInUse;
            }
            return false;
        }
        else
        {
            UIIsInUse = false;
            return UIIsInUse;
        }
    }

    public void ChangeArtItem()
    {
        //accelerate switch off menu canvas if change item button pressed
        ARCommonCached.GetPlaceItemOnPlaneScript.enabled = false;

        ARCommonCached.GetArtItemLevitateScript.enabled = true;
    }
}