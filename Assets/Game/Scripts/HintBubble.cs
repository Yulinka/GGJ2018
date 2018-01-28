using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintBubble : MonoBehaviour {

    private Image hat;
    private Image clothes;
    private Image glasses;
    private Image no;

    private Dictionary<BodyClothesState, Color> clothesColors;

	private void Start ()
    {
        no = transform.Find("No").gameObject.GetComponent<Image>();
        hat = transform.Find("Hat").gameObject.GetComponent<Image>();
        clothes = transform.Find("Clothing").gameObject.GetComponent<Image>();
        glasses = transform.Find("Glasses").gameObject.GetComponent<Image>();

        clothesColors = new Dictionary<BodyClothesState, Color>{
            {BodyClothesState.Red, Color.red},
            {BodyClothesState.Blue, Color.blue}
        };

        GiveClothesHint(BodyClothesState.Red);
	}

    private void resetHint()
    {
        no.enabled = false;
        hat.enabled = false;
        clothes.enabled = false;
        glasses.enabled = false;
    }

    public void GiveClothesHint(BodyClothesState state)
    {
        resetHint();
        clothes.enabled = true;
        clothes.color = clothesColors[state];
        
    }

    public void GiveHatHint(BodyHatState state)
    {
        resetHint();
        hat.enabled = true;

        if (state == BodyHatState.None)
            no.enabled = true;
    }

    public void GiveGlassesHint(BodyGlassesState state)
    {
        resetHint();
        glasses.enabled = true;

        if (state == BodyGlassesState.None)
            no.enabled = true;

    }
}
