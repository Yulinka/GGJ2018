using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintBubble : MonoBehaviour
{
    private float showTime;
    private Dictionary<BodyClothesState, Color> clothesColors;
    private Image hat;
    private Image clothes;
    private Image glasses;
    private Image no;
	private Image normalBubble;
	private Image shoutBubble;
	private Image eagle;
	private Image dove;
    private Image dots;

	private void Start ()
    {
		normalBubble = transform.Find("Bubble").gameObject.GetComponent<Image>();
		shoutBubble = transform.Find("Shout").gameObject.GetComponent<Image>();
        no = transform.Find("No").gameObject.GetComponent<Image>();
        hat = transform.Find("Hat").gameObject.GetComponent<Image>();
        clothes = transform.Find("Clothing").gameObject.GetComponent<Image>();
        glasses = transform.Find("Glasses").gameObject.GetComponent<Image>();
		eagle = transform.Find("Eagle").gameObject.GetComponent<Image>();
		dove = transform.Find("Dove").gameObject.GetComponent<Image>();
        dots = transform.Find("Dots").gameObject.GetComponent<Image>();

        clothesColors = new Dictionary<BodyClothesState, Color>{
            {BodyClothesState.Red, Color.red},
            {BodyClothesState.Blue, Color.blue},
            {BodyClothesState.Black, Color.black},
            {BodyClothesState.Gold, Color.yellow},
            {BodyClothesState.Green, Color.green},
            {BodyClothesState.Silver, Color.gray},
        };

        resetHint();
	}

    private void Update()
    {
        showTime -= Time.deltaTime;

		if (showTime <= 0 && (normalBubble.enabled || shoutBubble.enabled))
            resetHint();

    }

    private void resetHint()
    {
        normalBubble.color = Color.white;
		normalBubble.enabled = false;
		shoutBubble.color = Color.white;
		shoutBubble.enabled = false;
        no.enabled = false;
        hat.enabled = false;
        clothes.enabled = false;
        glasses.enabled = false;
        dots.enabled = false;
		eagle.enabled = false;
		dove.enabled = false;
    }

    private void startShow()
    {
        normalBubble.enabled = true;
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

    public void ShowFascistConvert()
    {
        resetHint();
        startShow();

		// TODO: Generalize this into startShow()
		normalBubble.enabled = false;
		shoutBubble.enabled = true;
        shoutBubble.color = Color.red;
        eagle.enabled = true;
    }

    public void ShowLose(bool isAgent)
    {
        if (isAgent)
            ShowFascistConvert();
        else
            ShowFascistHint();
        showTime = float.MaxValue;
    }

	public void ShowWin(bool isAgent)
	{
		if (!isAgent)
			ShowDoveHint();
		showTime = float.MaxValue;
	}

    public void ShowFascistHint()
    {
        resetHint();
        startShow();
        eagle.enabled = true;
    }

    public void ShowDotDotDot()
    {
        resetHint();
        startShow();
        dots.enabled = true;
    }

	public void ShowDoveHint()
	{
		resetHint();
		startShow();
		dove.enabled = true;
	}
}
