using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour
{
    public bool loop;
    public float frameSeconds = 0.25f;
    private Image myImageComponent;
    private Sprite [] sprites;
    private int frame = 0;
    private bool tutorialAnimFinished=true;

    public bool TutorialAnimFinished
    { get
        {
            return tutorialAnimFinished;
        }
    }

    public void OnEnable()
    {
        tutorialAnimFinished = false;
    }
    // Use this for initialization
    void Start()
    {
        myImageComponent = GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>("TutorialSprite");

        IEnumerator coroutine = SpriteAnimationTutorial();
        StartCoroutine(coroutine);
    }

    IEnumerator SpriteAnimationTutorial()
    {
        while(frame<sprites.Length)
        {
            myImageComponent.sprite = sprites[frame];
            frame++;
            yield return new WaitForSeconds(0.4f);
        }
        
        tutorialAnimFinished = true;
    }
}
