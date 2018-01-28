using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGenerator : MonoBehaviour
{
    public AiPerson Template;
    public int Count = 40;

    // Use this for initialization
    void Start()
    {
        GeneratePartygoers(Count);
    }

    public void GeneratePartygoers(int count)
    {
        for (int i = 0; i < count; ++i)
        {
//            var person = Instantiate(Template, transform);
//            person.bodyIndex = Random.Range(0, person.BodySprites.Length);
//            person.clothesIndex = Random.Range(0, person.ClothesSprites.Length);
//            person.glassesIndex = Random.Range(0, person.GlassesSprites.Length);
//            person.hairIndex = Random.Range(0, person.HairSprites.Length);
//            person.hatIndex = Random.Range(0, person.HatSprites.Length);
        }
    }
}
