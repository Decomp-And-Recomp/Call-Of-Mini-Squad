using System.Collections.Generic;
using CoMDS2;
using UnityEngine;

public class UITeamModelManager : MonoBehaviour
{
	public class ModelInfo
	{
		public int siteIndex = -1;

		public string fileName = string.Empty;

		public GameObject gameObject;
	}

	public IconRenderCamera iconRenderCamera;

	private Dictionary<int, ModelInfo> dictModelInfos = new Dictionary<int, ModelInfo>();

	private GameObject Create(int siteIndex, string fileName, Player.CharacterType ct, Weapon.WeaponType weapontType, Defined.RANK_TYPE rankType)
	{
		GameObject gameObject = Resources.Load("Models/NewCharacters/" + fileName) as GameObject;
		Object original = gameObject.transform.Find("Model").gameObject;
		Object original2 = gameObject.transform.Find("AudioTalk").gameObject;
		GameObject gameObject2 = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject gameObject3 = Object.Instantiate(original2, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject3.GetComponent<AudioSource>().spatialBlend = 0f;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		if (ct != Player.CharacterType.Rock)
		{
			Transform transform = null;
			transform = gameObject2.transform.Find("Bip01/Spine_00/Bip01 Spine/Spine_0/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 Prop1");
			Weapon weapon = WeaponBuilder.CreateWeaponPlayer(weapontType, 0);
			weapon.Mount(gameObject2.transform, transform, null);
			if (ct == Player.CharacterType.Lili || ct == Player.CharacterType.Clint || ct == Player.CharacterType.Eva || ct == Player.CharacterType.Bourne)
			{
				((WeaponPistol)weapon).GetLeftGun().SetActive(false);
				Transform transform2 = ((WeaponPistol)weapon).GetRightGun().GetTransform().Find("EffectLight");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
			}
			else
			{
				Transform transform3 = weapon.GetTransform().Find("EffectLight");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
			}
		}
		if (ct == Player.CharacterType.Rock)
		{
			gameObject2.transform.localScale = new Vector3(133f, 133f, 133f);
		}
		else
		{
			gameObject2.transform.localScale = new Vector3(165f, 165f, 165f);
		}
		gameObject2.layer = 24;
		return gameObject2;
	}

	private GameObject Create(int siteIndex, DataConf.HeroData heroData, Defined.RANK_TYPE rankType)
	{
		GameObject gameObject = Resources.Load("Models/NewCharacters/" + heroData.modelFileName) as GameObject;
		Object original = gameObject.transform.Find("Model").gameObject;
		Object original2 = gameObject.transform.Find("AudioTalk").gameObject;
		GameObject gameObject2 = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject gameObject3 = Object.Instantiate(original2, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		if (heroData.characterType != Player.CharacterType.Rock)
		{
			Transform transform = null;
			transform = gameObject2.transform.Find("Bip01/Spine_00/Bip01 Spine/Spine_0/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 Prop1");
			Weapon weapon = WeaponBuilder.CreateWeaponPlayer(heroData.weaponType, 0);
			weapon.Mount(gameObject2.transform, transform, null);
			if (heroData.characterType == Player.CharacterType.Lili || heroData.characterType == Player.CharacterType.Clint || heroData.characterType == Player.CharacterType.Eva || heroData.characterType == Player.CharacterType.Bourne)
			{
				((WeaponPistol)weapon).GetLeftGun().SetActive(false);
				Transform transform2 = ((WeaponPistol)weapon).GetRightGun().GetTransform().Find("EffectLight");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
			}
			else
			{
				Transform transform3 = weapon.GetTransform().Find("EffectLight");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
			}
		}
		if (heroData.characterType == Player.CharacterType.Rock)
		{
			gameObject2.transform.localScale = new Vector3(133f, 133f, 133f);
		}
		else
		{
			gameObject2.transform.localScale = new Vector3(165f, 165f, 165f);
		}
		gameObject2.layer = 24;
		return gameObject2;
	}

	public bool AddModelInfo(int siteIndex, string heroName, string fileName, Player.CharacterType ct, Weapon.WeaponType weapontType, Defined.RANK_TYPE rankType, bool bNeedAnimation, float fYRoation)
	{
		if (!dictModelInfos.ContainsKey(siteIndex))
		{
			ModelInfo modelInfo = new ModelInfo();
			modelInfo.siteIndex = siteIndex;
			modelInfo.fileName = fileName;
			modelInfo.gameObject = Create(modelInfo.siteIndex, modelInfo.fileName, ct, weapontType, rankType);
			dictModelInfos.Add(siteIndex, modelInfo);
			iconRenderCamera.Set(modelInfo.siteIndex, modelInfo.gameObject);
			GameObject anchorGO = iconRenderCamera.Get(modelInfo.siteIndex);
			if (anchorGO == null)
			{
				if (modelInfo.gameObject != null) Object.DestroyImmediate(modelInfo.gameObject);
				dictModelInfos.Remove(siteIndex);
				return false;
			}
			anchorGO.transform.localPosition = new Vector3(anchorGO.transform.localPosition.x, anchorGO.transform.localPosition.y - 150f, anchorGO.transform.localPosition.z);
			anchorGO.transform.Rotate(0f, 180f + fYRoation, 0f);
			if (bNeedAnimation)
			{
				UITeamModelAnimationManager uITeamModelAnimationManager = modelInfo.gameObject.GetComponent<UITeamModelAnimationManager>();
				if (uITeamModelAnimationManager == null)
				{
					uITeamModelAnimationManager = modelInfo.gameObject.AddComponent<UITeamModelAnimationManager>();
				}
				string defaultAnimaName = DataCenter.Conf().GetNewCharacterAnim(heroName, "Idle").name;
				string showAnimName = DataCenter.Conf().GetNewCharacterAnim(heroName, "Show").name;
				uITeamModelAnimationManager.SetAnimationInfos(defaultAnimaName, showAnimName);
				uITeamModelAnimationManager.PlayDefaultAnim(true);
			}
			return true;
		}
		return false;
	}

	public bool AddModelInfo(int siteIndex, DataConf.HeroData heroData, Defined.RANK_TYPE rankType, bool bNeedAnimation, float fYRoation)
	{
		if (!dictModelInfos.ContainsKey(siteIndex))
		{
			ModelInfo modelInfo = new ModelInfo();
			modelInfo.siteIndex = siteIndex;
			modelInfo.fileName = heroData.modelFileName;
			modelInfo.gameObject = Create(modelInfo.siteIndex, heroData, rankType);
			dictModelInfos.Add(siteIndex, modelInfo);
			iconRenderCamera.Set(modelInfo.siteIndex, modelInfo.gameObject);
			GameObject anchorGO = iconRenderCamera.Get(modelInfo.siteIndex);
			if (anchorGO == null)
			{
				if (modelInfo.gameObject != null) Object.DestroyImmediate(modelInfo.gameObject);
				dictModelInfos.Remove(siteIndex);
				return false;
			}
			anchorGO.transform.localPosition = new Vector3(anchorGO.transform.localPosition.x, anchorGO.transform.localPosition.y - 150f, anchorGO.transform.localPosition.z);
			anchorGO.transform.Rotate(0f, 180f + fYRoation, 0f);
			if (bNeedAnimation)
			{
				string empty = string.Empty;
				empty = DataCenter.Conf().GetNewCharacterAnim(heroData.name, "Idle").name;
				modelInfo.gameObject.GetComponent<Animation>()[empty].wrapMode = WrapMode.Loop;
				modelInfo.gameObject.GetComponent<Animation>().Play(empty);
			}
			return true;
		}
		return false;
	}

	public bool AddModelInfo_Force(int siteIndex, string heroName, string fileName, Player.CharacterType ct, Weapon.WeaponType weapontType, Defined.RANK_TYPE rankType, bool bNeedAnimation, float fYRoation)
	{
		bool flag = false;
		if (!dictModelInfos.ContainsKey(siteIndex))
		{
			flag = true;
		}
		else
		{
			Object.DestroyImmediate(dictModelInfos[siteIndex].gameObject);
			dictModelInfos.Remove(siteIndex);
			flag = false;
		}
		AddModelInfo(siteIndex, heroName, fileName, ct, weapontType, rankType, bNeedAnimation, fYRoation);
		return flag;
	}

	public bool AddModelInfo_Force(int siteIndex, DataConf.HeroData heroData, Defined.RANK_TYPE rankType, bool bNeedAnimation, float fYRoation)
	{
		bool flag = false;
		if (!dictModelInfos.ContainsKey(siteIndex))
		{
			flag = true;
		}
		else
		{
			Object.DestroyImmediate(dictModelInfos[siteIndex].gameObject);
			dictModelInfos.Remove(siteIndex);
			flag = false;
		}
		AddModelInfo(siteIndex, heroData, rankType, bNeedAnimation, fYRoation);
		return flag;
	}

	public Rect GetTexUV(int siteIndex)
	{
		Rect uv = new Rect(0f, 0f, 0f, 0f);
		iconRenderCamera.GetUV(siteIndex, ref uv);
		return uv;
	}

	public GameObject GetModel(int siteIndex)
	{
		GameObject gameObject = null;
		return iconRenderCamera.Get(siteIndex);
	}
}
