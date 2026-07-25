using System;
using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UIUtil
{
	public static Color _UIWhiteColor = new Color(78f / 85f, 14f / 15f, 78f / 85f, 0.85f);

	public static Color _UIGreenColor = new Color(0.03137255f, 69f / 85f, 0.03137255f, 0.85f);

	public static Color _UIBlueColor = new Color(0f, 58f / 85f, 1f, 0.85f);

	public static Color _UIPurpleColor = new Color(0.6901961f, 0f, 43f / 51f, 0.85f);

	public static Color _UIRedColor = new Color(1f, 0f, 0f, 0.85f);

	public static Color _UIYellowColor = new Color(1f, 84f / 85f, 0f, 0.85f);

	public static Color _UIGrayColor = new Color(0.6431373f, 0.6431373f, 0.6431373f, 0.85f);

	public static Color _UIBASEMAPGrayColor = new Color(0.101960786f, 0.101960786f, 0.101960786f, 0.8509804f);

	public static Color _UIBASEMAPYELLOWColor = new Color(0.7058824f, 31f / 85f, 0f, 0.24313726f);

	public static bool bStageMapChangedUI = true;

	public static bool bOpenClikShow = false;

	public static bool bOpenClikShow_full = false;

	public static Color GetColorByRankType(Defined.RANK_TYPE rankType)
	{
		Color white = Color.white;
		switch (rankType)
		{
		case Defined.RANK_TYPE.WHITE:
			return _UIWhiteColor;
		case Defined.RANK_TYPE.GREEN:
			return _UIGreenColor;
		case Defined.RANK_TYPE.BLUE:
			return _UIBlueColor;
		case Defined.RANK_TYPE.PURPLE:
			return _UIPurpleColor;
		default:
			return _UIWhiteColor;
		}
	}

	public static string GetColorSpriteNameByRankType(Defined.RANK_TYPE rankType)
	{
		string empty = string.Empty;
		switch (rankType)
		{
		case Defined.RANK_TYPE.WHITE:
			return string.Empty;
		case Defined.RANK_TYPE.GREEN:
			return "SD_di_lv";
		case Defined.RANK_TYPE.BLUE:
			return "SD_di_lan";
		case Defined.RANK_TYPE.PURPLE:
			return "SD_di_zi";
		default:
			return string.Empty;
		}
	}

	public static string GetBoxColorSpriteNameByRankType(Defined.RANK_TYPE rankType)
	{
		string empty = string.Empty;
		switch (rankType)
		{
		case Defined.RANK_TYPE.WHITE:
			return "SD_kuang_bai";
		case Defined.RANK_TYPE.GREEN:
			return "SD_kuang_lv";
		case Defined.RANK_TYPE.BLUE:
			return "SD_kuang_lan";
		case Defined.RANK_TYPE.PURPLE:
			return "SD_kuang_zi";
		default:
			return "SD_kuang_bai";
		}
	}

	public static void GetBackgroundSpriteNameByRankType(Defined.RANK_TYPE rt, ref string bk, ref string bkClick)
	{
		switch (rt)
		{
		case Defined.RANK_TYPE.WHITE:
			bk = "daojukuang";
			bkClick = "daojukuang_dianji";
			break;
		case Defined.RANK_TYPE.GREEN:
			bk = "daojukuang_lv";
			bkClick = "daojukuang_lv_dianji";
			break;
		case Defined.RANK_TYPE.BLUE:
			bk = "daojukuang_lan";
			bkClick = "daojukuang_lan_dianji";
			break;
		case Defined.RANK_TYPE.PURPLE:
			bk = "daojukuang_zi";
			bkClick = "daojukuang_zi_dianji";
			break;
		default:
			bk = "daojukuang";
			bkClick = "daojukuang_dianji";
			break;
		}
	}

	public static string GetBackgroundSpriteNameByRankType(Defined.RANK_TYPE rt)
	{
		string empty = string.Empty;
		switch (rt)
		{
		case Defined.RANK_TYPE.WHITE:
			return "daojukuang";
		case Defined.RANK_TYPE.GREEN:
			return "daojukuang_lv";
		case Defined.RANK_TYPE.BLUE:
			return "daojukuang_lan";
		case Defined.RANK_TYPE.PURPLE:
			return "daojukuang_zi";
		default:
			return "daojukuang";
		}
	}

	public static string GetCombinationString(Color color, string str)
	{
		string empty = string.Empty;
		string text = Convert.ToString((int)(color.r * 255f), 16);
		string text2 = Convert.ToString((int)(color.g * 255f), 16);
		string text3 = Convert.ToString((int)(color.b * 255f), 16);
		if (text.Length < 2)
		{
			text = "0" + text;
		}
		if (text2.Length < 2)
		{
			text2 = "0" + text2;
		}
		if (text3.Length < 2)
		{
			text3 = "0" + text3;
		}
		return "[" + text + text2 + text3 + "]" + str + "[-]";
	}

	public static string GetCombinationString(Defined.RANK_TYPE rankType, string str)
	{
		return GetCombinationString(GetColorByRankType(rankType), str);
	}

	public static string GetCurrencyIconSpriteNameByCurrencyType(Defined.COST_TYPE type)
	{
		string empty = string.Empty;
		switch (type)
		{
		case Defined.COST_TYPE.Crystal:
			return "icon_crystal";
		case Defined.COST_TYPE.Money:
			return "icon_coin";
		case Defined.COST_TYPE.Element:
			return "nengliang";
		case Defined.COST_TYPE.Honor:
			return "icon_honor";
		case Defined.COST_TYPE.Exp:
			return "icon_exp02";
		default:
			return string.Empty;
		}
	}

	public static string GetCurrencyNameByCurrencyType(Defined.COST_TYPE type)
	{
		switch (type)
		{
		case Defined.COST_TYPE.Crystal:
			return "icon_crystal";
		case Defined.COST_TYPE.Money:
			return "icon_coin";
		case Defined.COST_TYPE.Honor:
			return "icon_honor";
		case Defined.COST_TYPE.Exp:
			return "icon_exp";
		case Defined.COST_TYPE.Element:
			return "Elementium";
		default:
			return string.Empty;
		}
	}

	public static string GetQualityNameByRankType(Defined.RANK_TYPE rankType)
	{
		string empty = string.Empty;
		switch (rankType)
		{
		case Defined.RANK_TYPE.WHITE:
			return "Common";
		case Defined.RANK_TYPE.GREEN:
			return "Uncommon";
		case Defined.RANK_TYPE.BLUE:
			return "Rare";
		case Defined.RANK_TYPE.PURPLE:
			return "Epic";
		default:
			return "Common";
		}
	}

	public static Material GetEquipTextureMaterial(string _str)
	{
		Material material = null;
		return Resources.Load("EquipMaterial/" + _str) as Material;
	}

	public static void ShowStars(Transform trans, int showCount)
	{
		for (int i = 0; i < trans.childCount; i++)
		{
			UISprite component = trans.GetChild(i).GetComponent<UISprite>();
			if (i < showCount)
			{
				component.enabled = true;
			}
			else
			{
				component.enabled = false;
			}
		}
	}

	public static string GetHeroDetailInfo(Defined.RANK_TYPE heroRankType, int ratingScore, int level, int ownExp, int totalExp, int health, float damage, float defense, float dReduction, float hit, float critRate, string _captainSkill, string _teamSkill)
	{
		string empty = string.Empty;
		return GetCombinationString(heroRankType, GetQualityNameByRankType(heroRankType)) + "\n\n\n\n\n\n\n\n\n\n\n\n" + GetCombinationString(_UIYellowColor, "RATING " + ratingScore) + "\nLEVEL " + GetCombinationString(_UIGreenColor, level + string.Empty) + "\nEXP: " + GetCombinationString(_UIGreenColor, ownExp + string.Empty) + "/" + GetCombinationString(_UIGreenColor, totalExp + string.Empty) + "\nHEALTH: " + GetCombinationString(_UIGreenColor, health + string.Empty) + "\nDAMAGE: " + GetCombinationString(_UIGreenColor, damage + string.Empty) + "\nDEFENSE: " + GetCombinationString(_UIGreenColor, defense + string.Empty) + "\nD.REDUCTOPN: " + GetCombinationString(_UIGreenColor, dReduction + "%") + "\nHIT: " + GetCombinationString(_UIGreenColor, hit + "%") + "\nCRIT RATE: " + GetCombinationString(_UIGreenColor, critRate + "%") + "\n\n" + GetCombinationString(_UIYellowColor, "CAPTAIN SKILLS") + "\n\n" + _captainSkill + "\n\n" + GetCombinationString(_UIYellowColor, "TEAM SKILLS") + "\n\n" + _teamSkill;
	}

	public static string GetEquipmentDetailInfo(Defined.RANK_TYPE rankType, int level, int health, int defense, float perHealth, float perHit, float perSpeed, float perDamage, float perFrequency, int range, float perDamageAbs, float perStab, float perCrit, float perDodge)
	{
		string empty = string.Empty;
		empty = GetCombinationString(rankType, GetQualityNameByRankType(rankType)) + "\n\n\n\n\n\n\n\n\n\n\n\nLEVEL " + GetCombinationString(_UIGreenColor, level + string.Empty) + "\n";
		if (health != 0)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, health + string.Empty) + " Health\n";
		}
		if (defense != 0)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, defense + string.Empty) + " Defense\n";
		}
		if (perHealth != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perHealth * 100f + "%") + " HP\n";
		}
		if (perHit != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perHit + "%") + " Hit\n";
		}
		if (perSpeed != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perSpeed * 100f + "%") + " Speed\n";
		}
		if (perDamage != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perDamage * 100f + "%") + " Damage\n";
		}
		if (perFrequency != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perFrequency * 100f + "%") + " Frequency\n";
		}
		if (range != 0)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, range + string.Empty) + " Range\n";
		}
		if (perDamageAbs != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perDamageAbs + "%") + " Damage Absorption\n";
		}
		if (perStab != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perStab + "%") + " Stab\n";
		}
		if (perCrit != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perCrit * 100f + "%") + " Crit\n";
		}
		if (perDodge != 0f)
		{
			empty = empty + "+ " + GetCombinationString(_UIGreenColor, perDodge + "%") + " Dodge\n";
		}
		empty += "\n";
		empty = empty + GetCombinationString(_UIWhiteColor, "Endure:Immune to all harmful effects\nfor a period of time.") + "\n\n\n";
		empty = empty + GetCombinationString(_UIYellowColor, "Suits Bonuses") + "\n\n";
		empty = empty + "(" + GetCombinationString(_UIGreenColor, "2") + ")Set:\n";
		empty = empty + "  +" + GetCombinationString(_UIGreenColor, "0") + " Health\n";
		empty = empty + "  +" + GetCombinationString(_UIGreenColor, "0") + " Defense\n\n";
		empty = empty + GetCombinationString(_UIYellowColor, "Hero Bonuses") + "\n\n";
		empty = empty + GetCombinationString(_UIGreenColor, "RobotMan") + ":\n";
		empty = empty + "  +" + GetCombinationString(_UIGreenColor, "0") + " Health\n\n";
		empty = empty + GetCombinationString(_UIPurpleColor, "Soldier") + ":\n";
		return empty + "  +" + GetCombinationString(_UIGreenColor, "0") + " Damage";
	}

	public static string GetStuffDetailInfo(Defined.RANK_TYPE rankType, string description)
	{
		string empty = string.Empty;
		return GetCombinationString(rankType, GetQualityNameByRankType(rankType)) + "\n\n\n\n\n\n\n\n\n\n\n\n" + description;
	}

	private static void GetHeroSkillInfo(Player.CharacterType characterType, int skillLV, int skillStars, ref Dictionary<string, float> msgData)
	{
		DataConf.HeroSkillInfo heroSkillInfo = DataCenter.Conf().GetHeroSkillInfo(characterType, skillLV, skillStars);
		switch (characterType)
		{
		case Player.CharacterType.Mike:
		{
			DataConf.SkillMike skillMike = (DataConf.SkillMike)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillMike.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Chris:
		{
			DataConf.SkillChris skillChris = (DataConf.SkillChris)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillChris.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Lili:
		{
			DataConf.SkillLili skillLili = (DataConf.SkillLili)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
				msgData["[TSkill#br*]"] = skillLili.GetBR();
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
				msgData["[TSkill#dr*]"] = skillLili.GetDR();
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Vasily:
		{
			DataConf.SkillVasily skillVasily = (DataConf.SkillVasily)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillVasily.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Claire:
		{
			DataConf.SkillClaire skillClaire = (DataConf.SkillClaire)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillClaire.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.FireDragon:
		{
			DataConf.SkillFireDragon skillFireDragon = (DataConf.SkillFireDragon)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillFireDragon.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Zero:
		{
			DataConf.SkillZero skillZero = (DataConf.SkillZero)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillZero.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Arnoud:
		{
			DataConf.SkillArnoud skillArnoud = (DataConf.SkillArnoud)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillArnoud.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.XJohnX:
		{
			DataConf.SkillXJohnX skillXJohnX = (DataConf.SkillXJohnX)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillXJohnX.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Clint:
		{
			DataConf.SkillClint skillClint = (DataConf.SkillClint)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillClint.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Eva:
		{
			DataConf.SkillEva skillEva = (DataConf.SkillEva)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
				msgData["[TSkill#TargetHitRateDecrease*]"] = skillEva.GetTargetHitRateDecrease();
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
				msgData["[TSkill#TargetMoveSpeedDecrease*]"] = skillEva.GetTargetMoveSpeedDecrease();
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
				msgData["[TSkill#TargetAtkDecrease*]"] = skillEva.GetTargetAtkDecrease();
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Jason:
		{
			DataConf.SkillJason skillJason = (DataConf.SkillJason)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillJason.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Tanya:
		{
			DataConf.SkillTanya skillTanya = (DataConf.SkillTanya)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillTanya.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Bourne:
		{
			DataConf.SkillBourne skillBourne = (DataConf.SkillBourne)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillBourne.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Rock:
		{
			DataConf.SkillRock skillRock = (DataConf.SkillRock)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillRock.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Wesker:
		{
			DataConf.SkillWesker skillWesker = (DataConf.SkillWesker)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillWesker.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Oppenheimer:
		{
			DataConf.SkillOppenheimer skillOppenheimer = (DataConf.SkillOppenheimer)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillOppenheimer.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		case Player.CharacterType.Shepard:
		{
			DataConf.SkillShepard skillShepard = (DataConf.SkillShepard)heroSkillInfo;
			if (msgData.ContainsKey("[TSkill#atk*]"))
			{
				msgData["[TSkill#atk*]"] = skillShepard.GetATK().left;
			}
			if (msgData.ContainsKey("[TSkill#br*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#dr*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetHitRateDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetMoveSpeedDecrease*]"))
			{
			}
			if (msgData.ContainsKey("[TSkill#TargetAtkDecrease*]"))
			{
			}
			if (!msgData.ContainsKey("[TSkill#MINECOUNT*]"))
			{
			}
			break;
		}
		}
	}

	public static string GetHeroSkillInfo(Player.CharacterType characterType, int skillLV, int skillStar, string description)
	{
		string text = description;
		Dictionary<string, float> msgData = new Dictionary<string, float>();
		if (description.Contains("[TSkill#atk*]"))
		{
			msgData.Add("[TSkill#atk*]", -1f);
		}
		if (description.Contains("[TSkill#br*]"))
		{
			msgData.Add("[TSkill#br*]", -1f);
		}
		if (description.Contains("[TSkill#dr*]"))
		{
			msgData.Add("[TSkill#dr*]", -1f);
		}
		if (description.Contains("[TSkill#TargetHitRateDecrease*]"))
		{
			msgData.Add("[TSkill#TargetHitRateDecrease*]", -1f);
		}
		if (description.Contains("[TSkill#TargetMoveSpeedDecrease*]"))
		{
			msgData.Add("[TSkill#TargetMoveSpeedDecrease*]", -1f);
		}
		if (description.Contains("[TSkill#TargetAtkDecrease*]"))
		{
			msgData.Add("[TSkill#TargetAtkDecrease*]", -1f);
		}
		if (description.Contains("[TSkill#MINECOUNT*]"))
		{
			msgData.Add("[TSkill#MINECOUNT*]", -1f);
		}
		GetHeroSkillInfo(characterType, skillLV, skillStar, ref msgData);
		foreach (KeyValuePair<string, float> item in msgData)
		{
			text = text.Replace(item.Key, item.Value.ToString());
		}
		return text;
	}

	public static int CalMaxLV(int baseLV, int interval)
	{
		int num = 0;
		if (baseLV >= interval && baseLV % interval == 0)
		{
			return baseLV;
		}
		return (baseLV / interval + 1) * interval;
	}

	public static int GetWeaponDamage(DataConf.WeaponData weaponData, int level)
	{
		return -99;
	}

	public static string TimeLeft(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			text = num4 + "d ";
			num5++;
		}
		if (num3 > 0)
		{
			text = text + num3 + "h ";
			num5++;
		}
		if (num > 0 && num5 < 2)
		{
			text = text + num + "m ";
			num5++;
		}
		if (num2 > 0 && num5 < 2)
		{
			text = text + num2 + "s";
			num5++;
		}
		return text;
	}

	public static string TimeLeftFullShow(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " Day" + ((num4 <= 1) ? string.Empty : "s") + ((num3 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " Hour" + ((num3 <= 1) ? string.Empty : "s") + ((num <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num > 0 && num5 < 2)
		{
			num5++;
			string text2 = text;
			text = text2 + num + " Minute" + ((num <= 1) ? string.Empty : "s") + ((num2 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num2 > 0 && num5 < 2)
		{
			num5++;
			string text2 = text;
			text = text2 + num2 + " Second" + ((num2 <= 1) ? string.Empty : "s");
		}
		return text;
	}

	public static string TimeLeftFullShowDHM(long seconds)
	{
		long num = seconds % 60;
		long num2 = seconds / 60;
		long num3 = num2 / 60;
		num2 %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " Day" + ((num4 <= 1) ? string.Empty : "s") + ((num3 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " Hour" + ((num3 <= 1) ? string.Empty : "s") + ((num2 <= 0) ? string.Empty : ", ");
		}
		if (num2 > 0)
		{
			num5++;
			text = text + num2 + " Min";
		}
		return text;
	}

	public static string MinuteTimeBase(int minutes)
	{
		int num = minutes % 60;
		int num2 = minutes / 60;
		int num3 = num2 / 24;
		num2 %= 24;
		string text = string.Empty;
		if (num3 > 0)
		{
			text = text + num3 + "d ";
		}
		if (num2 > 0)
		{
			text = text + num2 + "h ";
		}
		if (num > 0)
		{
			text = text + num + "m";
		}
		return text;
	}

	public static string TimeToStr_AHMS(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = "0" + num3;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num.ToString();
		if (num < 10)
		{
			text2 = "0" + num;
		}
		empty = empty + text2 + ":";
		num5++;
		string text3 = num2.ToString();
		if (num2 < 10)
		{
			text3 = "0" + num2;
		}
		return empty + text3;
	}

	public static string TimeToStr_HMS(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = "0" + num3;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num.ToString();
		if (num < 10)
		{
			text2 = "0" + num;
		}
		empty = empty + text2 + ":";
		num5++;
		string text3 = num2.ToString();
		if (num2 < 10)
		{
			text3 = "0" + num2;
		}
		return empty + text3;
	}

	public static string TimeToStr_MS(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		string empty = string.Empty;
		string text = num.ToString();
		if (num < 10)
		{
			text = "0" + num;
		}
		empty = empty + text + ":";
		string text2 = num2.ToString();
		if (num2 < 10)
		{
			text2 = "0" + num2;
		}
		return empty + text2;
	}

	public static string GetProtocolLaguageCode()
	{
		string languageCode = DevicePlugin.GetLanguageCode();
		if (languageCode.Contains("ja"))
		{
			return "japanese";
		}
		if (languageCode.Contains("zh"))
		{
			return "chinese";
		}
		if (languageCode.Contains("ko"))
		{
			return "korean";
		}
		return "english";
	}

	public static void PDebug(string _str, string _strlevel = "1")
	{
		if (UIConstant.DebugMode)
		{
			if (_strlevel.Contains("1"))
			{
			}
			if (_strlevel.Contains("2"))
			{
			}
			if (_strlevel.Contains("3"))
			{
			}
			if (_strlevel.Contains("4"))
			{
				NGUIDebug.Log(_str);
			}
			if (!_strlevel.Contains("5"))
			{
			}
		}
	}

	public static void ShowReviewMessageBox()
	{
		UIDialogManager.Instance.reviewDialogScript.Show();
	}

	public static void ShowOpenClik(bool show_full)
	{
		if (OpenClikPlugin.IsAdReady())
		{
			bOpenClikShow_full = show_full;
			OpenClikPlugin.Show(show_full);
			bOpenClikShow = true;
		}
		else
		{
			Debug.LogWarning("ShowOpenClik show_full is " + show_full + ", but IsAdReady is false");
		}
	}

	public static void HideOpenClik()
	{
		OpenClikPlugin.Hide();
		bOpenClikShow = false;
	}
}
