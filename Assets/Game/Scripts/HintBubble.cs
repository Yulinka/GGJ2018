using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintBubble : MonoBehaviour {

    private Image hat;
    private Image clothes;
    private Image glasses;
    private Image no;
    private Image bubble;
    private float showTime;

    private Dictionary<BodyClothesState, Color> clothesColors;

	private void Start ()
    {
        bubble = transform.Find("Bubble").gameObject.GetComponent<Image>();
        no = transform.Find("No").gameObject.GetComponent<Image>();
        hat = transform.Find("Hat").gameObject.GetComponent<Image>();
        clothes = transform.Find("Clothing").gameObject.GetComponent<Image>();
        glasses = transform.Find("Glasses").gameObject.GetComponent<Image>();

        clothesColors = new Dictionary<BodyClothesState, Color>{
            {BodyClothesState.Red, Color.red},
            {BodyClothesState.Blue, Color.blue}
        };

        resetHint();
	}

    private void Update()
    {
        showTime -= Time.deltaTime;

        if (showTime <= 0 && bubble.enabled)
            resetHint();

    }

    private void resetHint()
    {
        bubble.enabled = false;
        no.enabled = false;
        hat.enabled = false;
        clothes.enabled = false;
        glasses.enabled = false;
    }

    private void startShow()
    {
        bubble.enabled = true;
        showTime = 5f;
    }

    public void ShowNoHint()
    {
        resetHint();
        startShow();
        no.enabled = true;
    }

    public void ShowClothesHint(BodyClothesState state)
    {
        resetHint();
        startShow();
        clothes.enabled = true;
        clothes.color = clothesColors[state];
    }

    public void ShowHatHint(BodyHatState state)
    {
        resetHint();
        startShow();
        hat.enabled = true;

        if (state == BodyHatState.None)
            no.enabled = true;
    }

    public void ShowGlassesHint(BodyGlassesState state)
    {
        resetHint();
        startShow();
        glasses.enabled = true;

        if (state == BodyGlassesState.None)
            no.enabled = true;
    }
}
