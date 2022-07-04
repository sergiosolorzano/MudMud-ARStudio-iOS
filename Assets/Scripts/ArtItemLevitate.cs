using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class ArtItemLevitate : MonoBehaviour {

    //demo variables
    public bool demoWasPlayed=true;

    public Button [] selectionButton;
    public Image[] selectionImage;
    public Button [] onGameButton;
    public Image[] onGameImage;

    //tutorial anim variables
    //black canvas for selection and ongame
    public Image curtain;
    //ongame buttons fading
    float buttonFadeStartTime;

    public GameObject[] Art = new GameObject[9];
    private string[] resourcesToLoad = { "Art1", "Art2", "Art3" };
    public GameObject menuText;
    public GameObject scanText;
    public GameObject placeMeButton;
    private Button placeMeButtonButtonComponent;
    public int firstSpinComplete = 0;//At 3 we call delegate myPlaceMeButtonDelegate to switch on the 

    GameObject ArtItemGO;

    int objArrayPosToInstance;//object to instance held in Resources

    Camera cam;

    delegate void ScanningMssgDelegate();
    ScanningMssgDelegate scanMssgFadeInAndOut;

    //roulette
    public GameObject[] ArtRow1 = new GameObject[3];
    public GameObject[] ArtRow2 = new GameObject[3];
    public GameObject[] ArtRow3 = new GameObject[3];

    //PlaceMe Button Delegates
    delegate void PlaceMeButtonDelegate(bool onOrOff);
    PlaceMeButtonDelegate myPlaceMeButtonDelegate;

    //private int rouletteObj;
    private bool moveRow1Now;
    private bool moveRow2Now;
    private bool moveRow3Now;
    private bool firstSpin=true;
    private float firstSpinSpeed = 20f;
    private float standardSpinSpeed = 70f;
    private float step=0;//affected by speed movetowards

    private int jumpPositions;
    private int currentJump1Position;
    private int currentJump2Position;
    private int currentJump3Position;
    private int currentRow=0;
    private int [] selectedArtInEachRow= new int[3] { 0, 0, 0 };

    private Queue<GameObject> Row1Queue = new Queue<GameObject>();
    private Queue<Vector3> originalRow1Queue = new Queue<Vector3>();
    private Queue<GameObject> Row2Queue = new Queue<GameObject>();
    private Queue<Vector3> originalRow2Queue = new Queue<Vector3>();
    private Queue<GameObject> Row3Queue = new Queue<GameObject>();
    private Queue<Vector3> originalRow3Queue = new Queue<Vector3>();
    private Queue<Vector3> CurrentOriginalRow1Queue = new Queue<Vector3>();
    private Queue<GameObject> CurrentRow1Queue = new Queue<GameObject>();
    private Queue<Vector3> CurrentOriginalRow2Queue = new Queue<Vector3>();
    private Queue<GameObject> CurrentRow2Queue = new Queue<GameObject>();
    private Queue<Vector3> CurrentOriginalRow3Queue = new Queue<Vector3>();
    private Queue<GameObject> CurrentRow3Queue = new Queue<GameObject>();

    //swipe variables
    Vector2 startPos = new Vector2(0, 0);
    Vector2 endPos = new Vector2(0, 0);
    float startTime = 0.0f;
    Ray ray;

    public void PrepareOnEnableScripts()
    {
        ARCommonCached.GetARPlaneManagerScript.enabled = false;
        ARCommonCached.GetUIControlsInUseScript.UnRenderPlanes();

        cam = Camera.main;
        ARCommonCached.GetLightObject.intensity = 1.2f;

        jumpPositions = ArtRow1.Length;
        currentJump1Position = currentJump2Position = currentJump3Position = 0;

        //turn on selection buttons/images
        TurnOnSelectionItems();
        //turn off onGame items
        TurnOffOnGameItems();
        //turn no black curtain
        curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 0.84f);

        //Turn Off
        ARCommonCached.GetPlaceItemOnPlaneScript.enabled = false;
        
        ARCommonCached.GetARPointCloudManagerScript.enabled = false;
        ARCommonCached.GetARTargetScript.enabled = false;
        ARCommonCached.GetUIControlsInUseScript.enabled = false;

        //subscribe to placeMeButtonDelegate
        myPlaceMeButtonDelegate += PlaceMeButtonSwitch;
    }

    public void Enable()
    {
        if (ARCommonCached.GetSpriteAnimScript.TutorialAnimFinished)
            enabled = true;
        else
            enabled = false;
    }
    private void OnEnable()
    {
        PrepareOnEnableScripts();
        if (GameObject.FindWithTag("Art") != null)
        {
            ArtItemGO = GameObject.FindWithTag("Art");
            Destroy(ArtItemGO);
        }

        ARCommonCached.GetPlaceItemOnPlaneScript.m_ObjectToPlacePrefab = Resources.Load("TribalArt") as GameObject;

        for (int i = 0; i < Art.Length; i++)
        {
            TraverseOnOffMeshRenderer(Art[i]);
        }

        for (int i = 0; i < Art.Length; i++)
        {
            LockStartPositions(i);
        }
    }

    public void Disable()
    {
        ARCommonCached.GetMyAnalysticsScript.startTimeOnGame = Time.time;
        enabled = false;
    }

    public void OnDisable()
    {
        if (!demoWasPlayed)
            OnDisableDemo();
        else
            OnDisableNonDemo();
    }

    public void TurnOnSelectionItems()
    {
        if(selectionButton.Length>0)
        {
            for (int i = 0; i < selectionButton.Length; i++)
            {
                if(selectionButton[i]!=null)
                    selectionButton[i].enabled = true;
            }
        }

        if(selectionImage.Length>0)
        {
            for (int i = 0; i < selectionImage.Length; i++)
            {
                if(selectionImage[i]!=null)
                    selectionImage[i].enabled = true;
            }
        }
    }

    public void TurnOffSelectionItems()
    {
        if(selectionButton.Length>0)
        {
            for (int i = 0; i < selectionButton.Length; i++)
            {
                if(selectionButton[i]!=null)
                    selectionButton[i].enabled = false;
            }
        }

        if(selectionImage.Length>0)
        {
            for (int i = 0; i < selectionImage.Length; i++)
            {
                if(selectionImage[i]!=null)
                    selectionImage[i].enabled = false;
            }
        }
    }

    public void TurnOnOnGameItems()
    {
        if(onGameButton.Length>0)
        {
            for (int i = 0; i < onGameButton.Length; i++)
            {
                if(onGameButton[i]!=null)
                    onGameButton[i].enabled = true;
            }
        }

        if(onGameImage.Length>0)
        {
            for (int i = 0; i < onGameImage.Length; i++)
            {
                if (onGameImage[i] != null)
                    onGameImage[i].enabled = true;
            }
        }
    }

    public void TurnOffOnGameItems()
    {
        if(onGameButton.Length>0)
        {
            for (int i = 0; i < onGameButton.Length; i++)
                onGameButton[i].enabled = false;
        }

        if(onGameImage.Length>0)
        {
            for (int i = 0; i < onGameImage.Length; i++)
                onGameImage[i].enabled = false;
        }
    }
    public void OnDisableNonDemo()
    {
        //Console.WriteLine("Unsubscribed button delegate");
        myPlaceMeButtonDelegate -= PlaceMeButtonSwitch;

        for (int x = 0; x < Art.Length; x++)
        {
            TraverseOnOffMeshRenderer(Art[x]);
        }

        //ARCommonCached.GetARSessionScript.enabled = true;

        //turn off selection button/images
        TurnOffSelectionItems();

        //turn off curtain
        curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 0.0f);

        ARCommonCached.GetARPlaneManagerScript.enabled = true;
        ARCommonCached.GetUIControlsInUseScript.enabled = true;
        ARCommonCached.GetUIControlsInUseScript.DoRenderPlanes();

        ARCommonCached.GetPlaceItemOnPlaneScript.enabled = true;
        //ARCommonCached.GetARPlaneManagerScript.enabled = true;
        ARCommonCached.GetARPointCloudManagerScript.enabled = true;
        
        ARCommonCached.GetARTargetScript.enabled = true;

        //load sub-stick-objects to be placed
        for (int i = 0; i < resourcesToLoad.Length; i++)
            ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[i] = Resources.Load(resourcesToLoad[i]) as GameObject;

        //turn on OnGame Items
        TurnOnOnGameItems();
        ARCommonCached.GetLightObject.intensity = 1.6f;
    }

    public void OnDisableDemo()
    {
        //Console.WriteLine("Unsubscribed button delegate");
        myPlaceMeButtonDelegate -= PlaceMeButtonSwitch;

        for (int x = 0; x < Art.Length; x++)
        {
            TraverseOnOffMeshRenderer(Art[x]);
        }

        //ARCommonCached.GetARSessionScript.enabled = true;

        if(ARCommonCached.GetARPointCloudManagerScript!=null)
            ARCommonCached.GetARPointCloudManagerScript.enabled = true;

        //load sub-stick-objects to be placed
        for (int i = 0; i < resourcesToLoad.Length; i++)
            ARCommonCached.GetPlaceItemOnPlaneScript.m_SubObjectPrefab[i] = Resources.Load(resourcesToLoad[i]) as GameObject;

        TurnOffSelectionItems();

        if (ARCommonCached.GetTutorialAnimObject != null)
        {
            ARCommonCached.GetTutorialAnimImage.enabled = true;
            ARCommonCached.GetSpriteAnimScript .enabled = true;
        }
        else
        {
            //Debug.Log("Can't find TutorialAnim object");
        }

        IEnumerator coroutine = DemoCountDown();
        StartCoroutine(coroutine);
        ARCommonCached.GetLightObject.intensity = 1.6f;
    }

    IEnumerator DemoCountDown()
    {

        yield return new WaitUntil(() => ARCommonCached.GetSpriteAnimScript.TutorialAnimFinished==true);

        if (ARCommonCached.GetTutorialAnimObject != null)
        {
            ARCommonCached.GetTutorialAnimImage.enabled = false;
            ARCommonCached.GetSpriteAnimScript.enabled = false;
        }
        //else
            //Debug.Log("Can't find TutorialAnim");

        IEnumerator fadeOutCoroutine = FadeOutCurtain(4f, curtain);//fade out curtain , fade in buttons
        StartCoroutine(fadeOutCoroutine);

        ARCommonCached.GetPlaceItemOnPlaneScript.enabled = true;
        ARCommonCached.GetUIControlsInUseScript.enabled = true;
        ARCommonCached.GetARTargetScript.enabled = true;

        //activate image of buttons but not the button itself 
        for (int i = 0; i < onGameImage.Length; i++)
            onGameImage[i].enabled = true;

        demoWasPlayed = true;
    }

    public void FrontArtWhenButtonPressed()
    {
        //load sub-stick-objects to be placed
        for (int i = 0; i < resourcesToLoad.Length; i++)
            ARCommonCached.GetMyAnalysticsScript.ArtPieceStructure[i] = Resources.Load(resourcesToLoad[i]) as GameObject;
    }

    IEnumerator FadeOutCurtain(float tCurtain, Image i)
    {
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - ((Time.deltaTime) / tCurtain));
            yield return null;
        }
        
        IEnumerator fadeInCoroutine = FadeInButtons(onGameImage, 4f);//buttons time add curtain
        StartCoroutine(fadeInCoroutine);
        Debug.Log("Switching on planemanager");
        ARCommonCached.GetARPlaneManagerScript.enabled = true;
        //Debug.Log("fading in buttons");
    }

    IEnumerator FadeInButtons(Image[] buttons, float tButtons)
    {
        for (int j = 0; j < buttons.Length; j++)
        {
            buttons[j].color = new Color(buttons[j].color.r, buttons[j].color.g, buttons[j].color.b, 0);
        }

        while (buttons[buttons.Length - 1].color.a < 1.0f)
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                buttons[j].color = new Color(buttons[j].color.r, buttons[j].color.g, buttons[j].color.b, buttons[j].color.a + ((Time.deltaTime) / tButtons));
            }
            yield return null;
        }

        //once fading is finished, turn on buttons
        //Debug.Log("Turning on Buttons");
        for (int i = 0; i < onGameButton.Length; i++)
            onGameButton[i].enabled = true;
    }

    //applicable to the first time the Art Menu is shown only
    public void Start()
    {
        //initial render slow spin
        firstRouletteSpinA();
        //first Spin start with PlaceMe Button switched off
        placeMeButtonButtonComponent = placeMeButton.GetComponent<Button>();
        placeMeButtonButtonComponent.enabled = false;

        step = firstSpinSpeed * Time.deltaTime;
    }

    public void PlaceMeButtonSwitch(bool onOrOff)
    {
        if(firstSpin)
        {
            if (firstSpinComplete < resourcesToLoad.Count())
                return;
            else
            {
                placeMeButtonButtonComponent.enabled = true;
                return;
            }
        }

        if (onOrOff)
        {
            placeMeButtonButtonComponent.enabled = true;
        }
            
        else
        {
            placeMeButtonButtonComponent.enabled = false;
        }
    }


    private void LockStartPositions(int i)
    {
        if (Array.IndexOf(ArtRow1, Art[i].gameObject)!=-1)
        {
            Row1Queue.Enqueue(ArtRow1[i]);
            originalRow1Queue.Enqueue(ArtRow1[i].transform.localPosition);
            return;
        }

        if(Array.IndexOf(ArtRow2, Art[i].gameObject)!=-1)
        {   
            Row2Queue.Enqueue(ArtRow2[i-3]);   
            originalRow2Queue.Enqueue(ArtRow2[i-3].transform.position);
            return;
        }

        if(Array.IndexOf(ArtRow3, Art[i].gameObject)!=-1)
        {
            Row3Queue.Enqueue(ArtRow3[i-6]);
            originalRow3Queue.Enqueue(ArtRow3[i-6].transform.position);
            return;
        }
    }

    private void Update()
    {
        
        CheckUserClickRoulette();

        if (moveRow1Now)
        {
            CurrentRow1Queue.ElementAt(0).transform.localPosition= Vector3.MoveTowards(CurrentRow1Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(1), step);
            CurrentRow1Queue.ElementAt(1).transform.localPosition= Vector3.MoveTowards(CurrentRow1Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(2), step);
            CurrentRow1Queue.ElementAt(2).transform.localPosition= Vector3.MoveTowards(CurrentRow1Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(0), step);


            if (Vector3.Distance(CurrentRow1Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(1)) < 5f
            && Vector3.Distance(CurrentRow1Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(2)) < 5f
            && Vector3.Distance(CurrentRow1Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(0)) < 5f
            && firstSpin)
            {
                step = Math.Max(step - 0.05f * step, 0.04f);
            }
                
            if (Vector3.Distance(CurrentRow1Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(1)) < 0.1f
            && Vector3.Distance(CurrentRow1Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(2)) < 0.1f
            && Vector3.Distance(CurrentRow1Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow1Queue.ElementAt(0)) < 0.1f)
            {
                if (!firstSpin)
                    currentJump1Position++;

                if (firstSpin)
                    currentRow = 0;

                //control for the PlaceMeNowButton
                if (firstSpin)
                    firstSpinComplete++;

                CurrentRow1Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = false;

                if (--selectedArtInEachRow[currentRow]< 0)
                    selectedArtInEachRow[currentRow] = ArtRow1.Length - 1;

                resourcesToLoad[currentRow] = CurrentRow1Queue.ElementAt(selectedArtInEachRow[currentRow]).name;
                CurrentRow1Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = true;

                //
                if (currentJump1Position == jumpPositions-1 && !firstSpin)
                {
                    moveRow1Now = false;
                }
                if (firstSpin)
                {
                    moveRow1Now = false;
                }

                CurrentOriginalRow1Queue.Clear();
                foreach (var q in CurrentRow1Queue)
                {
                    CurrentOriginalRow1Queue.Enqueue(q.transform.localPosition);
                }
                myPlaceMeButtonDelegate(true);
            }
        }

        if (moveRow2Now)
        {
            CurrentRow2Queue.ElementAt(0).transform.localPosition = Vector3.MoveTowards(CurrentRow2Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(1), step);
            CurrentRow2Queue.ElementAt(1).transform.localPosition = Vector3.MoveTowards(CurrentRow2Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(2), step);
            CurrentRow2Queue.ElementAt(2).transform.localPosition = Vector3.MoveTowards(CurrentRow2Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(0), step);

            if (Vector3.Distance(CurrentRow2Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(1)) < 0.1f
            && Vector3.Distance(CurrentRow2Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(2)) < 0.1f
            && Vector3.Distance(CurrentRow2Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow2Queue.ElementAt(0)) < 0.1f)
            {
                if (!firstSpin)
                    currentJump2Position++;

                if (firstSpin)
                    currentRow = 1;

                //control for the PlaceMeNowButton
                if (firstSpin)
                    firstSpinComplete++;

                CurrentRow2Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = false;

                if (--selectedArtInEachRow[currentRow] < 0)
                    selectedArtInEachRow[currentRow] = ArtRow1.Length - 1;
                resourcesToLoad[currentRow] = CurrentRow2Queue.ElementAt(selectedArtInEachRow[currentRow]).name;

                CurrentRow2Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = true;

                if (currentJump2Position == jumpPositions - 1 && !firstSpin)
                {
                    moveRow2Now = false;
                }
                if (firstSpin)
                    moveRow2Now = false;

                CurrentOriginalRow2Queue.Clear();
                foreach (var q in CurrentRow2Queue)
                {
                    CurrentOriginalRow2Queue.Enqueue(q.transform.localPosition);
                }
                myPlaceMeButtonDelegate(true);
            }
        }

        if (moveRow3Now)
        {
            CurrentRow3Queue.ElementAt(0).transform.localPosition = Vector3.MoveTowards(CurrentRow3Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(1), step);
            CurrentRow3Queue.ElementAt(1).transform.localPosition = Vector3.MoveTowards(CurrentRow3Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(2), step);
            CurrentRow3Queue.ElementAt(2).transform.localPosition = Vector3.MoveTowards(CurrentRow3Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(0), step);

            if (Vector3.Distance(CurrentRow3Queue.ElementAt(0).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(1)) < 0.1f
            && Vector3.Distance(CurrentRow3Queue.ElementAt(1).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(2)) < 0.1f
            && Vector3.Distance(CurrentRow3Queue.ElementAt(2).transform.localPosition, CurrentOriginalRow3Queue.ElementAt(0)) < 0.1f)
            {
                if (!firstSpin)
                    currentJump3Position++;

                if (firstSpin)
                    currentRow = 2;

                //control for the PlaceMeNowButton
                if (firstSpin)
                    firstSpinComplete++;

                CurrentRow3Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = false;

                if (--selectedArtInEachRow[currentRow] < 0)
                    selectedArtInEachRow[currentRow] = ArtRow1.Length - 1;
                resourcesToLoad[currentRow] = CurrentRow3Queue.ElementAt(selectedArtInEachRow[currentRow]).name;

                CurrentRow3Queue.ElementAt(selectedArtInEachRow[currentRow]).GetComponent<BoxCollider>().enabled = true;

                if (currentJump3Position == jumpPositions - 1 && !firstSpin)
                {
                    moveRow3Now = false;
                }
                if (firstSpin)
                    moveRow3Now = false;

                    CurrentOriginalRow3Queue.Clear();
                foreach (var q in CurrentRow3Queue)
                {
                    CurrentOriginalRow3Queue.Enqueue(q.transform.localPosition);
                }
                myPlaceMeButtonDelegate(true);
            }
        }
    }

    private bool CheckSwipeTouch()
    {
        float diffTime = 0.0f;
        float distance=0.0f;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
        {
            startPos = Input.GetTouch(0).position;
            startTime = Time.time;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPos = Input.GetTouch(0).position;
            diffTime = Time.time - startTime;
            startTime = 0;
            distance = Vector2.Distance(startPos, endPos);
            if (distance != 0.0f && diffTime != 0.0f)
                return true;
        }
        return false;
    }

    private void CheckUserClickRoulette()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //disable initial spin mechanics
                if (!moveRow1Now && !moveRow2Now && !moveRow3Now)
                {
                    firstSpin = false;
                    step = standardSpinSpeed * Time.deltaTime;
                }

                if(firstSpin)
                {
                    step = standardSpinSpeed * Time.deltaTime;
                    return;
                }

                //Console.WriteLine("About to switch off delegate");
                myPlaceMeButtonDelegate(false);

                //Debug.Log("I hit collider " + hit.collider.name);

                    if(Array.IndexOf(ArtRow1, hit.collider.transform.gameObject)!=-1 && !moveRow1Now && !moveRow2Now && !moveRow3Now)
                    {
                        CurrentRow1Queue = Row1Queue;
                        currentRow = 0;
                        
                        CurrentOriginalRow1Queue.Clear();

                        for (int i = 0;i< ArtRow1.Length;i++)
                        {
                            CurrentOriginalRow1Queue.Enqueue(ArtRow1[i].transform.localPosition);
                        }
                        currentJump1Position = 0;
                        moveRow1Now = true;

                    return;
                    }

                    if (Array.IndexOf(ArtRow2, hit.collider.transform.gameObject) != -1 && !moveRow2Now && !moveRow1Now && !moveRow3Now)
                    {
                        CurrentRow2Queue = Row2Queue;
                        currentRow = 1;
                        
                        CurrentOriginalRow2Queue.Clear();

                        for (int i = 0; i < ArtRow2.Length; i++)
                        {
                            CurrentOriginalRow2Queue.Enqueue(ArtRow2[i].transform.localPosition);
                        }
                        currentJump2Position = 0;
                        moveRow2Now = true;

                    return;
                    }

                    if (Array.IndexOf(ArtRow3, hit.collider.transform.gameObject) != -1 && !moveRow3Now && !moveRow1Now && !moveRow2Now)
                    {
                        CurrentRow3Queue = Row3Queue;
                        currentRow = 2;
                        
                        CurrentOriginalRow3Queue.Clear();

                        for (int i = 0; i < ArtRow3.Length; i++)
                        {
                            CurrentOriginalRow3Queue.Enqueue(ArtRow3[i].transform.localPosition);
                        }
                        currentJump3Position = 0;
                        moveRow3Now = true;
                    return;
                    }
            }
        }
    }

    private void firstRouletteSpinA()
    {
        //first row
        myPlaceMeButtonDelegate(false);

        CurrentRow1Queue = Row1Queue;
        currentRow = 0;

        CurrentOriginalRow1Queue.Clear();

        for (int i = 0; i < ArtRow1.Length; i++)
        {
            CurrentOriginalRow1Queue.Enqueue(ArtRow1[i].transform.localPosition);
        }
        currentJump1Position = 0;
        moveRow1Now = true;


        StartCoroutine(firstRouletteSpinB());

        StartCoroutine(firstRouletteSpinC());
    }

    private IEnumerator firstRouletteSpinB()
    {
        float currCountdownValue = 1.0f;
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(0.25f);
            currCountdownValue--;
        }
        //second row
        CurrentRow2Queue = Row2Queue;
        currentRow = 1;

        CurrentOriginalRow2Queue.Clear();

        for (int i = 0; i < ArtRow2.Length; i++)
        {
            CurrentOriginalRow2Queue.Enqueue(ArtRow2[i].transform.localPosition);
        }
        currentJump2Position = 0;
        moveRow2Now = true;
    }

    private IEnumerator firstRouletteSpinC()
    {
        float currCountdownValue = 1.0f;
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(0.5f);
            currCountdownValue--;
        }
        //third row
        CurrentRow3Queue = Row3Queue;
        currentRow = 2;

        CurrentOriginalRow3Queue.Clear();

        for (int i = 0; i < ArtRow3.Length; i++)
        {
            CurrentOriginalRow3Queue.Enqueue(ArtRow3[i].transform.localPosition);
        }
        currentJump3Position = 0;
        moveRow3Now = true;
    }

    IEnumerator BringArtFront(GameObject[] Art)
    {
        Vector3 scaleIncrease = new Vector3(0.5f, 0.5f, 0.5f);

        if (Art[0] != null)
        {
            while (Art[0] != null && Art[0].transform.parent.localScale.x < 1.2)
            {
                for (int i = 0; i < Art.Length; i++)
                {
                    if (Art[i] != null)
                    {
                        Art[i].transform.parent.localScale += scaleIncrease * Time.deltaTime;
                        yield return null;
                    }
                }
            }
        }
    }

    private void ImmediateBringArtFront(GameObject[] Art)
    {
        int i = 0;
        if (Art[i] != null)
        {
            for (i = 0; i < Art.Length; i++)
            {
                Art[i].transform.parent.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
    }

    void TraverseOnOffMeshRenderer(GameObject obj)
    {
        if(obj!=null)
        {
            if (obj.GetComponent<MeshRenderer>() != null)
            {
                obj.GetComponent<MeshRenderer>().enabled = !obj.GetComponent<MeshRenderer>().enabled;

                foreach (Transform child in obj.transform)
                {
                    TraverseOnOffMeshRenderer(child.gameObject);
                }
            }
        }
    }

    void TraverseOnOffCollider(GameObject obj)
    {
        if(obj!=null)
        {
            if (obj.GetComponent<BoxCollider>() != null)
            {
                obj.GetComponent<BoxCollider>().enabled = !obj.GetComponent<BoxCollider>().enabled;
                foreach (Transform child in obj.transform)
                {
                    TraverseOnOffCollider(child.gameObject);
                }
            }
        }
    }

    public void ScanningMssg()
    {
        //alpha out Pick your Art Work mssg
        if (scanMssgFadeInAndOut != null)
        {
            if (scanMssgFadeInAndOut != null)
            {
                scanMssgFadeInAndOut();
                //now unsubscribe so it doesn't happen more than once in update()
                scanMssgFadeInAndOut -= FadeInOutScanningMssg;
            }
        }
    }

    private void FadeInOutScanningMssg()
    {
        StartCoroutine(FadeInOutText(3f, scanText.GetComponent<Text>()));
    }

    IEnumerator FadeOutText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeInOutText(float t, Text i)
    {
        //fade out
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }

        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - ((Time.deltaTime) / t));
            yield return null;
        }
    }
    
    

    float MCos(float value)
    {
        return Mathf.Cos(Mathf.Deg2Rad * value);
    }

    float MSin(float value)
    {
        return Mathf.Sin(Mathf.Deg2Rad * value);
    }
}
