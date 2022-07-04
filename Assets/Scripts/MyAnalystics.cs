using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class MyAnalystics : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] ArtPieceStructure = new GameObject[3];
    private Dictionary<string, string> equateArtName = new Dictionary<string, string>();
    //private Dictionary<string, int> equateArtNameToInt = new Dictionary<string, int>();
    private float startAfterSlpashTime;
    public float startTimeOnGame = 0;//Set by ArtItemLevitate when enter OnGame pressed

    void Start()
    {
        startAfterSlpashTime = Time.time;

        Application.quitting += Quit;

        equateArtName.Add("Art1", "Trunk");
        equateArtName.Add("Art2", "Trunk");
        equateArtName.Add("Art3", "Trunk");
        equateArtName.Add("Art7", "Barrel");
        equateArtName.Add("Art5", "Barrel");
        equateArtName.Add("Art6", "Barrel");
        equateArtName.Add("Art4", "Hips");
        equateArtName.Add("Art8", "Hips");
        equateArtName.Add("Art9", "Hips");

        /*equateArtNameToInt.Add("Art1", 1);
        equateArtNameToInt.Add("Art2", 1);
        equateArtNameToInt.Add("Art3", 1);
        equateArtNameToInt.Add("Art7", 2);
        equateArtNameToInt.Add("Art5", 2);
        equateArtNameToInt.Add("Art6", 2);
        equateArtNameToInt.Add("Art4", 3);
        equateArtNameToInt.Add("Art8", 3);
        equateArtNameToInt.Add("Art9", 3);*/
    }

    public void ShopAtSelectPressed()
    {
        //Debug.Log("ShopAtSelect was pressed");

        //get art objects currently at the front in selection screen
        ARCommonCached.GetArtItemLevitateScript.FrontArtWhenButtonPressed();
        equateArtName.TryGetValue(ArtPieceStructure[0].name, out string TopArtPiece);
        equateArtName.TryGetValue(ArtPieceStructure[1].name, out string MiddleArtPiece);
        equateArtName.TryGetValue(ArtPieceStructure[2].name, out string BottomArtPiece);

        Analytics.CustomEvent("ShopAtSelectPressed", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "TimeOnGame", Math.Round((Time.time-startTimeOnGame)/60,1)},
            { "ShopAtSelectClickedCount", 1},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

        /*//
        equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

        Analytics.CustomEvent("ShopAtSelectPressedTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

        Analytics.CustomEvent("ShopAtSelectPressedMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

        Analytics.CustomEvent("ShopAtSelectPressedBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
    }

    public void ShopOnGamePressed()
    {
        //Debug.Log("ShopOnGame was pressed");

        //get art objects currently at the front in selection screen
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[0].name, out string TopArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[1].name, out string MiddleArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[2].name, out string BottomArtPiece);

        Analytics.CustomEvent("ShopOnGamePressed", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "TimeOnGame", Math.Round((Time.time-startTimeOnGame)/60,1)},
            { "ShopOnGameClickedCount", 1},
            { "WasArtInstantiated", ARCommonCached.GetPlaceItemOnPlaneScript.ObjectInstanced},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

        /*//
        equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

        Analytics.CustomEvent("ShopOnGamePressedTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

        Analytics.CustomEvent("ShopOnGamePressedMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

        Analytics.CustomEvent("ShopOnGamePressedBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/

        startTimeOnGame = 0;
    }

    public void ClientLogoBannerPressed()
    {
        //Debug.Log("ClientLogoBannerPressed was pressed");

        if (ARCommonCached.GetUIControlsInUseScript.enabled == true)
        {
            equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[0].name, out string TopArtPiece);
            equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[1].name, out string MiddleArtPiece);
            equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[2].name, out string BottomArtPiece);

            Analytics.CustomEvent("ClientLogoBannerPressedAtOnGame", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "TimeOnGame", Math.Round((Time.time-startTimeOnGame)/60,1)},
            { "ClientBannerClickedCount", 1},
            { "WasArtInstantiated", ARCommonCached.GetPlaceItemOnPlaneScript.ObjectInstanced},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

            /*//
            equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
            equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
            equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

            Analytics.CustomEvent("ClientLogoBannerPressedTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

            Analytics.CustomEvent("ClientLogoBannerPressedMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

            Analytics.CustomEvent("ClientLogoBannerPressedBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
        }
        else
        {
            //get art objects currently at the front in selection screen
            ARCommonCached.GetArtItemLevitateScript.FrontArtWhenButtonPressed();
            equateArtName.TryGetValue(ArtPieceStructure[0].name, out string TopArtPiece);
            equateArtName.TryGetValue(ArtPieceStructure[1].name, out string MiddleArtPiece);
            equateArtName.TryGetValue(ArtPieceStructure[2].name, out string BottomArtPiece);

            Analytics.CustomEvent("ClientLogoBannerPressedAtSelect", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "ClientBannerClickedCount", 1},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

            /*//
            equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
            equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
            equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

            Analytics.CustomEvent("ClientLogoBannerPressedAtSelectTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

            Analytics.CustomEvent("ClientLogoBannerPressedAtSelectMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

            Analytics.CustomEvent("ClientLogoBannerPressedAtSelectBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
        }
        startTimeOnGame = 0;
    }

    public void VisitingRendergon()
    {
        //Debug.Log("VisitRendergon was pressed");
        bool onGameScreen = false;
        if (ARCommonCached.GetUIControlsInUseScript.enabled == true)
            onGameScreen = true;

        Analytics.CustomEvent("VisitRendergonPressed", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "WasArtInstantiated", ARCommonCached.GetPlaceItemOnPlaneScript.ObjectInstanced},
            { "VisitRendergonClickedCount", 1},
            { "PressedOnGameScreen", onGameScreen},
        });
        startTimeOnGame = 0;
    }

    public void OnGameTime()
    {
        //Debug.Log("TimeOnGame recorded");

        //get art objects currently at the front in selection screen
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[0].name, out string TopArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[1].name, out string MiddleArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[2].name, out string BottomArtPiece);

        Analytics.CustomEvent("TimeOnGamePressedBack", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "TimeOnGame", Math.Round((Time.time-startTimeOnGame)/60,1)},
            { "ClickedBack",  1},
            { "WasArtInstantiated", ARCommonCached.GetPlaceItemOnPlaneScript.ObjectInstanced},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

        /*//
        equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

        Analytics.CustomEvent("TimeOnGamePressedBackTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

        Analytics.CustomEvent("TimeOnGamePressedBackMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

        Analytics.CustomEvent("TimeOnGamePressedBackBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
    }

    public void OnGamePhoto()
    {
        //Debug.Log("Photo recorded");

        //get art objects currently at the front in selection screen
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[0].name, out string TopArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[1].name, out string MiddleArtPiece);
        equateArtName.TryGetValue(ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[2].name, out string BottomArtPiece);

        Analytics.CustomEvent("Photo", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "TimeOnGame", Math.Round((Time.time-startTimeOnGame)/60,1)},
            { "OnGamePhotoClickedCount", 1},
            { "WasArtInstantiated", ARCommonCached.GetPlaceItemOnPlaneScript.ObjectInstanced},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

        /*//
        equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

        Analytics.CustomEvent("PhotoTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

        Analytics.CustomEvent("PhotoMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

        Analytics.CustomEvent("PhotoBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
    }

    public void PlaceItExpanded()
    {
        //get art objects currently at the front in selection screen
        ARCommonCached.GetArtItemLevitateScript.FrontArtWhenButtonPressed();
        equateArtName.TryGetValue(ArtPieceStructure[0].name, out string TopArtPiece);
        equateArtName.TryGetValue(ArtPieceStructure[1].name, out string MiddleArtPiece);
        equateArtName.TryGetValue(ArtPieceStructure[2].name, out string BottomArtPiece);

        Analytics.CustomEvent("PlaceItExpanded", new Dictionary<string, object>
        {
            { "TimeOnAppAfterSplash", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
            { "PlaceItClickedCount", 1},
            { "TopPiece",  TopArtPiece},
            { "MiddlePiece", MiddleArtPiece},
            { "BottomPiece", BottomArtPiece},
        });

        /*//
        equateArtNameToInt.TryGetValue(ArtPieceStructure[0].name, out int TopArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[1].name, out int MiddleArtPieceInt);
        equateArtNameToInt.TryGetValue(ArtPieceStructure[2].name, out int BottomArtPieceInt);

        Analytics.CustomEvent("PlaceItTopPiece", new Dictionary<string, object>
        {
            { "TopPiece",  TopArtPieceInt},
        });

        Analytics.CustomEvent("PlaceItMiddlePiece", new Dictionary<string, object>
        {
            { "MiddlePiece",  MiddleArtPieceInt},
        });

        Analytics.CustomEvent("PlaceItBottomPiece", new Dictionary<string, object>
        {
            { "BottomPiece",  BottomArtPieceInt},
        });*/
    }

    public void InstancedArtCount()//Event called at PlaceItemOnPlane
    {
        Analytics.CustomEvent("InstancedArtCount", new Dictionary<string, object>
        {
            { "InstancedArtCount", 1},
        });
    }

    void Quit()
    {
        //Debug.Log("Quitting the Player");
        Analytics.CustomEvent("TimeFromSplashToEnd", new Dictionary<string, object>
        {
            { "TimeFromSplashToEnd", Math.Round((Time.time-startAfterSlpashTime)/60,1)},
        });
    }
}