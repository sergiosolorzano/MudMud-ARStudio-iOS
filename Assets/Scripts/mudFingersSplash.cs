using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mudFingersSplash : MonoBehaviour
{
    public RectTransform mudTopRect, mudBottomRect, mudRightRect, mudLeftRect;
    private Vector2 mudTopStartRect, mudBottomStartRect, mudRightStartRect, mudLeftStartRect;
    private Vector2 rightTarget, leftTarget;
    private float startTime;
    private float timePassed;
    private float timeToMoveSprites = 3f;//secs it takes

    private bool changeScene=false;


    private void Start()
    {
        //Application.targetFrameRate = 60;
        rightTarget = new Vector2(-245, mudRightRect.anchoredPosition.y);
        leftTarget = new Vector2(244, mudLeftRect.anchoredPosition.y);

        startTime = Time.time;

        mudTopStartRect = mudTopRect.anchoredPosition;
        mudBottomStartRect = mudBottomRect.anchoredPosition;
        mudRightStartRect = mudRightRect.anchoredPosition;
        mudLeftStartRect = mudLeftRect.anchoredPosition;
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SelectionScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void Update()
    {
        //Debug.Log("Time.time " + Time.time + " startTime " + startTime + " time to move sprites " + timeToMoveSprites + " time passed " + timePassed);
        timePassed = (Time.time - startTime) / timeToMoveSprites;

        mudTopRect.offsetMax = Vector2.Lerp(mudTopStartRect, new Vector2(mudTopRect.anchoredPosition.x, 0), timePassed);
        mudTopRect.offsetMin= Vector2.Lerp(mudTopStartRect, new Vector2(mudTopRect.anchoredPosition.x, 0), timePassed);
        mudBottomRect.offsetMax = Vector2.Lerp(mudBottomStartRect, new Vector2(mudBottomRect.anchoredPosition.x, 0), timePassed);
        mudBottomRect.offsetMin = Vector2.Lerp(mudBottomStartRect, new Vector2(mudBottomRect.anchoredPosition.x, 0), timePassed);

        mudRightRect.anchoredPosition = Vector2.Lerp(mudRightStartRect, rightTarget, timePassed);
        mudLeftRect.anchoredPosition = Vector2.Lerp(mudLeftStartRect, leftTarget, timePassed);

        if(timePassed >= 1 && changeScene==false)
        {
            IEnumerator coroutine = LoadYourAsyncScene();
            StartCoroutine(coroutine);
            changeScene = true;
        }
    }
}