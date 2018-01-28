using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyState
{
    CuteLady,
    CuteMale,
    FatLady,
    FatMale,
    HotLady,
    HotMale,
    SkinnyMale
}

public enum BodySkinState
{
    White,
    Brown,
    Cream,
    Dark,
}

public enum BodyGlassesState
{
    None,
    Glasses,
}

public enum BodyHatState
{
    None,
    Hat
}


public enum BodyClothesState
{
    Red,
    Blue
}

public class BodyConfig
{
    public BodyState Body;
    public BodySkinState Skin;
    public BodyGlassesState Glasses;
    public BodyHatState Hat;
    public BodyClothesState Clothes;

    public Sprite BodySprite;
    public Sprite HairSprite;
    public Sprite GlassesSprite;
    public Sprite HatSprite;
    public Sprite ClothesSprite;
}

public class BodyGenerator : MonoBehaviour
{
    public BodyConfig GetNextConfig()
    {
        var config = new BodyConfig();
        config.Body = BodyState.HotLady;
        config.Skin = BodySkinState.Cream;
        config.Glasses = BodyGlassesState.None;
        config.Hat = BodyHatState.Hat;

        config.BodySprite = Resources.Load<Sprite>("Characters/hotlady/body/cream");
        config.HairSprite = Resources.Load<Sprite>("Characters/hotlady/hairs/hair5");
        config.ClothesSprite = Resources.Load<Sprite>("Characters/hotlady/clothes/red");
        config.HatSprite = Resources.Load<Sprite>("Characters/hotlady/hats/hat1");
        config.GlassesSprite = Resources.Load<Sprite>("Characters/hotlady/glasses/glasses1");

        return config;
    }

    BodyConfig GenerateBody()
    {
        BodyConfig config = GetNextConfig();
        //config.ChooseSprites();
        return config;
        
    }
}
