using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 Direction;
    public float Degrees;

    SpriteRenderer sprite;

    public float FadeTime = 1;
    public bool FadingOut = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if (FadingOut)
            sprite.color = new Color(1, 1, 1, Mathf.Max(0, sprite.color.a - (Time.smoothDeltaTime / FadeTime)));
        else
            sprite.color = new Color(1, 1, 1, Mathf.Min(1, sprite.color.a + (Time.smoothDeltaTime / FadeTime)));
        transform.rotation *= Quaternion.AngleAxis(Degrees * Mathf.Deg2Rad * Time.smoothDeltaTime, Direction);
    }
}
