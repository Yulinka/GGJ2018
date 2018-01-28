using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyState
{
    CuteLady,
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

public enum BodyHatColorState
{
	Black,
	Red,
	Green,
	Blue,
	Gold,
	Silver,
}

public enum BodyHairState
{
	Blonde,
	Red,
	Black,
	Silver,
	Brown,
}


public enum BodyClothesState
{
    Red,
    Blue,
	Green,
	Black,
	Gold,
	Silver,
}

public class BodyConfig
{
    public BodyState Body;
    public BodySkinState Skin;
    public BodyGlassesState Glasses;
	public BodyHatState Hat;
	public BodyHatColorState HatColor;
	public BodyHairState Hair;
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
//		config.Body = this.selectRandomFromEnum<BodyState>();
		config.Body = BodyState.HotLady;
		config.Skin = this.selectRandomFromEnum<BodySkinState>();
		config.Glasses = this.selectRandomFromEnum<BodyGlassesState>();
		config.Hat = this.selectRandomFromEnum<BodyHatState>();
		config.Hair = this.selectRandomFromEnum<BodyHairState>();
		config.HatColor = this.selectRandomFromEnum<BodyHatColorState>();
		config.Clothes = this.selectRandomFromEnum<BodyClothesState>();

		config.BodySprite = Resources.Load<Sprite>(this.buildBodySpritePath(config));
		config.HairSprite = Resources.Load<Sprite>(this.buildHairSpritePath(config));
		config.ClothesSprite = Resources.Load<Sprite>(this.buildClothesSpritePath(config));
		config.HatSprite = Resources.Load<Sprite>(this.buildHatSpritePath(config));
		config.GlassesSprite = Resources.Load<Sprite>(this.buildGlassesSpritePath(config));

        return config;
    }

    BodyConfig GenerateBody()
    {
        BodyConfig config = GetNextConfig();
        //config.ChooseSprites();
        return config;
        
    }

	private T selectRandomFromEnum<T>()
	{
		System.Array values = System.Enum.GetValues(typeof(T));
		return (T)values.GetValue((int)(UnityEngine.Random.value * (values.Length - 1)));
	}

	private string buildBodySpritePath(BodyConfig config)
	{
		return "Characters/" + this.bodyTypeToPath (config.Body)
			+ "/bases/face1" + this.skinTypeToPath (config.Skin);
	}

	private string buildHairSpritePath(BodyConfig config)
	{
		int hairIndex = (int)(UnityEngine.Random.value * (4 - 1)) + 1;
		return "Characters/" + this.bodyTypeToPath(config.Body)
			+ "/hairs/hair" + hairIndex + this.hairColorTypeToPath(config.Hair);
	}

	private string buildClothesSpritePath(BodyConfig config)
	{
		return "Characters/" + this.bodyTypeToPath(config.Body)
			+ "/" + this.clothesDir(config.Body) + "/" + this.clothesColorToName(config.Clothes);
	}

	private string buildHatSpritePath(BodyConfig config)
	{
		return "Characters/" + this.bodyTypeToPath(config.Body)
				+ "/hats/hat" + this.generateHatIndex(config) + this.hatColorToPath(config.HatColor);
	}

	private string buildGlassesSpritePath(BodyConfig config)
	{
		return "Characters/" + this.bodyTypeToPath(config.Body)
			+ "/glasses/" + this.glassesTypeToName(config.Glasses);
	}

	private int generateHatIndex(BodyConfig config)
	{
		int numHats = 3;
//		if (config.HatColor == BodyHatColorState.Black)
//			numHats++;

		return (int)(UnityEngine.Random.value * (numHats - 1)) + 1;
	}

	private string clothesDir(BodyState state) {
		return "dresses";
	}

	private string bodyTypeToPath(BodyState state)
	{
		switch (state) {
		case BodyState.CuteLady:
			return "cutelady";
		case BodyState.FatLady:
			return "fatlady";
		case BodyState.HotLady:
			return "hotlady";
		case BodyState.FatMale:
			return "fatgent";
		case BodyState.SkinnyMale:
			return "skinnygent";
		case BodyState.HotMale:
			return "hotgent";
		default:
			return "";
		}
	}

	private string skinTypeToPath(BodySkinState state)
	{
		switch (state) {
		case BodySkinState.White:
			return "white";
		case BodySkinState.Cream:
			return "cream";
		case BodySkinState.Brown:
			return "brown";
		case BodySkinState.Dark:
			return "dark";
		default:
			return "";
		}
	}

	private string clothesColorToName(BodyClothesState state)
	{
		switch (state) {
		case BodyClothesState.Blue:
			return "blue";
		case BodyClothesState.Red:
			return "red";
		case BodyClothesState.Black:
			return "black";
		case BodyClothesState.Green:
			return "green";
		case BodyClothesState.Gold:
			return "gold";
		case BodyClothesState.Silver:
			return "silver";
		default:
			return "";
		}
	}

	private string hatColorToPath(BodyHatColorState state)
	{
		switch (state) {
		case BodyHatColorState.Black:
			return "black";
		case BodyHatColorState.Red:
			return "red";
		case BodyHatColorState.Green:
			return "green";
		case BodyHatColorState.Blue:
			return "blue";
		case BodyHatColorState.Gold:
			return "gold";
		case BodyHatColorState.Silver:
			return "silver";
		default:
			//return "";
			return "black";
		}
	}

	private string glassesTypeToName(BodyGlassesState state)
	{
		switch (state) {
		case BodyGlassesState.Glasses:
			return "glasses1";
		default:
			//return "";
			return "glasses1";
		}
	}

	private string hairColorTypeToPath(BodyHairState state)
	{
		switch (state) {
		case BodyHairState.Blonde:
			return "blonde";
		case BodyHairState.Black:
			return "black";
		case BodyHairState.Red:
			return "red";
		case BodyHairState.Silver:
			return "silver";
		case BodyHairState.Brown:
			return "brown";
		default:
			return "";
		}
	}
}
