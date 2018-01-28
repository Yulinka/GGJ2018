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

    private Sprite BodySprite;
    private Sprite GlassesSprite;
    private Sprite HatSprite;
    private Sprite ClothesSprite;
}

public class BodyGenerator : MonoBehaviour
{
    BodyConfig GetNextConfig()
    {
        var config = new BodyConfig();
        config.Body = BodyState.HotLady;
        config.Skin = BodySkinState.Cream;
        config.Glasses = BodyGlassesState.None;
        config.Hat = BodyHatState.Hat;




        config.Body = BodyState
        return null;
    }

    BodyConfig GenerateBody()
    {
        BodyConfig config = GetNextConfig();
        config.ChooseSprites();
        return config;
        
    }
}
