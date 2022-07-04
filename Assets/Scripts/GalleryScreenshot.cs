using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryScreenshot : MonoBehaviour {

    IEnumerator coroutine;
    public Image backButton;
    public Image photoButton;
    public Image shopButton;
    public Image questionMark;
    public Image greenTick;

    public void TakeShot()
	{
        backButton.enabled = false;
        photoButton.enabled = false;
        shopButton.enabled = false;
        ARCommonCached.GetARTargetScript.enabled = false;
        /*
        if (questionMark != null && questionMark.enabled==true)
            questionMark.enabled = false;
        if (questionMark.enabled == false)
            Debug.Log("I disabled question");

        if (greenTick != null && questionMark.enabled==true)
            greenTick.enabled = false;*/

        ARCommonCached.GetPhotoMssg.color = new Color(ARCommonCached.GetPhotoMssg.color.r, ARCommonCached.GetPhotoMssg.color.g, ARCommonCached.GetPhotoMssg.color.b, 0);

        StartCoroutine(TakeScreenshotAndSave());
        coroutine = FadeOutText(3.0f, ARCommonCached.GetPhotoMssg);
    }
	
	private IEnumerator TakeScreenshotAndSave()
	{
		yield return new WaitForEndOfFrame();

        StopCoroutine(coroutine);

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		// Save the screenshot to Gallery/Photos
		Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "MudMudStudio", "Snapshot_{0}.png"));

        //NativeGallery.GetSavePath("MudMudStudio", "Snapshot_{0}.png");
        
        ARCommonCached.GetPhotoMssg.text = "Photo saved at Images/MudMudStudio Folder.";
        ARCommonCached.GetPhotoMssg.enabled = true;
        ARCommonCached.GetPhotoMssg.color= new Color(ARCommonCached.GetPhotoMssg.color.r, ARCommonCached.GetPhotoMssg.color.g, ARCommonCached.GetPhotoMssg.color.b, 1);
        //Debug.Log("Alpha above " + ARCommonCached.GetPhotoMssg.color.a);

        backButton.enabled = true;
        photoButton.enabled = true;
        shopButton.enabled = true;
        ARCommonCached.GetARTargetScript.enabled = true;
        /*if (questionMark != null && ARCommonCached.GetARTargetScript.TargetState == ARTarget.FocusState.Finding)
            questionMark.enabled = true;
        if (greenTick != null && ARCommonCached.GetARTargetScript.TargetState == ARTarget.FocusState.Found)
            greenTick.enabled = true;*/

        StartCoroutine(coroutine);
		// To avoid memory leaks
		Destroy(ss);
	}

    IEnumerator FadeOutText(float t, Text i)
    {
        yield return new WaitForSeconds(3.0f);
        //fade out
        //i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            //Debug.Log("alpha " + i.color.a);
            yield return null;
        }
    }
}
