using System;
using UnityEngine;

namespace CoMDS2
{
	public class BattleUIEvent : MonoBehaviour
	{
		public enum UIControlID
		{
			BT_Skill = 0,
			Label_player_icon_leader = 1,
			Label_player_icon_teammate_1 = 2,
			Label_player_icon_teammate_2 = 3,
			Label_player_icon_teammate_3 = 4,
			Label_player_icon_teammate_4 = 5,
			Label_player_hp_bar_leader = 6,
			Label_player_hp_bar_teammate_1 = 7,
			Label_player_hp_bar_teammate_2 = 8,
			Label_player_hp_bar_teammate_3 = 9,
			Label_player_hp_bar_teammate_4 = 10,
			Label_player_clip_leader = 11,
			Label_player_clip_teammate_1 = 12,
			Label_player_clip_teammate_2 = 13,
			Label_player_clip_teammate_3 = 14,
			Label_player_clip_teammate_4 = 15,
			Panel_player_leader = 16,
			Panel_player_teammate_1 = 17,
			Panel_player_teammate_2 = 18,
			Panel_player_teammate_3 = 19,
			Panel_player_teammate_4 = 20,
			Panel_Target = 21,
			Label_target_name = 22,
			Label_target_hp_bar = 23,
			Label_target_hp_groove_num = 24,
			Target_hp_bar_boss = 25,
			Target_hp_bar_normal = 26,
			Rank_Frame_Leader = 27,
			Rank_Frame_Teammate1 = 28,
			Rank_Frame_Teammate2 = 29,
			Rank_Frame_Teammate3 = 30,
			Rank_Frame_Teammate4 = 31,
			ZombieIcon = 32,
			Label_Elite_Attibute = 33,
			Panel_TargetIndicates = 90,
			Panel_GamePause = 100,
			Pause_BT_Resume = 101,
			Pause_BT_Quit = 102,
			Panel_GameWin = 115,
			Win_BT_OK = 116,
			Win_Label_Money = 117,
			Win_Label_TeamExp = 118,
			Win_Label_BattleTime = 119,
			Win_StarIcons = 120,
			Win_ExtraRewards = 121,
			Panel_GameFailed = 130,
			BT_GameFailed_OK = 131,
			BT_GameFailed_UseCrystal = 132,
			BT_GameFailed_Quit = 133,
			Panel_GameRetreat = 145,
			BT_Pause = 160,
			Label_Select_Item = 161,
			Panel_GameUI = 200,
			Panel_GameReady = 201,
			PVP_TargetPanel = 300,
			PVP_Label_Target_icon_leader = 301,
			PVP_Label_Target_icon_teammate_1 = 302,
			PVP_Label_Target_icon_teammate_2 = 303,
			PVP_Label_Target_icon_teammate_3 = 304,
			PVP_Label_Target_icon_teammate_4 = 305,
			PVP_Label_Target_hp_bar_leader = 306,
			PVP_Label_Target_hp_bar_teammate_1 = 307,
			PVP_Label_Target_hp_bar_teammate_2 = 308,
			PVP_Label_Target_hp_bar_teammate_3 = 309,
			PVP_Label_Target_hp_bar_teammate_4 = 310,
			PVP_Label_Target_clip_leader = 311,
			PVP_Label_Target_clip_teammate_1 = 312,
			PVP_Label_Target_clip_teammate_2 = 313,
			PVP_Label_Target_clip_teammate_3 = 314,
			PVP_Label_Target_clip_teammate_4 = 315,
			PVP_Panel_Target_leader = 316,
			PVP_Panel_Target_teammate_1 = 317,
			PVP_Panel_Target_teammate_2 = 318,
			PVP_Panel_Target_teammate_3 = 319,
			PVP_Panel_Target_teammate_4 = 320,
			PVP_Rank_Frame_Target_Leader = 321,
			PVP_Rank_Frame_Target_Teammate1 = 322,
			PVP_Rank_Frame_Target_Teammate2 = 323,
			PVP_Rank_Frame_Target_Teammate3 = 324,
			PVP_Rank_Frame_Target_Teammate4 = 325,
			Panel_PVPGameUI = 326,
			PVP_MyTeamHpBar = 327,
			PVP_MyName = 328,
			PVP_TargetTeamHpBar = 329,
			PVP_TargetName = 330,
			PVP_PanelGameWin = 331,
			PVP_PanelGameFailed = 332,
			Pause_BT_Music = 333,
			Pause_BT_SFX = 334,
			EnemyHpBars = 335,
			HurtFlash_Frame_Leader = 400,
			HurtFlash_Frame_Teammate1 = 401,
			HurtFlash_Frame_Teammate2 = 402,
			HurtFlash_Frame_Teammate3 = 403,
			HurtFlash_Frame_Teammate4 = 404,
			Panel_Reload_UpHead = 405,
			Label_Player_HpBar_UpHead_Leader = 406,
			Label_Player_HpBar_UpHead_Teammate_1 = 407,
			Label_Player_HpBar_UpHead_Teammate_2 = 408,
			Label_Player_HpBar_UpHead_Teammate_3 = 409,
			Label_Player_HpBar_UpHead_Teammate_4 = 410,
			CameraView = 420,
			SwitchPlayer = 421,
			SkillManager = 422,
			Dialog = 450,
			DialogButtonNext = 451,
			DialogButtonOK = 452,
			BT_SwitchToRPG = 453,
			BT_SwitchToSquad = 454,
			Panel_ProgressIndicates = 455,
			BT_CameraMode1 = 456,
			BT_CameraMode2 = 457,
			BT_CameraMode3 = 458,
			Label_Clip = 459,
			BT_ChangePlayer1 = 460,
			BT_ChangePlayer2 = 461,
			BT_ChangePlayer3 = 462,
			BT_ChangePlayer4 = 463,
			BT_ChangePlayer5 = 464,
			BT_ChangePlayer6 = 465,
			BT_ChangePlayer7 = 466,
			BT_ChangePlayer8 = 467,
			BT_ShiftGun = 468,
			BT_SceneChannel01 = 469,
			BT_SceneChannel02 = 470,
			BT_SceneChannel03 = 471,
			BT_SceneLaboratory01 = 472,
			BT_SceneLaboratory02 = 473,
			Label_SkillCDTime = 474,
			BT_SaveData = 475
		}

		public enum SoundTriggerType
		{
			OnClik = 0,
			OnPress = 1,
			OnRelease = 2
		}

		private static int HP_PER_GROOVE = 200;

		public UIControlID controlId;

		public static bool s_anyButtonDown;

		private ITAudioEvent playSound;

		private UILabel m_labelText;

		private UISlider m_slider;

		private Color m_color_dark;

		private Color m_color_light;

		private UISprite m_sprite;

		private UITexture m_texture;

		private int m_CharacterID = -1;

		private DS2ActiveObject m_target;

		private float m_timer;

		private int m_iValue;

		private int m_iValueMax;

		private float m_fValue;

		private float m_fValue2;

		private float m_fValueMax;

		private float m_fDeltaValue;

		private int m_iLastValue;

		private UIWidget[] m_widgets;

		public GameObject[] m_gameObjects;

		private bool m_bRunOnlyOnce;

		private bool m_bVisible = true;

		public void Awake()
		{
			UIControlManager.Instance.AddControl((int)controlId, base.gameObject);
		}

		public void Start()
		{
			switch (controlId)
			{
			case UIControlID.Label_target_name:
			case UIControlID.Label_target_hp_groove_num:
			case UIControlID.Label_Elite_Attibute:
			case UIControlID.PVP_Label_Target_clip_leader:
			case UIControlID.PVP_Label_Target_clip_teammate_1:
			case UIControlID.PVP_Label_Target_clip_teammate_2:
			case UIControlID.PVP_Label_Target_clip_teammate_3:
			case UIControlID.PVP_Label_Target_clip_teammate_4:
			case UIControlID.Label_SkillCDTime:
				m_labelText = base.gameObject.GetComponent<UILabel>();
				break;
			case UIControlID.Label_player_clip_leader:
			case UIControlID.Label_player_clip_teammate_1:
			case UIControlID.Label_player_clip_teammate_2:
			case UIControlID.Label_player_clip_teammate_3:
			case UIControlID.Label_player_clip_teammate_4:
			{
				GameObject gameObject8 = base.transform.Find("ClipNum").gameObject;
				m_labelText = gameObject8.GetComponent<UILabel>();
				m_gameObjects = new GameObject[1];
				m_gameObjects[0] = base.transform.Find("ClipIconLight").gameObject;
				m_gameObjects[0].SetActive(false);
				GameObject gameObject9 = base.transform.Find("ProgressClip/Foreground").gameObject;
				m_sprite = gameObject9.GetComponent<UISprite>();
				Player playerBySiteNum = GameBattle.m_instance.GetPlayerBySiteNum((int)(controlId - 11));
				m_sprite.fillAmount = 0f;
				break;
			}
			case UIControlID.Label_player_hp_bar_leader:
			case UIControlID.Label_player_hp_bar_teammate_1:
			case UIControlID.Label_player_hp_bar_teammate_2:
			case UIControlID.Label_player_hp_bar_teammate_3:
			case UIControlID.Label_player_hp_bar_teammate_4:
			{
				m_slider = base.gameObject.GetComponent<UISlider>();
				m_widgets = new UISprite[2];
				m_widgets[0] = m_gameObjects[0].GetComponent<UISprite>();
				m_widgets[1] = m_gameObjects[1].GetComponent<UISprite>();
				m_gameObjects[1].SetActive(false);
				int num12 = (int)(controlId - 6);
				if (num12 < GameBattle.m_instance.GetPlayerList().Length)
				{
					TeamSiteData teamSiteData2 = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num12);
					if (teamSiteData2.playerData != null)
					{
						break;
					}
					for (int num13 = num12; num13 < DataCenter.Save().GetTeamData().teamSitesData.Length; num13++)
					{
						teamSiteData2 = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num13);
						if (teamSiteData2.playerData != null)
						{
							break;
						}
					}
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.Label_Player_HpBar_UpHead_Leader:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_1:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_2:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_3:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_4:
			{
				m_slider = base.gameObject.GetComponent<UISlider>();
				m_widgets = new UISprite[2];
				m_widgets[0] = m_gameObjects[0].GetComponent<UISprite>();
				m_widgets[1] = m_gameObjects[1].GetComponent<UISprite>();
				m_gameObjects[1].SetActive(false);
				int num17 = (int)(controlId - 406);
				if (num17 < GameBattle.m_instance.GetPlayerList().Length)
				{
					TeamSiteData teamSiteData5 = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num17);
					if (teamSiteData5.playerData != null)
					{
						break;
					}
					for (int num18 = num17; num18 < DataCenter.Save().GetTeamData().teamSitesData.Length; num18++)
					{
						teamSiteData5 = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num18);
						if (teamSiteData5.playerData != null)
						{
							break;
						}
					}
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.Target_hp_bar_normal:
				m_slider = base.gameObject.GetComponent<UISlider>();
				break;
			case UIControlID.Label_player_icon_leader:
			case UIControlID.Label_player_icon_teammate_1:
			case UIControlID.Label_player_icon_teammate_2:
			case UIControlID.Label_player_icon_teammate_3:
			case UIControlID.Label_player_icon_teammate_4:
			case UIControlID.ZombieIcon:
			case UIControlID.PVP_Label_Target_icon_leader:
			case UIControlID.PVP_Label_Target_icon_teammate_1:
			case UIControlID.PVP_Label_Target_icon_teammate_2:
			case UIControlID.PVP_Label_Target_icon_teammate_3:
			case UIControlID.PVP_Label_Target_icon_teammate_4:
				m_sprite = base.gameObject.GetComponent<UISprite>();
				break;
			case UIControlID.BT_Skill:
			{
				m_color_dark = new Color(0.4f, 0.4f, 0.4f);
				m_color_light = Color.white;
				GameObject gameObject10 = base.transform.Find("skillIcon").gameObject;
				GameObject gameObject11 = base.transform.Find("Foreground").gameObject;
				GameObject gameObject12 = base.transform.Find("Background").gameObject;
				GameObject gameObject13 = base.transform.Find("Background2").gameObject;
				m_widgets = new UISprite[4];
				m_widgets[0] = gameObject10.GetComponent<UISprite>();
				m_widgets[1] = gameObject11.GetComponent<UISprite>();
				m_widgets[2] = gameObject12.GetComponent<UISprite>();
				m_widgets[3] = gameObject13.GetComponent<UISprite>();
				int num19 = -1;
				num19 = ((base.gameObject.name == "Skill") ? (-1) : ((!(base.gameObject.name == "Skill1")) ? ((base.gameObject.name == "Skill2") ? 1 : ((base.gameObject.name == "Skill3") ? 2 : ((!(base.gameObject.name == "Skill4")) ? (-1) : 3))) : 0));
				Player player = GameBattle.m_instance.GetPlayer(num19);
				if (player == null)
				{
					base.gameObject.SetActive(false);
					break;
				}
				if (num19 != -1)
				{
					if (!DataCenter.Save().m_bManualUseSkill)
					{
						base.gameObject.SetActive(false);
						break;
					}
					base.gameObject.SetActive(true);
				}
				if (player != null)
				{
					float skillCDTime = player.GetSkillCDTime();
					if (skillCDTime > 0f || !player.Alive())
					{
						m_widgets[2].gameObject.SetActive(true);
						m_widgets[3].gameObject.SetActive(false);
						float skillTotalCDTime = player.GetSkillTotalCDTime();
						float fillAmount = skillCDTime / skillTotalCDTime;
						((UISprite)m_widgets[1]).fillAmount = fillAmount;
					}
					else if (GameBattle.m_instance.SkillCommonnalityCDTime > 0f)
					{
						m_widgets[0].color = m_color_dark;
						float skillCommonnalityTotalCDTime = GameBattle.m_instance.SkillCommonnalityTotalCDTime;
						float fillAmount2 = GameBattle.m_instance.SkillCommonnalityCDTime / skillCommonnalityTotalCDTime;
						((UISprite)m_widgets[1]).fillAmount = fillAmount2;
					}
					else
					{
						m_widgets[2].gameObject.SetActive(false);
						m_widgets[3].gameObject.SetActive(true);
						((UISprite)m_widgets[1]).fillAmount = 0f;
					}
					m_CharacterID = player.heroIndex;
					((UISprite)m_widgets[0]).spriteName = player.skillInfo.fileName;
				}
				break;
			}
			case UIControlID.Panel_player_leader:
			case UIControlID.Panel_player_teammate_1:
			case UIControlID.Panel_player_teammate_2:
			case UIControlID.Panel_player_teammate_3:
			case UIControlID.Panel_player_teammate_4:
			{
				int num = (int)(controlId - 16);
				if (num < GameBattle.m_instance.GetPlayerList().Length)
				{
					TeamSiteData teamSiteData = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)num);
					if (teamSiteData.playerData == null)
					{
						for (int i = num; i < DataCenter.Save().GetTeamData().teamSitesData.Length; i++)
						{
							teamSiteData = DataCenter.Save().GetTeamSiteData((Defined.TEAM_SITE)i);
							if (teamSiteData.playerData != null)
							{
								break;
							}
						}
					}
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				if (!DataCenter.State().isPVPMode)
				{
					m_gameObjects = new GameObject[1];
					m_gameObjects[0] = UIControlManager.Instance.GetControl(161);
				}
				break;
			}
			case UIControlID.PVP_Panel_Target_leader:
			case UIControlID.PVP_Panel_Target_teammate_1:
			case UIControlID.PVP_Panel_Target_teammate_2:
			case UIControlID.PVP_Panel_Target_teammate_3:
			case UIControlID.PVP_Panel_Target_teammate_4:
			{
				if (!DataCenter.State().isPVPMode)
				{
					base.gameObject.SetActive(false);
					break;
				}
				TeamSiteData teamSiteData3 = DataCenter.User().PVP_SelectTarget.team.teamSitesData[(int)(controlId - 316)];
				if (teamSiteData3.playerData == null)
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.PVP_Label_Target_hp_bar_leader:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_1:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_2:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_3:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_4:
			{
				m_slider = base.gameObject.GetComponent<UISlider>();
				TeamSiteData teamSiteData4 = DataCenter.User().PVP_SelectTarget.team.teamSitesData[(int)(controlId - 306)];
				if (teamSiteData4.playerData == null)
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.Panel_Target:
				m_gameObjects = new GameObject[3];
				m_gameObjects[0] = base.transform.Find("HpBar/NormalHp").gameObject;
				m_gameObjects[1] = base.transform.Find("HpBar/BossHp").gameObject;
				m_gameObjects[2] = base.transform.Find("name").gameObject;
				m_labelText = m_gameObjects[2].GetComponent<UILabel>();
				m_target = null;
				base.gameObject.SetActive(false);
				break;
			case UIControlID.Label_target_hp_bar:
			{
				int num7 = base.transform.childCount - 1;
				m_sprite = base.transform.Find("CursorBG").gameObject.GetComponent<UISprite>();
				m_sprite.depth = 1;
				m_widgets = new UISprite[num7];
				for (int num8 = 0; num8 < num7; num8++)
				{
					GameObject gameObject4 = base.transform.Find("Foreground_" + num8).gameObject;
					m_widgets[num8] = gameObject4.GetComponent<UISprite>();
				}
				break;
			}
			case UIControlID.Panel_TargetIndicates:
			{
				GameObject gameObject3 = base.transform.Find("indicate").gameObject;
				m_gameObjects = new GameObject[30];
				for (int n = 0; n < 30; n++)
				{
					m_gameObjects[n] = UnityEngine.Object.Instantiate(gameObject3) as GameObject;
					m_gameObjects[n].name = gameObject3.name;
					m_gameObjects[n].transform.parent = base.transform;
					m_gameObjects[n].transform.localPosition = gameObject3.transform.localPosition;
					m_gameObjects[n].transform.localScale = gameObject3.transform.localScale;
					m_gameObjects[n].SetActive(false);
				}
				UnityEngine.Object.DestroyImmediate(gameObject3);
				float num4 = Screen.width >> 1;
				float num5 = Screen.height >> 1;
				m_fValueMax = Mathf.Sqrt(num4 * num4 + num5 * num5);
				break;
			}
			case UIControlID.Panel_ProgressIndicates:
			{
				float num2 = Screen.width >> 1;
				float num3 = Screen.height >> 1;
				m_fValueMax = Mathf.Sqrt(num2 * num2 + num3 * num3);
				break;
			}
			case UIControlID.BT_GameFailed_UseCrystal:
			{
				int reviveItem2 = DataCenter.Save().ReviveItem;
				if (reviveItem2 > 0)
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.BT_GameFailed_OK:
			{
				int reviveItem = DataCenter.Save().ReviveItem;
				if (reviveItem <= 0)
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
			case UIControlID.Panel_GamePause:
				base.transform.localPosition = Vector3.zero;
				base.gameObject.SetActive(false);
				break;
			case UIControlID.Panel_GameWin:
				base.transform.localPosition = Vector3.zero;
				base.gameObject.SetActive(false);
				break;
			case UIControlID.Win_Label_Money:
			{
				m_labelText = base.gameObject.GetComponent<UILabel>();
				int num11 = 0;
				if (Util.s_debug)
				{
					DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
					num11 = 999;
					m_iValue = DataCenter.Save().Money;
					DataCenter.Save().Money += num11;
				}
				else
				{
					num11 = DataCenter.Save().selectLevelDropData.money;
				}
				m_iValue = 0;
				m_iValueMax = num11;
				m_fDeltaValue = (float)num11 / 0.5f;
				if (num11 <= 0)
				{
					m_gameObjects[0].SetActive(false);
				}
				playSound = base.gameObject.GetComponentInChildren<ITAudioEvent>();
				m_bRunOnlyOnce = false;
				break;
			}
			case UIControlID.Win_Label_TeamExp:
			{
				m_labelText = base.gameObject.GetComponent<UILabel>();
				int num6 = 0;
				if (!Util.s_debug)
				{
					num6 = DataCenter.Save().selectLevelDropData.exp;
					m_iValue = 0;
					m_fValue = 0f;
				}
				m_iValueMax = num6;
				m_fDeltaValue = (float)num6 / 0.5f;
				if (num6 <= 0)
				{
					m_gameObjects[0].SetActive(false);
				}
				playSound = base.gameObject.GetComponentInChildren<ITAudioEvent>();
				m_bRunOnlyOnce = false;
				break;
			}
			case UIControlID.Win_StarIcons:
			{
				int childCount2 = base.transform.childCount;
				m_gameObjects = new GameObject[childCount2];
				for (int l = 0; l < childCount2; l++)
				{
					m_gameObjects[l] = base.transform.GetChild(l).gameObject;
					m_gameObjects[l].SetActive(false);
				}
				m_timer = 0f;
				m_iValue = 0;
				break;
			}
			case UIControlID.Win_Label_BattleTime:
				m_bRunOnlyOnce = true;
				m_labelText = base.gameObject.GetComponent<UILabel>();
				break;
			case UIControlID.Win_ExtraRewards:
			{
				//thanks triniti
				DataCenter.Save().selectLevelDropData.extraCrystal = BattleResultController.GetExtraCrystals();
				Transform[] array = new Transform[3];
				for (int num15 = 1; num15 <= 3; num15++)
				{
					array[num15 - 1] = base.transform.Find("Items/ItemPos" + num15);
				}
				GameObject gameObject5 = base.transform.Find("Items/ItemMoney").gameObject;
				GameObject gameObject6 = base.transform.Find("Items/ItemCrystal").gameObject;
				GameObject gameObject7 = base.transform.Find("Items/ItemHonor").gameObject;
				UILabel componentInChildren = gameObject5.GetComponentInChildren<UILabel>();
				UILabel componentInChildren2 = gameObject6.GetComponentInChildren<UILabel>();
				UILabel componentInChildren3 = gameObject7.GetComponentInChildren<UILabel>();
				int num16 = 0;
				if (DataCenter.Save().selectLevelDropData.extraHonor > 0)
				{
					gameObject7.SetActive(true);
					gameObject7.transform.localPosition = array[num16].localPosition;
					componentInChildren3.text = string.Empty + DataCenter.Save().selectLevelDropData.extraHonor;
					num16++;
				}
				else
				{
					gameObject7.SetActive(false);
				}
				if (DataCenter.Save().selectLevelDropData.extraCrystal > 0)
				{
					gameObject6.SetActive(true);
					gameObject6.transform.localPosition = array[num16].localPosition;
					componentInChildren2.text = string.Empty + DataCenter.Save().selectLevelDropData.extraCrystal;
					num16++;
				}
				else
				{
					gameObject6.SetActive(false);
				}
				if (DataCenter.Save().selectLevelDropData.extraMoney > 0)
				{
					gameObject5.SetActive(true);
					gameObject5.transform.localPosition = array[num16].localPosition;
					componentInChildren.text = string.Empty + DataCenter.Save().selectLevelDropData.extraMoney;
					num16++;
				}
				else
				{
					gameObject5.SetActive(false);
				}
				break;
			}
			case UIControlID.Label_Select_Item:
				base.gameObject.SetActive(false);
				break;
			case UIControlID.Rank_Frame_Leader:
			case UIControlID.Rank_Frame_Teammate1:
			case UIControlID.Rank_Frame_Teammate2:
			case UIControlID.Rank_Frame_Teammate3:
			case UIControlID.Rank_Frame_Teammate4:
			case UIControlID.PVP_Rank_Frame_Target_Leader:
			case UIControlID.PVP_Rank_Frame_Target_Teammate1:
			case UIControlID.PVP_Rank_Frame_Target_Teammate2:
			case UIControlID.PVP_Rank_Frame_Target_Teammate3:
			case UIControlID.PVP_Rank_Frame_Target_Teammate4:
			{
				m_gameObjects = new GameObject[4];
				int childCount3 = base.transform.childCount;
				for (int num14 = 0; num14 < childCount3; num14++)
				{
					m_gameObjects[num14] = base.transform.Find("RankFrame" + num14).gameObject;
				}
				break;
			}
			case UIControlID.Panel_GameReady:
				base.transform.localPosition = Vector3.zero;
				m_bRunOnlyOnce = true;
				m_timer = 0f;
				m_iValue = 3;
				m_fValue = 0f;
				m_gameObjects[2].SetActive(false);
				m_gameObjects[1].SetActive(false);
				m_gameObjects[0].SetActive(false);
				break;
			case UIControlID.PVP_TargetPanel:
				if (!DataCenter.State().isPVPMode)
				{
					base.gameObject.SetActive(false);
				}
				break;
			case UIControlID.PVP_MyName:
				m_labelText = base.gameObject.GetComponent<UILabel>();
				m_labelText.text = DataCenter.User().PVP_myData.sName;
				break;
			case UIControlID.PVP_TargetName:
				m_labelText = base.gameObject.GetComponent<UILabel>();
				m_labelText.text = DataCenter.User().PVP_SelectTarget.sName;
				break;
			case UIControlID.PVP_MyTeamHpBar:
			{
				m_slider = base.gameObject.GetComponent<UISlider>();
				DS2ActiveObject[] playerList = GameBattle.m_instance.GetPlayerList();
				for (int num10 = 0; num10 < playerList.Length; num10++)
				{
					m_fValueMax += playerList[num10].hp;
				}
				break;
			}
			case UIControlID.PVP_TargetTeamHpBar:
			{
				m_slider = base.gameObject.GetComponent<UISlider>();
				DS2ActiveObject[] enemyList = GameBattle.m_instance.GetEnemyList();
				for (int num9 = 0; num9 < enemyList.Length; num9++)
				{
					m_fValueMax += enemyList[num9].hp;
				}
				break;
			}
			case UIControlID.PVP_PanelGameWin:
				base.transform.localPosition = Vector3.zero;
				base.gameObject.SetActive(false);
				m_gameObjects = new GameObject[1];
				m_gameObjects[0] = base.transform.Find("labelWin").gameObject;
				m_gameObjects[0].transform.localPosition = new Vector3(-(Screen.width >> 1), m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
				m_fDeltaValue = Mathf.Abs(m_gameObjects[0].transform.localPosition.x / 0.45f * Time.deltaTime);
				break;
			case UIControlID.Panel_GameFailed:
			case UIControlID.PVP_PanelGameFailed:
				base.transform.localPosition = Vector3.zero;
				base.gameObject.SetActive(false);
				break;
			case UIControlID.Pause_BT_Music:
				m_labelText = base.transform.Find("Label").gameObject.GetComponent<UILabel>();
				m_gameObjects = new GameObject[1];
				m_gameObjects[0] = base.transform.Find("cusor").gameObject;
				if (DataCenter.Save().PlayMusic)
				{
					m_labelText.text = "ON";
					m_labelText.transform.localPosition = new Vector3(-17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
					m_gameObjects[0].transform.localPosition = new Vector3(17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
				}
				else
				{
					m_labelText.text = "OFF";
					m_labelText.transform.localPosition = new Vector3(17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
					m_gameObjects[0].transform.localPosition = new Vector3(-17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
				}
				break;
			case UIControlID.Pause_BT_SFX:
				m_labelText = base.transform.Find("Label").gameObject.GetComponent<UILabel>();
				m_gameObjects = new GameObject[1];
				m_gameObjects[0] = base.transform.Find("cusor").gameObject;
				if (DataCenter.Save().PlaySound)
				{
					m_labelText.text = "ON";
					m_labelText.transform.localPosition = new Vector3(-17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
					m_gameObjects[0].transform.localPosition = new Vector3(17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
				}
				else
				{
					m_labelText.text = "OFF";
					m_labelText.transform.localPosition = new Vector3(17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
					m_gameObjects[0].transform.localPosition = new Vector3(-17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
				}
				break;
			case UIControlID.EnemyHpBars:
			{
				GameObject gameObject = base.transform.Find("HpBar").gameObject;
				for (int m = 1; m < 30; m++)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
					gameObject2.name = "HpBar";
					gameObject2.transform.parent = gameObject.transform.parent;
					gameObject2.SetActive(false);
					UIControlManager.Instance.AddEnemyHpBarToBuffer(gameObject2);
				}
				gameObject.SetActive(false);
				UIControlManager.Instance.AddEnemyHpBarToBuffer(gameObject);
				break;
			}
			case UIControlID.HurtFlash_Frame_Leader:
			case UIControlID.HurtFlash_Frame_Teammate1:
			case UIControlID.HurtFlash_Frame_Teammate2:
			case UIControlID.HurtFlash_Frame_Teammate3:
			case UIControlID.HurtFlash_Frame_Teammate4:
				m_sprite = base.gameObject.GetComponent<UISprite>();
				m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
				break;
			case UIControlID.Panel_Reload_UpHead:
			{
				int childCount = base.transform.childCount;
				m_gameObjects = new GameObject[childCount];
				for (int k = 0; k < childCount; k++)
				{
					m_gameObjects[k] = base.transform.GetChild(k).gameObject;
					m_gameObjects[k].SetActive(false);
				}
				break;
			}
			case UIControlID.CameraView:
			{
				if (m_gameObjects == null)
				{
					break;
				}
				for (int j = 0; j < m_gameObjects.Length; j++)
				{
					UIToggle component = m_gameObjects[j].GetComponent<UIToggle>();
					if (j == (int)DataCenter.Save().CameraView)
					{
						component.value = true;
						component.startsActive = true;
						component.activeSprite.alpha = 1f;
					}
					else
					{
						component.value = false;
						component.startsActive = false;
						component.activeSprite.alpha = 0f;
					}
				}
				break;
			}
			case UIControlID.SwitchPlayer:
				m_labelText = base.gameObject.GetComponentInChildren<UILabel>();
				if (DataCenter.Save().m_bCanChangeTeamMember)
				{
					m_labelText.text = "SWITCH PLAYER      ON";
				}
				else
				{
					m_labelText.text = "SWITCH PLAYER      OFF";
				}
				break;
			case UIControlID.DialogButtonNext:
				DialogInit();
				break;
			}
		}

		public void OnPress(bool isDown)
		{
			s_anyButtonDown = isDown;
			TUIHandleManager.s_anyUIButtonDown = isDown;
			if (GameBattle.m_instance.GameState != GameBattle.State.Game)
			{
				if (isDown)
				{
					UIControlID uIControlID = controlId;
					if (uIControlID == UIControlID.Pause_BT_Resume)
					{
						GameObject control = UIControlManager.Instance.GetControl(100);
						control.SetActive(false);
						UIUtil.HideOpenClik();
						GameBattle.m_instance.GameState = GameBattle.State.Game;
						TUIHandleManager.s_anyUIButtonDown = false;
						s_anyButtonDown = false;
					}
					return;
				}
				switch (controlId)
				{
				case UIControlID.Pause_BT_Music:
					DataCenter.Save().PlayMusic = !DataCenter.Save().PlayMusic;
					if (DataCenter.Save().PlayMusic)
					{
						DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
						if (currentGameLevelData.isBossLevel)
						{
							BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.BGM_BOSS);
						}
						else
						{
							BackgroundMusicManager.Instance().PlayBackgroundMusic(currentGameLevelData.sBGM);
						}
					}
					else
					{
						BackgroundMusicManager.Instance().StopBG();
					}
					Save.RequestWrite();
					break;
				case UIControlID.Pause_BT_SFX:
					DataCenter.Save().PlaySound = !DataCenter.Save().PlaySound;
					Save.RequestWrite();
					break;
				}
			}
			else
			{
				if (GameBattle.s_bInputLocked)
				{
					return;
				}
				if (isDown)
				{
					if (Tutorial.Instance.TutorialInProgress)
					{
						if (controlId == UIControlID.BT_Skill)
						{
							if (Tutorial.Instance.TutorialPahseSkill == Tutorial.TutorialPhaseState.InProgress)
							{
								int num = -1;
								if (!(base.gameObject.name == "Skill"))
								{
									TUIHandleManager.s_anyUIButtonDown = false;
									s_anyButtonDown = false;
									return;
								}
								num = -1;
								Tutorial.Instance.TutorialPahseSkill = Tutorial.TutorialPhaseState.Done;
								Player player = GameBattle.m_instance.GetPlayer(num);
								if (player == null)
								{
									TUIHandleManager.s_anyUIButtonDown = false;
									s_anyButtonDown = false;
									base.gameObject.SetActive(false);
									return;
								}
								if (!player.SkillInCDTime() && !(GameBattle.m_instance.SkillCommonnalityCDTime > 0f) && player.Alive())
								{
									player.UseSkill(0);
								}
							}
							TUIHandleManager.s_anyUIButtonDown = false;
							s_anyButtonDown = false;
							return;
						}
						if (Tutorial.Instance.TutorialPahseChangeMode != Tutorial.TutorialPhaseState.InProgress)
						{
							TUIHandleManager.s_anyUIButtonDown = false;
							s_anyButtonDown = false;
							return;
						}
						if (controlId != UIControlID.BT_SwitchToRPG)
						{
							TUIHandleManager.s_anyUIButtonDown = false;
							s_anyButtonDown = false;
							return;
						}
					}
					switch (controlId)
					{
					case UIControlID.BT_Skill:
					{
						int num2 = -1;
						num2 = ((base.gameObject.name == "Skill") ? (-1) : ((!(base.gameObject.name == "Skill1")) ? ((base.gameObject.name == "Skill2") ? 1 : ((base.gameObject.name == "Skill3") ? 2 : ((!(base.gameObject.name == "Skill4")) ? (-1) : 3))) : 0));
						Player player2 = GameBattle.m_instance.GetPlayer(num2);
						if (player2 == null)
						{
							base.gameObject.SetActive(false);
						}
						else if (player2.clique == DS2ActiveObject.Clique.Player && !player2.SkillInCDTime() && !(GameBattle.m_instance.SkillCommonnalityCDTime > 0f) && player2.Alive())
						{
							player2.UseSkill(0);
						}
						break;
					}
					case UIControlID.Panel_player_teammate_1:
					case UIControlID.Panel_player_teammate_2:
					case UIControlID.Panel_player_teammate_3:
					case UIControlID.Panel_player_teammate_4:
						if (DataCenter.Save().m_bCanChangeTeamMember)
						{
							m_gameObjects[0].transform.position = base.transform.position;
							m_gameObjects[0].SetActive(true);
						}
						break;
					case UIControlID.BT_SwitchToRPG:
						if (DataCenter.Save().squadMode && Time.realtimeSinceStartup - GameBattle.m_instance.changeModeLimitTime > 0.3f)
						{
							GameBattle.m_instance.changeModeLimitTime = Time.realtimeSinceStartup;
							UIPlaySound component2 = base.gameObject.GetComponent<UIPlaySound>();
							component2.Play();
							base.gameObject.SetActive(false);
							m_gameObjects[0].SetActive(true);
							DataCenter.Save().squadMode = false;
							GameBattle.m_instance.SetSquadMode(DataCenter.Save().squadMode);
							if (Tutorial.Instance.TutorialPahseChangeMode == Tutorial.TutorialPhaseState.InProgress)
							{
								Tutorial.Instance.TutorialPahseChangeMode = Tutorial.TutorialPhaseState.Done;
							}
							TUIHandleManager.s_anyUIButtonDown = false;
							s_anyButtonDown = false;
						}
						break;
					case UIControlID.BT_SwitchToSquad:
						if (!DataCenter.Save().squadMode && GameBattle.m_instance.GetPlayerAliveList().Length > 1 && Time.realtimeSinceStartup - GameBattle.m_instance.changeModeLimitTime > 0.3f)
						{
							GameBattle.m_instance.changeModeLimitTime = Time.realtimeSinceStartup;
							UIPlaySound component = base.gameObject.GetComponent<UIPlaySound>();
							component.Play();
							base.gameObject.SetActive(false);
							m_gameObjects[0].SetActive(true);
							DataCenter.Save().squadMode = true;
							GameBattle.m_instance.SetSquadMode(DataCenter.Save().squadMode);
							TUIHandleManager.s_anyUIButtonDown = false;
							s_anyButtonDown = false;
						}
						break;
					}
					return;
				}
				if (Tutorial.Instance.TutorialInProgress)
				{
					if (controlId == UIControlID.Panel_player_teammate_1 && Tutorial.Instance.TutorialPahseChangePlayer == Tutorial.TutorialPhaseState.InProgress)
					{
						Tutorial.Instance.TutorialPahseChangePlayer = Tutorial.TutorialPhaseState.Done;
						if (DataCenter.Save().m_bCanChangeTeamMember)
						{
							m_gameObjects[0].SetActive(false);
							int teammateToCurrentControlPlayer = (int)(controlId - 17);
							GameBattle.m_instance.SetTeammateToCurrentControlPlayer(teammateToCurrentControlPlayer);
						}
					}
					return;
				}
				switch (controlId)
				{
				case UIControlID.Panel_player_teammate_1:
				case UIControlID.Panel_player_teammate_2:
				case UIControlID.Panel_player_teammate_3:
				case UIControlID.Panel_player_teammate_4:
					if (DataCenter.Save().m_bCanChangeTeamMember)
					{
						m_gameObjects[0].SetActive(false);
						int teammateToCurrentControlPlayer2 = (int)(controlId - 17);
						GameBattle.m_instance.SetTeammateToCurrentControlPlayer(teammateToCurrentControlPlayer2);
					}
					break;
				}
			}
		}

		public void OnClick()
		{
			if (GameBattle.m_instance.GameState != GameBattle.State.Game)
			{
				switch (controlId)
				{
				case UIControlID.Pause_BT_Quit:
				case UIControlID.Win_BT_OK:
				case UIControlID.BT_GameFailed_Quit:
				{
					UIImageButton component = GetComponent<UIImageButton>();
					if (component.isEnabled)
					{
						FadeInfoScript.Instance.FadeOut();
						FadeInfoScript.Instance.SubFinishEvent(base.gameObject, "Quit");
						component.isEnabled = false;
					}
					GameObject control2 = UIControlManager.Instance.GetControl(101);
					UIImageButton component2 = control2.GetComponent<UIImageButton>();
					component2.isEnabled = false;
					break;
				}
				case UIControlID.BT_GameFailed_OK:
				case UIControlID.BT_GameFailed_UseCrystal:
				{
					GameObject control = UIControlManager.Instance.GetControl(130);
					control.SetActive(false);
					GameBattle.m_instance.GameState = GameBattle.State.Game;
					GameBattle.m_instance.Restart();
					break;
				}
				case UIControlID.SwitchPlayer:
				{
					DataCenter.Save().m_bCanChangeTeamMember = !DataCenter.Save().m_bCanChangeTeamMember;
					DataCenter.Save().m_bManualUseSkill = !DataCenter.Save().m_bCanChangeTeamMember;
					for (int i = 0; i < m_gameObjects.Length; i++)
					{
						m_gameObjects[i].SetActive(DataCenter.Save().m_bManualUseSkill);
					}
					if (DataCenter.Save().m_bCanChangeTeamMember)
					{
						m_labelText.text = "SWITCH PLAYER      ON";
					}
					else
					{
						m_labelText.text = "SWITCH PLAYER      OFF";
					}
					break;
				}
				case UIControlID.DialogButtonOK:
					GameBattle.m_instance.m_UIDialog.SetActive(false);
					if (GameBattle.m_instance.GameState == GameBattle.State.DialogStart)
					{
						GameBattle.m_instance.GameState = GameBattle.State.Ready;
					}
					else if (GameBattle.m_instance.GameState == GameBattle.State.DialogEnd)
					{
						GameBattle.m_instance.GameState = GameBattle.State.Win;
					}
					break;
				}
			}
			else if (!GameBattle.s_bInputLocked)
			{
				switch (controlId)
				{
				case UIControlID.BT_CameraMode1:
					GameBattle.s_debugCameraMode = ((GameBattle.s_debugCameraMode == 0) ? 2 : 0);
					break;
				case UIControlID.BT_CameraMode2:
					GameBattle.s_debugCameraMode = 1;
					break;
				case UIControlID.BT_CameraMode3:
					GameBattle.s_debugCameraMode = 2;
					break;
				case UIControlID.BT_SceneChannel01:
					Application.LoadLevel("TestNewScene");
					break;
				case UIControlID.BT_SceneChannel02:
					Application.LoadLevel("TestSceneChannel02");
					break;
				case UIControlID.BT_SceneChannel03:
					Application.LoadLevel("TestSceneChannel03");
					break;
				case UIControlID.BT_SceneLaboratory01:
					Application.LoadLevel("TestSceneLaboratory01");
					break;
				case UIControlID.BT_SceneLaboratory02:
					Application.LoadLevel("TestSceneLaboratory02");
					break;
				case UIControlID.BT_SaveData:
					Save.Write();
					break;
				case UIControlID.BT_Pause:
					GameBattle.m_instance.GameState = GameBattle.State.Pause;
					UIUtil.ShowOpenClik(false);
					break;
				}
			}
		}

		public void Update()
		{
			if (GameBattle.m_instance.GameState != GameBattle.State.Game)
			{
				switch (controlId)
				{
				case UIControlID.Panel_GameWin:
					if (m_timer == -1f)
					{
						break;
					}
					m_timer += Time.deltaTime;
					if (m_timer >= 3.5f && GameBattle.m_instance.bGetBattleResultData)
					{
						m_timer = -1f;
						BattleUIEvent component = UIControlManager.Instance.GetControl(118).GetComponent<BattleUIEvent>();
						BattleUIEvent component2 = UIControlManager.Instance.GetControl(117).GetComponent<BattleUIEvent>();
						int num = 0;
						int num2 = 0;
						if (Util.s_debug)
						{
							DataConf.GameLevelData currentGameLevelData = DataCenter.Conf().GetCurrentGameLevelData();
							component.m_iValue = DataCenter.Save().Money;
							DataCenter.Save().Money += num;
						}
						else
						{
							num = DataCenter.Save().selectLevelDropData.money;
							num2 = DataCenter.Save().selectLevelDropData.exp;
							component.m_iValue = 0;
							component.m_fValue = 0f;
						}
						component.m_iValueMax = num2;
						component.m_fDeltaValue = (float)num2 / 0.5f;
						component2.m_iValue = 0;
						component2.m_iValueMax = num;
						component2.m_fDeltaValue = (float)num / 0.5f;
						if (num <= 0)
						{
							component2.m_gameObjects[0].SetActive(false);
						}
						if (num2 <= 0)
						{
							component.m_gameObjects[0].SetActive(false);
						}
						m_gameObjects[0].SetActive(false);
						m_gameObjects[1].SetActive(true);
						UIUtil.ShowOpenClik(false);
					}
					break;
				case UIControlID.Win_Label_Money:
					if (m_fValue < (float)m_iValueMax)
					{
						if (!m_bRunOnlyOnce)
						{
							m_bRunOnlyOnce = true;
							playSound.Trigger();
						}
						m_fValue += m_fDeltaValue * Time.deltaTime;
					}
					else
					{
						m_fValue = m_iValueMax;
						playSound.Stop();
					}
					m_iValue = (int)m_fValue;
					m_labelText.text = string.Empty + m_iValue;
					break;
				case UIControlID.Win_Label_TeamExp:
					if (m_fValue < (float)m_iValueMax)
					{
						m_fValue += m_fDeltaValue * Time.deltaTime;
					}
					else
					{
						m_fValue = m_iValueMax;
					}
					m_iValue = (int)m_fValue;
					m_labelText.text = string.Empty + m_iValue;
					break;
				case UIControlID.Win_Label_BattleTime:
					if (m_bRunOnlyOnce)
					{
						m_bRunOnlyOnce = false;
						string text = UIUtil.TimeLeft((long)DataCenter.State().battleTime);
						m_labelText.text = "BATTLE TIME:" + text;
					}
					break;
				case UIControlID.Panel_GameReady:
					if (m_bRunOnlyOnce)
					{
						m_bRunOnlyOnce = false;
						m_gameObjects[2].SetActive(true);
					}
					m_timer += Time.deltaTime;
					if (!(m_timer >= 1f))
					{
						break;
					}
					m_timer = 0f;
					m_gameObjects[m_iValue - 1].SetActive(false);
					m_iValue--;
					if (m_iValue <= 0)
					{
						base.gameObject.SetActive(false);
						GameBattle.m_instance.GameState = GameBattle.State.Game;
						if (!DataCenter.Save().BattleTutorialFinished)
						{
							GameBattle.m_instance.IsPause = true;
						}
					DataCenter.State().battleTime = Time.time;
					}
					else
					{
						m_gameObjects[m_iValue - 1].SetActive(true);
					}
					break;
				case UIControlID.PVP_PanelGameWin:
					if (m_gameObjects[0].transform.localPosition.x < 0f)
					{
						m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x + m_fDeltaValue, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						if (m_gameObjects[0].transform.localPosition.x >= 0f)
						{
							m_gameObjects[0].transform.localPosition = new Vector3(0f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						}
						break;
					}
					if (m_gameObjects[0].transform.localPosition.x > 0f)
					{
						m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x - m_fDeltaValue, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						if (m_gameObjects[0].transform.localPosition.x <= 0f)
						{
							m_gameObjects[0].transform.localPosition = new Vector3(0f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						}
						break;
					}
					m_timer += Time.deltaTime;
					if (m_timer >= 3f && m_timer != -1f)
					{
						m_timer = -1f;
						FadeInfoScript.Instance.FadeOut();
						FadeInfoScript.Instance.SubFinishEvent(base.gameObject, "Quit");
					}
					break;
				case UIControlID.Win_StarIcons:
					if (m_iValue < DataCenter.State().battleStars)
					{
						m_timer += Time.deltaTime;
						if (m_timer >= 1f)
						{
							m_timer = 0f;
							for (int i = 0; i < DataCenter.State().battleStars; i++)
							{
								m_gameObjects[i].SetActive(true);
							}
							m_iValue = DataCenter.State().battleStars;
						}
					}
					else if (m_iValue >= 3)
					{
						if (m_timer == -1f)
						{
							break;
						}
						m_timer += Time.deltaTime;
						if (m_timer >= 1f)
						{
							m_timer = -1f;
							if (DataCenter.Save().selectLevelDropData.extraCrystal > 0 || DataCenter.Save().selectLevelDropData.extraHonor > 0 || DataCenter.Save().selectLevelDropData.extraMoney > 0)
							{
								GameObject control = UIControlManager.Instance.GetControl(121);
								control.SetActive(true);
							}
							GameObject control2 = UIControlManager.Instance.GetControl(116);
							control2.SetActive(true);
						}
					}
					else
					{
						GameObject control3 = UIControlManager.Instance.GetControl(116);
						control3.SetActive(true);
					}
					break;
				case UIControlID.Panel_GameFailed:
				case UIControlID.PVP_PanelGameFailed:
					m_timer += Time.deltaTime;
					if (m_timer >= 3f && m_timer != -1f)
					{
						m_timer = -1f;
						FadeInfoScript.Instance.FadeOut();
						FadeInfoScript.Instance.SubFinishEvent(base.gameObject, "Quit");
					}
					break;
				case UIControlID.Pause_BT_Music:
					if (DataCenter.Save().PlayMusic)
					{
						if (m_gameObjects[0].transform.localPosition.x < 17f)
						{
							float num5 = 5f;
							m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x + num5, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
							if (m_gameObjects[0].transform.localPosition.x >= 17f)
							{
								m_gameObjects[0].transform.localPosition = new Vector3(17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
								m_labelText.text = "ON";
								m_labelText.transform.localPosition = new Vector3(-17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
							}
						}
					}
					else if (m_gameObjects[0].transform.localPosition.x > -17f)
					{
						float num6 = 5f;
						m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x - num6, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						if (m_gameObjects[0].transform.localPosition.x <= -17f)
						{
							m_gameObjects[0].transform.localPosition = new Vector3(-17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
							m_labelText.text = "OFF";
							m_labelText.transform.localPosition = new Vector3(17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
						}
					}
					break;
				case UIControlID.Pause_BT_SFX:
					if (DataCenter.Save().PlaySound)
					{
						if (m_gameObjects[0].transform.localPosition.x < 17f)
						{
							float num3 = 5f;
							m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x + num3, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
							if (m_gameObjects[0].transform.localPosition.x >= 17f)
							{
								m_gameObjects[0].transform.localPosition = new Vector3(17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
								m_labelText.text = "ON";
								m_labelText.transform.localPosition = new Vector3(-17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
							}
						}
					}
					else if (m_gameObjects[0].transform.localPosition.x > -17f)
					{
						float num4 = 5f;
						m_gameObjects[0].transform.localPosition = new Vector3(m_gameObjects[0].transform.localPosition.x - num4, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
						if (m_gameObjects[0].transform.localPosition.x <= -17f)
						{
							m_gameObjects[0].transform.localPosition = new Vector3(-17f, m_gameObjects[0].transform.localPosition.y, m_gameObjects[0].transform.localPosition.z);
							m_labelText.text = "OFF";
							m_labelText.transform.localPosition = new Vector3(17f, m_labelText.transform.localPosition.y, m_labelText.transform.localPosition.z);
						}
					}
					break;
				case UIControlID.DialogButtonNext:
				{
					TypewriterEffectEx typewriterEffectEx = m_labelText.gameObject.GetComponent<TypewriterEffectEx>();
					if (typewriterEffectEx == null)
					{
						typewriterEffectEx = m_labelText.gameObject.AddComponent<TypewriterEffectEx>();
					}
					if (typewriterEffectEx.ShowFinish())
					{
						m_timer += Time.deltaTime;
					}
					if (!(m_timer >= 2f))
					{
						break;
					}
					if (m_bRunOnlyOnce)
					{
						GameBattle.m_instance.m_UIDialog.SetActive(false);
						if (GameBattle.m_instance.GameState == GameBattle.State.DialogStart)
						{
							GameBattle.m_instance.GameState = GameBattle.State.Ready;
						}
						else if (GameBattle.m_instance.GameState == GameBattle.State.DialogEnd)
						{
							GameBattle.m_instance.GameState = GameBattle.State.Win;
						}
						break;
					}
					m_timer = 0f;
					m_iValue++;
					if (GameBattle.m_instance.GameState == GameBattle.State.DialogStart)
					{
						int playerID = DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].playerID;
						if (playerID != -1)
						{
							m_gameObjects[2].SetActive(true);
							DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerID);
							m_sprite.spriteName = heroDataByIndex.iconFileName;
							m_labelText.text = heroDataByIndex.name + ":" + DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].dialog;
						}
						else
						{
							m_gameObjects[2].SetActive(false);
							m_sprite.spriteName = string.Empty;
							m_labelText.text = DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].dialog;
						}
						if (m_iValue >= DataCenter.Conf().GetCurrentGameLevelData().dialogStart.Count - 1)
						{
							m_bRunOnlyOnce = true;
						}
					}
					else if (GameBattle.m_instance.GameState == GameBattle.State.DialogEnd)
					{
						int playerID2 = DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].playerID;
						if (playerID2 != -1)
						{
							m_gameObjects[2].SetActive(true);
							DataConf.HeroData heroDataByIndex2 = DataCenter.Conf().GetHeroDataByIndex(playerID2);
							m_sprite.spriteName = heroDataByIndex2.iconFileName;
							m_labelText.text = heroDataByIndex2.name + ":" + DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].dialog;
						}
						else
						{
							m_gameObjects[2].SetActive(false);
							m_sprite.spriteName = string.Empty;
							m_labelText.text = DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].dialog;
						}
						if (m_iValue >= DataCenter.Conf().GetCurrentGameLevelData().dialogEnd.Count - 1)
						{
							m_bRunOnlyOnce = true;
						}
					}
					typewriterEffectEx.Reset();
					break;
				}
				}
				return;
			}
			switch (controlId)
			{
			case UIControlID.Label_Clip:
			{
				Player player3 = GameBattle.m_instance.GetPlayer();
				if (player3.m_weapon != null)
				{
					string text5 = ((player3.m_weapon.m_iBulletCount != -999) ? ("Clip:" + player3.m_weapon.m_iBulletCount + "/" + player3.m_weapon.attribute.clip) : "∞");
					m_labelText.text = text5;
				}
				break;
			}
			case UIControlID.Label_SkillCDTime:
			{
				Player player5 = GameBattle.m_instance.GetPlayer();
				string text6 = "Skill CD Time:" + Mathf.FloorToInt(player5.GetSkillCDTime()) + "s";
				m_labelText.text = text6;
				break;
			}
			case UIControlID.BT_Skill:
			{
				int num31 = -1;
				num31 = ((base.gameObject.name == "Skill") ? (-1) : ((!(base.gameObject.name == "Skill1")) ? ((base.gameObject.name == "Skill2") ? 1 : ((base.gameObject.name == "Skill3") ? 2 : ((!(base.gameObject.name == "Skill4")) ? (-1) : 3))) : 0));
				Player player15 = GameBattle.m_instance.GetPlayer(num31);
				if (player15 == null)
				{
					Player[] playerBetrayList5 = GameBattle.m_instance.GetPlayerBetrayList();
					if (playerBetrayList5.Length <= 0)
					{
						base.gameObject.SetActive(false);
						break;
					}
					player15 = playerBetrayList5[0];
				}
				if (num31 != -1)
				{
					if (!DataCenter.Save().m_bManualUseSkill)
					{
						base.gameObject.SetActive(false);
						break;
					}
					base.gameObject.SetActive(true);
				}
				if (m_CharacterID != player15.heroIndex)
				{
					m_CharacterID = player15.heroIndex;
					((UISprite)m_widgets[0]).spriteName = player15.skillInfo.fileName;
					if (!player15.SkillInCDTime())
					{
						((UISprite)m_widgets[1]).fillAmount = 0f;
					}
				}
				float skillCDTime = player15.GetSkillCDTime();
				if (skillCDTime > 0f || !player15.Alive() || player15.clique != 0 || Tutorial.Instance.TutorialPahseSkill == Tutorial.TutorialPhaseState.None)
				{
					if (!m_widgets[2].gameObject.activeInHierarchy)
					{
						m_widgets[2].gameObject.SetActive(true);
					}
					if (m_widgets[3].gameObject.activeInHierarchy)
					{
						m_widgets[3].gameObject.SetActive(false);
					}
					float skillTotalCDTime = player15.GetSkillTotalCDTime();
					float fillAmount = skillCDTime / skillTotalCDTime;
					((UISprite)m_widgets[1]).fillAmount = fillAmount;
					if (!player15.Alive() || player15.clique != 0 || Tutorial.Instance.TutorialPahseSkill == Tutorial.TutorialPhaseState.None)
					{
						((UISprite)m_widgets[1]).fillAmount = 1f;
					}
					break;
				}
				if (GameBattle.m_instance.SkillCommonnalityCDTime > 0f)
				{
					if (!m_widgets[2].gameObject.activeInHierarchy)
					{
						m_widgets[2].gameObject.SetActive(true);
					}
					if (m_widgets[3].gameObject.activeInHierarchy)
					{
						m_widgets[3].gameObject.SetActive(false);
					}
					float skillCommonnalityTotalCDTime = GameBattle.m_instance.SkillCommonnalityTotalCDTime;
					float fillAmount2 = GameBattle.m_instance.SkillCommonnalityCDTime / skillCommonnalityTotalCDTime;
					((UISprite)m_widgets[1]).fillAmount = fillAmount2;
					break;
				}
				if (!m_widgets[3].gameObject.activeInHierarchy)
				{
					m_widgets[3].gameObject.SetActive(true);
					TweenAlpha component3 = m_widgets[3].gameObject.GetComponent<TweenAlpha>();
					if (component3 != null)
					{
						component3.ResetToBeginning();
						component3.PlayForward();
					}
				}
				((UISprite)m_widgets[1]).fillAmount = 0f;
				break;
			}
			case UIControlID.Label_player_clip_leader:
			case UIControlID.Label_player_clip_teammate_1:
			case UIControlID.Label_player_clip_teammate_2:
			case UIControlID.Label_player_clip_teammate_3:
			case UIControlID.Label_player_clip_teammate_4:
			{
				Player player2 = null;
				player2 = (Util.s_squadMode ? GameBattle.m_instance.GetPlayerBySquadSite((int)(controlId - 11)) : ((controlId != UIControlID.Label_player_clip_leader) ? GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 12)) : GameBattle.m_instance.GetPlayer()));
				if (player2 == null)
				{
					Player[] playerBetrayList = GameBattle.m_instance.GetPlayerBetrayList();
					if (playerBetrayList.Length <= 0)
					{
						break;
					}
					player2 = playerBetrayList[0];
				}
				if (player2.m_weapon == null)
				{
					break;
				}
				if (player2.m_weapon.attribute.clip == -1)
				{
					if (!m_gameObjects[0].activeInHierarchy)
					{
						m_gameObjects[0].SetActive(true);
					}
					string text2 = "∞";
					m_labelText.text = text2;
					m_labelText.transform.localScale = new Vector3(2f, 2f, 1f);
				}
				else if (player2.m_weapon.NeedReload())
				{
					string text3 = ((player2.m_weapon.m_iBulletCount != -999) ? ("x" + player2.m_weapon.m_iBulletCount) : "x∞");
					m_labelText.text = text3;
					m_gameObjects[0].SetActive(false);
					m_sprite.fillAmount = player2.m_weapon.GetReloadProgress();
					m_labelText.transform.localScale = Vector3.one;
				}
				else
				{
					if (!m_gameObjects[0].activeInHierarchy)
					{
						m_gameObjects[0].SetActive(true);
					}
					string text4 = ((player2.m_weapon.m_iBulletCount != -999) ? ("x" + player2.m_weapon.m_iBulletCount) : "x∞");
					m_labelText.text = text4;
					m_labelText.transform.localScale = Vector3.one;
				}
				break;
			}
			case UIControlID.PVP_Label_Target_clip_leader:
			case UIControlID.PVP_Label_Target_clip_teammate_1:
			case UIControlID.PVP_Label_Target_clip_teammate_2:
			case UIControlID.PVP_Label_Target_clip_teammate_3:
			case UIControlID.PVP_Label_Target_clip_teammate_4:
			{
				Player player16 = null;
				player16 = (Util.s_squadMode ? GameBattle.m_instance.GetPlayerBySquadSite((int)(controlId - 311)) : ((controlId != UIControlID.PVP_Label_Target_clip_leader) ? GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 312)) : GameBattle.m_instance.GetPlayer()));
				if (player16 == null || player16.m_weapon == null)
				{
					break;
				}
				if (player16.m_weapon.attribute.clip == -1)
				{
					string text7 = "∞";
					m_labelText.color = Color.green;
					m_labelText.text = text7;
					base.transform.localScale = new Vector3(2f, 3f, 1f);
				}
				else if (player16.m_weapon.m_iBulletCount == 0)
				{
					if (m_bVisible)
					{
						m_timer += Time.deltaTime;
						if (m_timer >= 0.25f)
						{
							m_timer = 0f;
							m_bVisible = false;
							m_labelText.text = string.Empty;
						}
					}
					else
					{
						m_timer += Time.deltaTime;
						if (m_timer >= 0.25f)
						{
							m_timer = 0f;
							m_bVisible = true;
							string text8 = "reload";
							m_labelText.color = Color.red;
							m_labelText.text = text8;
						}
					}
					base.transform.localScale = Vector3.one;
				}
				else
				{
					string text9 = ((player16.m_weapon.m_iBulletCount != -999) ? ("x" + player16.m_weapon.m_iBulletCount) : "x∞");
					m_labelText.color = Color.green;
					m_labelText.text = text9;
					base.transform.localScale = Vector3.one;
				}
				break;
			}
			case UIControlID.Label_player_icon_leader:
			case UIControlID.Label_player_icon_teammate_1:
			case UIControlID.Label_player_icon_teammate_2:
			case UIControlID.Label_player_icon_teammate_3:
			case UIControlID.Label_player_icon_teammate_4:
			{
				Player player12 = null;
				if (Util.s_squadMode)
				{
					int siteNum3 = (int)(controlId - 1);
					player12 = GameBattle.m_instance.GetPlayerBySquadSite(siteNum3);
				}
				else if (controlId == UIControlID.Label_player_icon_leader)
				{
					player12 = GameBattle.m_instance.GetPlayer();
				}
				else
				{
					int index = (int)(controlId - 2);
					player12 = GameBattle.m_instance.GetTeammateByIndex(index);
				}
				if (player12 == null)
				{
					Player[] playerBetrayList4 = GameBattle.m_instance.GetPlayerBetrayList();
					if (playerBetrayList4.Length <= 0)
					{
						break;
					}
					player12 = playerBetrayList4[0];
				}
				if (m_CharacterID != player12.heroIndex)
				{
					m_CharacterID = player12.heroIndex;
					m_sprite.spriteName = player12.iconName;
				}
				break;
			}
			case UIControlID.PVP_Label_Target_icon_leader:
			case UIControlID.PVP_Label_Target_icon_teammate_1:
			case UIControlID.PVP_Label_Target_icon_teammate_2:
			case UIControlID.PVP_Label_Target_icon_teammate_3:
			case UIControlID.PVP_Label_Target_icon_teammate_4:
			{
				Player player14 = GameBattle.m_instance.GetComputerObjByIndex((int)(controlId - 301)) as Player;
				if (player14 != null && m_CharacterID != player14.heroIndex)
				{
					m_CharacterID = player14.heroIndex;
					m_sprite.spriteName = player14.iconName;
				}
				break;
			}
			case UIControlID.Label_player_hp_bar_leader:
			case UIControlID.Label_player_hp_bar_teammate_1:
			case UIControlID.Label_player_hp_bar_teammate_2:
			case UIControlID.Label_player_hp_bar_teammate_3:
			case UIControlID.Label_player_hp_bar_teammate_4:
			{
				Player player10 = null;
				if (!Util.s_squadMode)
				{
					player10 = ((controlId != UIControlID.Label_player_hp_bar_leader) ? GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 7)) : GameBattle.m_instance.GetPlayer());
				}
				else
				{
					int siteNum2 = (int)(controlId - 6);
					player10 = GameBattle.m_instance.GetPlayerBySquadSite(siteNum2);
				}
				if (player10 == null)
				{
					Player[] playerBetrayList3 = GameBattle.m_instance.GetPlayerBetrayList();
					if (playerBetrayList3.Length <= 0)
					{
						break;
					}
					player10 = playerBetrayList3[0];
				}
				if ((float)player10.hp < (float)player10.hpMax * 0.3f)
				{
					if (m_slider.foregroundWidget != m_widgets[1])
					{
						m_gameObjects[1].SetActive(true);
						m_slider.foregroundWidget = m_widgets[1];
						m_gameObjects[0].SetActive(false);
					}
				}
				else if (m_slider.foregroundWidget != m_widgets[0])
				{
					m_gameObjects[0].SetActive(true);
					m_slider.foregroundWidget = m_widgets[0];
					m_gameObjects[1].SetActive(false);
				}
				float num23 = player10.hp;
				if (num23 > 0f)
				{
					float num24 = player10.hpMax;
					m_slider.value = num23 / num24;
				}
				else
				{
					m_slider.value = 0f;
				}
				break;
			}
			case UIControlID.Label_Player_HpBar_UpHead_Leader:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_1:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_2:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_3:
			case UIControlID.Label_Player_HpBar_UpHead_Teammate_4:
			{
				Player player6 = null;
				if (!Util.s_squadMode)
				{
					player6 = ((controlId != UIControlID.Label_Player_HpBar_UpHead_Leader) ? GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 407)) : GameBattle.m_instance.GetPlayer());
				}
				else
				{
					int siteNum = (int)(controlId - 406);
					player6 = GameBattle.m_instance.GetPlayerBySquadSite(siteNum);
				}
				if (player6 == null && player6 == null)
				{
					Player[] playerBetrayList2 = GameBattle.m_instance.GetPlayerBetrayList();
					if (playerBetrayList2.Length <= 0)
					{
						base.gameObject.SetActive(false);
						break;
					}
					player6 = playerBetrayList2[0];
				}
				float num13 = player6.hp;
				if ((float)player6.hp < (float)player6.hpMax * 0.3f)
				{
					if (m_slider.foregroundWidget != m_widgets[1])
					{
						m_gameObjects[1].SetActive(true);
						m_slider.foregroundWidget = m_widgets[1];
						m_gameObjects[0].SetActive(false);
					}
				}
				else if (m_slider.foregroundWidget != m_widgets[0])
				{
					m_gameObjects[0].SetActive(true);
					m_slider.foregroundWidget = m_widgets[0];
					m_gameObjects[1].SetActive(false);
				}
				if (num13 > 0f)
				{
					float num14 = player6.hpMax;
					m_slider.value = num13 / num14;
				}
				else
				{
					m_slider.value = 0f;
				}
				float num15 = ((!player6.Visible) ? 999f : 2.5f);
				Vector3 position3 = new Vector3(player6.GetTransform().position.x, player6.GetTransform().position.y + num15, player6.GetTransform().position.z);
				Vector3 p = GameBattle.m_instance.GetCamera().WorldToScreenPoint(position3);
				p = Util.ScreenPointToNGUI(p);
				base.transform.localPosition = p;
				break;
			}
			case UIControlID.PVP_Label_Target_hp_bar_leader:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_1:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_2:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_3:
			case UIControlID.PVP_Label_Target_hp_bar_teammate_4:
			{
				Player player8 = GameBattle.m_instance.GetComputerObjByIndex((int)(controlId - 306)) as Player;
				if (player8 != null)
				{
					float num18 = player8.hp;
					if (num18 > 0f)
					{
						float num19 = player8.hpMax;
						m_slider.value = num18 / num19;
					}
					else
					{
						m_slider.value = 0f;
					}
					if (DataCenter.State().isPVPMode)
					{
						float num20 = ((!player8.Visible) ? 999f : 2.5f);
						Vector3 position4 = new Vector3(player8.GetTransform().position.x, player8.GetTransform().position.y + num20, player8.GetTransform().position.z);
						Vector3 vector3 = GameBattle.m_instance.GetCamera().WorldToScreenPoint(position4);
						vector3 = new Vector3(vector3.x - (float)(Screen.width / 2), vector3.y - (float)(Screen.height / 2), vector3.z);
						base.transform.localPosition = vector3;
					}
				}
				break;
			}
			case UIControlID.Panel_Target:
				if (m_target != null && !m_target.Alive())
				{
					m_timer += Time.deltaTime;
					if (m_timer >= 2f)
					{
						base.gameObject.SetActive(false);
						m_target = null;
					}
				}
				break;
			case UIControlID.Label_target_hp_bar:
				updateBossHpBar();
				break;
			case UIControlID.Target_hp_bar_normal:
			{
				Player player7 = GameBattle.m_instance.GetPlayer();
				m_target = player7.lockedTarget;
				if (m_target == null)
				{
					break;
				}
				float num17 = (float)m_target.hp / (float)m_target.hpMax;
				if (m_slider.value > num17)
				{
					if (m_slider.value - num17 > m_fDeltaValue)
					{
						m_fDeltaValue = m_slider.value - num17;
					}
					m_slider.value -= m_fDeltaValue / 0.2f * Time.deltaTime;
				}
				else if (m_slider.value != num17)
				{
					m_slider.value = num17;
					m_fDeltaValue = 0f;
				}
				break;
			}
			case UIControlID.Label_target_name:
			{
				Player player4 = GameBattle.m_instance.GetPlayer();
				m_target = player4.lockedTarget;
				if (m_target != null)
				{
					m_labelText.text = m_target.name;
				}
				break;
			}
			case UIControlID.Label_target_hp_groove_num:
				if (GameBattle.m_instance.UIVisableTarget != null)
				{
					m_target = GameBattle.m_instance.UIVisableTarget;
					m_iValue = m_target.hp / HP_PER_GROOVE;
					if (m_iValue > 0)
					{
						m_labelText.text = "X" + m_iValue;
					}
					else
					{
						m_labelText.text = string.Empty;
					}
				}
				break;
			case UIControlID.Panel_TargetIndicates:
			{
				DS2Object cameraFocus = GameBattle.m_instance.CameraFocus;
				Vector3 position = cameraFocus.GetTransform().position;
				DS2ActiveObject[] targetList = GameBattle.m_instance.GetTargetList(DS2ActiveObject.Clique.Player);
				int num7 = targetList.Length;
				if (num7 == 0)
				{
					for (int j = 0; j <= m_iValue; j++)
					{
						m_gameObjects[m_iValue].SetActive(false);
					}
					break;
				}
				if (num7 > m_gameObjects.Length)
				{
					num7 = m_gameObjects.Length;
				}
				Camera camera = GameBattle.m_instance.GetCamera();
				Vector3 vector = camera.WorldToScreenPoint(position);
				Rect rect = new Rect(20f, 20f, Screen.width - 40, Screen.height - 40);
				float num8 = Screen.width >> 1;
				float num9 = Screen.height >> 1;
				if (m_iValue >= num7)
				{
					m_iValue = num7 - 1;
				}
				DS2ActiveObject dS2ActiveObject = targetList[m_iValue];
				Vector3 position2 = dS2ActiveObject.GetTransform().position;
				Vector3 point = camera.WorldToScreenPoint(position2);
				if (rect.Contains(point))
				{
					m_gameObjects[m_iValue].SetActive(false);
					m_iValue++;
				}
				else
				{
					float num10 = Mathf.Atan2(position2.z - position.z, position2.x - position.x);
					Vector2 vector2 = new Vector2(num8 + m_fValueMax * Mathf.Cos(num10), num9 + m_fValueMax * Mathf.Sin(num10));
					vector2.x = Mathf.Clamp(vector2.x, 20f, Screen.width - 20);
					vector2.y = Mathf.Clamp(vector2.y, 20f, Screen.height - 20);
					float num11 = num10 * 57.29578f;
					if (num11 < 0f)
					{
						num11 += 360f;
					}
					m_gameObjects[m_iValue].transform.localPosition = Util.ScreenPointToNGUI(vector2);
					m_gameObjects[m_iValue].transform.localRotation = Quaternion.Euler(0f, 0f, num11);
					m_gameObjects[m_iValue].SetActive(true);
					m_iValue++;
				}
				if (m_iValue >= num7)
				{
					for (int k = m_iValue; k < m_iValueMax; k++)
					{
						m_gameObjects[k].SetActive(false);
					}
					m_iValueMax = m_iValue;
					m_iValue = 0;
				}
				break;
			}
			case UIControlID.Panel_ProgressIndicates:
			{
				DS2Object cameraFocus2 = GameBattle.m_instance.CameraFocus;
				Vector3 position6 = cameraFocus2.GetTransform().position;
				Camera camera2 = GameBattle.m_instance.GetCamera();
				Vector3 vector4 = camera2.WorldToScreenPoint(position6);
				Rect rect2 = new Rect(20f, 20f, Screen.width - 20, Screen.height - 20);
				float num32 = Screen.width >> 1;
				float num33 = Screen.height >> 1;
				Vector3 currentMainPointPosition = MinimapNGUI.instance.GetCurrentMainPointPosition();
				Vector3 point2 = camera2.WorldToScreenPoint(currentMainPointPosition);
				if (rect2.Contains(point2) || GameBattle.m_instance.bMainPointInProcess)
				{
					m_gameObjects[0].SetActive(false);
					break;
				}
				float num34 = Mathf.Atan2(currentMainPointPosition.z - position6.z, currentMainPointPosition.x - position6.x);
				Vector2 vector5 = new Vector2(num32 + m_fValueMax * Mathf.Cos(num34), num33 + m_fValueMax * Mathf.Sin(num34));
				vector5.x = Mathf.Clamp(vector5.x, 20f, Screen.width - 20);
				vector5.y = Mathf.Clamp(vector5.y, 20f, Screen.height - 20);
				float num35 = num34 * 57.29578f;
				if (num35 < 0f)
				{
					num35 += 360f;
				}
				m_gameObjects[0].transform.localPosition = Util.ScreenPointToNGUI(vector5);
				m_gameObjects[0].transform.localRotation = Quaternion.Euler(0f, 0f, num35);
				m_gameObjects[0].SetActive(true);
				break;
			}
			case UIControlID.Rank_Frame_Leader:
			{
				Player player9 = GameBattle.m_instance.GetPlayer();
				if (m_CharacterID == player9.heroIndex)
				{
					break;
				}
				m_CharacterID = player9.heroIndex;
				for (int num22 = 0; num22 < m_gameObjects.Length; num22++)
				{
					if (player9.rank == (Defined.RANK_TYPE)num22)
					{
						m_gameObjects[num22].SetActive(true);
					}
					else
					{
						m_gameObjects[num22].SetActive(false);
					}
				}
				break;
			}
			case UIControlID.Rank_Frame_Teammate1:
			case UIControlID.Rank_Frame_Teammate2:
			case UIControlID.Rank_Frame_Teammate3:
			case UIControlID.Rank_Frame_Teammate4:
			{
				Player teammateByIndex = GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 28));
				if (teammateByIndex == null || m_CharacterID == teammateByIndex.heroIndex)
				{
					break;
				}
				m_CharacterID = teammateByIndex.heroIndex;
				for (int num21 = 0; num21 < m_gameObjects.Length; num21++)
				{
					if (teammateByIndex.rank == (Defined.RANK_TYPE)num21)
					{
						m_gameObjects[num21].SetActive(true);
					}
					else
					{
						m_gameObjects[num21].SetActive(false);
					}
				}
				break;
			}
			case UIControlID.PVP_Rank_Frame_Target_Leader:
			case UIControlID.PVP_Rank_Frame_Target_Teammate1:
			case UIControlID.PVP_Rank_Frame_Target_Teammate2:
			case UIControlID.PVP_Rank_Frame_Target_Teammate3:
			case UIControlID.PVP_Rank_Frame_Target_Teammate4:
			{
				Player player = GameBattle.m_instance.GetComputerObjByIndex((int)(controlId - 321)) as Player;
				if (player == null || m_CharacterID == player.heroIndex)
				{
					break;
				}
				m_CharacterID = player.heroIndex;
				for (int l = 0; l < m_gameObjects.Length; l++)
				{
					if (player.rank == (Defined.RANK_TYPE)l)
					{
						m_gameObjects[l].SetActive(true);
					}
					else
					{
						m_gameObjects[l].SetActive(false);
					}
				}
				break;
			}
			case UIControlID.PVP_MyTeamHpBar:
			{
				m_fValue = 0f;
				DS2ActiveObject[] playerList2 = GameBattle.m_instance.GetPlayerList();
				if (m_fValueMax == 0f)
				{
					for (int num28 = 0; num28 < playerList2.Length; num28++)
					{
						m_fValueMax += playerList2[num28].hpMax;
					}
				}
				for (int num29 = 0; num29 < playerList2.Length; num29++)
				{
					m_fValue += playerList2[num29].hp;
				}
				float num30 = m_fValue / m_fValueMax;
				if (m_slider.value > num30)
				{
					m_slider.value -= (m_slider.value - num30) / 0.5f * Time.deltaTime;
					if (m_slider.value <= num30)
					{
						m_slider.value = num30;
					}
				}
				else
				{
					m_slider.value = num30;
				}
				break;
			}
			case UIControlID.PVP_TargetTeamHpBar:
			{
				m_fValue = 0f;
				DS2ActiveObject[] enemyList = GameBattle.m_instance.GetEnemyList();
				if (m_fValueMax == 0f)
				{
					for (int m = 0; m < enemyList.Length; m++)
					{
						m_fValueMax += enemyList[m].hpMax;
					}
				}
				for (int n = 0; n < enemyList.Length; n++)
				{
					m_fValue += enemyList[n].hp;
				}
				float num16 = m_fValue / m_fValueMax;
				if (m_slider.value > num16)
				{
					m_slider.value -= (m_slider.value - num16) / 0.5f * Time.deltaTime;
					if (m_slider.value <= num16)
					{
						m_slider.value = num16;
					}
				}
				else
				{
					m_slider.value = num16;
				}
				break;
			}
			case UIControlID.HurtFlash_Frame_Leader:
			case UIControlID.HurtFlash_Frame_Teammate1:
			case UIControlID.HurtFlash_Frame_Teammate2:
			case UIControlID.HurtFlash_Frame_Teammate3:
			case UIControlID.HurtFlash_Frame_Teammate4:
			{
				Player player13 = null;
				player13 = (Util.s_squadMode ? GameBattle.m_instance.GetPlayerBySquadSite((int)(controlId - 400)) : ((controlId != UIControlID.HurtFlash_Frame_Leader) ? GameBattle.m_instance.GetTeammateByIndex((int)(controlId - 401)) : GameBattle.m_instance.GetPlayer()));
				if (player13 == null || !(player13.UIHurtFlashTimer > 0f))
				{
					break;
				}
				if (m_bVisible)
				{
					float num26 = m_sprite.color.a + 0.1f;
					if (num26 >= 1f)
					{
						num26 = 1f;
						m_bVisible = false;
					}
					m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, num26);
				}
				else
				{
					float num27 = m_sprite.color.a - 0.1f;
					if (num27 <= 0f)
					{
						num27 = 0f;
						m_bVisible = true;
					}
					m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, num27);
				}
				player13.UIHurtFlashTimer -= Time.deltaTime;
				if (player13.UIHurtFlashTimer <= 0f)
				{
					player13.UIHurtFlashTimer = 0f;
					m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
				}
				break;
			}
			case UIControlID.Panel_Reload_UpHead:
			{
				DS2ActiveObject[] playerList = GameBattle.m_instance.GetPlayerList();
				for (int num25 = 0; num25 < playerList.Length; num25++)
				{
					if (playerList[num25].objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_PLAYER || playerList[num25].objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ALLY)
					{
						Player player11 = (Player)playerList[num25];
						if (player11.m_weapon.NeedReload() && player11.Alive())
						{
							Vector3 position5 = new Vector3(player11.m_effectPointUpHead.position.x, player11.m_effectPointUpHead.position.y + 1f, player11.m_effectPointUpHead.position.z);
							Vector3 p2 = GameBattle.m_instance.GetCamera().WorldToScreenPoint(position5);
							p2 = Util.ScreenPointToNGUI(p2);
							m_gameObjects[num25].transform.localPosition = p2;
							m_gameObjects[num25].SetActive(true);
						}
						else if (m_gameObjects[num25].activeInHierarchy)
						{
							m_gameObjects[num25].SetActive(false);
						}
					}
				}
				break;
			}
			case UIControlID.ZombieIcon:
				if (GameBattle.m_instance.UIVisableTarget == null)
				{
					break;
				}
				m_target = GameBattle.m_instance.UIVisableTarget;
				if (m_target != null && m_target.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY && m_CharacterID != m_target.id)
				{
					Enemy enemy3 = (Enemy)m_target;
					if (enemy3.isBoss || enemy3.eliteType != 0)
					{
						DataConf.EnemyData enemyDataByType = DataCenter.Conf().GetEnemyDataByType(enemy3.enemyType);
						m_sprite.spriteName = enemyDataByType.iconFileName;
					}
				}
				break;
			case UIControlID.Label_Elite_Attibute:
			{
				if (GameBattle.m_instance.UIVisableTarget == null)
				{
					break;
				}
				m_target = GameBattle.m_instance.UIVisableTarget;
				if (m_target.objectType == Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
				{
					Enemy enemy = (Enemy)m_target;
				}
				if (m_target.id == m_CharacterID)
				{
					break;
				}
				m_CharacterID = m_target.id;
				if (m_target.objectType != Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
				{
					break;
				}
				Enemy enemy2 = (Enemy)m_target;
				m_labelText.text = string.Empty;
				int num12 = 0;
				{
					foreach (SpecialAttribute.AttributeType item in enemy2.specialAttribute)
					{
						SpecialAttribute.SpecialAttributeData specialAttributeData = DataCenter.Conf().GetSpecialAttributeData(item);
						m_labelText.text += specialAttributeData.name;
						if (num12 < enemy2.specialAttribute.Count - 1)
						{
							m_labelText.text += ";";
						}
						num12++;
					}
					break;
				}
			}
			case UIControlID.BT_SwitchToRPG:
				if (GameBattle.m_instance.GetPlayerObjCount() <= 1 || !DataCenter.Save().squadMode)
				{
					base.gameObject.SetActive(false);
				}
				break;
			case UIControlID.BT_SwitchToSquad:
				if (GameBattle.m_instance.GetPlayerObjCount() <= 1 || DataCenter.Save().squadMode)
				{
					base.gameObject.SetActive(false);
				}
				break;
			}
		}

		public void OnEnable()
		{
		}

		public void OnDisable()
		{
		}

		public void SetTarget(DS2ActiveObject target)
		{
			m_timer = 0f;
			if (target.objectType != Defined.OBJECT_TYPE.OBJECT_TYPE_ENEMY)
			{
				return;
			}
			Enemy enemy = (Enemy)target;
			if (enemy.isBoss || enemy.eliteType != 0)
			{
				m_gameObjects[0].SetActive(false);
				m_gameObjects[1].SetActive(true);
				m_labelText.text = enemy.enemyName;
			}
			else
			{
				if (m_target != null)
				{
					Enemy enemy2 = (Enemy)m_target;
					if (enemy2.isBoss || enemy2.eliteType != 0)
					{
						m_gameObjects[0].SetActive(false);
						m_gameObjects[1].SetActive(false);
						return;
					}
				}
				m_gameObjects[0].SetActive(false);
				m_gameObjects[1].SetActive(false);
			}
			m_target = target;
			if (m_target.id != m_CharacterID)
			{
				m_CharacterID = m_target.id;
				if (m_target.hpMax < 1000)
				{
					HP_PER_GROOVE = 300;
				}
				else if (m_target.hpMax <= 30000)
				{
					HP_PER_GROOVE = 1000;
				}
				else if (m_target.hpMax > 30000 && m_target.hpMax <= 300000)
				{
					HP_PER_GROOVE = 10000;
				}
				else if (m_target.hpMax > 300000 && m_target.hpMax <= 3000000)
				{
					HP_PER_GROOVE = 100000;
				}
				else
				{
					HP_PER_GROOVE = 1000000;
				}
			}
		}

		public void EnableScreenGameWin()
		{
			base.gameObject.SetActive(true);
			m_gameObjects[0].SetActive(true);
			m_gameObjects[1].SetActive(false);
			GameObject control = UIControlManager.Instance.GetControl(121);
			control.SetActive(false);
			GameObject control2 = UIControlManager.Instance.GetControl(116);
			control2.SetActive(false);
			for (int i = 2; i < 5; i++)
			{
				if (i - 2 < DataCenter.State().battleStars)
				{
					m_gameObjects[i].SetActive(true);
				}
				else
				{
					m_gameObjects[i].SetActive(false);
				}
			}
		}

		private void updateBossHpBar()
		{
			if (GameBattle.m_instance.UIVisableTarget == null)
			{
				return;
			}
			m_target = GameBattle.m_instance.UIVisableTarget;
			m_iValueMax = m_target.hp / HP_PER_GROOVE;
			int num = m_target.hp % HP_PER_GROOVE;
			if (m_target.id != m_CharacterID)
			{
				m_CharacterID = m_target.id;
				m_iValue = m_iValueMax;
				m_sprite.fillAmount = (float)num / (float)HP_PER_GROOVE;
			}
			else if (m_iLastValue != m_target.hp)
			{
				if (m_iValue > m_iValueMax)
				{
					m_iValue = m_iValueMax;
					m_sprite.fillAmount = 1f;
					m_iLastValue = m_target.hp;
				}
				else if (m_target.hp != 0)
				{
					m_sprite.fillAmount = (float)(m_target.hpLast % HP_PER_GROOVE) / (float)HP_PER_GROOVE;
				}
				m_iLastValue = m_target.hp;
			}
			int num2 = 0;
			int num3 = 0;
			if (m_iValueMax >= m_widgets.Length)
			{
				num2 = m_iValueMax % (m_widgets.Length - 1) + 1;
				num3 = num2 - 1;
				if (num3 <= 0)
				{
					num3 = m_widgets.Length - 1;
				}
			}
			else
			{
				num2 = m_iValueMax;
				num3 = num2 - 1;
				if (num3 <= 0)
				{
					num3 = 0;
				}
			}
			for (int i = 0; i < m_widgets.Length; i++)
			{
				if (i == num2 || i == num3)
				{
					m_widgets[i].gameObject.SetActive(true);
					m_widgets[i].depth = 2;
					TweenColor component = m_sprite.gameObject.GetComponent<TweenColor>();
					component.to = new Color(m_widgets[i].color.r, m_widgets[i].color.g, m_widgets[i].color.b, 255f);
					if (i == num3 && num3 != num2)
					{
						((UISprite)m_widgets[i]).fillAmount = 1f;
						m_widgets[i].depth = 0;
					}
				}
				else
				{
					m_widgets[i].gameObject.SetActive(false);
				}
			}
			float num4 = (float)num / (float)HP_PER_GROOVE;
			int num5 = num2;
			if (((UISprite)m_widgets[num5]).fillAmount > num4)
			{
				((UISprite)m_widgets[num5]).fillAmount = num4;
			}
			else
			{
				((UISprite)m_widgets[num5]).fillAmount = num4;
			}
			float num6 = 0.5f;
			if (m_iValue > m_iValueMax)
			{
				m_sprite.fillAmount -= num6 * Time.deltaTime;
				if (m_sprite.fillAmount <= 0f)
				{
					m_sprite.fillAmount = 1f;
					m_iValue--;
				}
			}
			else
			{
				if (!(m_sprite.fillAmount > ((UISprite)m_widgets[num5]).fillAmount))
				{
					return;
				}
				m_sprite.fillAmount -= num6 * Time.deltaTime;
				if (m_sprite.fillAmount < ((UISprite)m_widgets[num5]).fillAmount)
				{
					m_sprite.fillAmount = ((UISprite)m_widgets[num5]).fillAmount;
					TweenColor component2 = m_sprite.gameObject.GetComponent<TweenColor>();
					component2.enabled = false;
					return;
				}
				TweenColor component3 = m_sprite.gameObject.GetComponent<TweenColor>();
				if (!component3.enabled)
				{
					component3.enabled = true;
				}
			}
		}

		private void Quit()
		{
			Time.timeScale = 1f;
			GameBattle.s_enemyCount = 0;
			Save.Write();
			BackgroundMusicManager.Instance().PlayBackgroundMusic(BackgroundMusicManager.MusicType.UI_BG);
			bool wasNewUser = DataCenter.Save().bNewUser;
			if (DataCenter.Save().bNewUser)
			{
				DataCenter.Save().tutorialStep = Defined.TutorialStep.TutorialBattle;
				DataCenter.Save().BattleTutorialFinished = true;
				DataCenter.Save().bNewUser = false;
				DataCenter.Save().MigrateTutorialDummyTeamSites();
				Save.Write();
			}
			if (DataCenter.State().isEncounterLevel)
			{
				DataCenter.State().isPVPMode = false;
			}
			if (DataCenter.State().isPVPMode)
			{
				SceneLoadingManager.SwitchScene("UIArena");
			}
			else if (GameBattle.m_instance.GameState == GameBattle.State.Win)
			{
				if (wasNewUser)
				{
					SceneLoadingManager.SwitchScene("UICaricature");
				}
				else
				{
					SceneLoadingManager.SwitchScene("UIBase");
				}
			}
			else if (DataCenter.Save().bNewUser)
			{
				SceneLoadingManager.SwitchScene("UICheckUpdate");
			}
			else
			{
				SceneLoadingManager.SwitchScene("UIBase");
			}
			TAudioManager.instance.AudioListener.transform.parent = TAudioManager.instance.transform;
			TAudioManager.instance.AudioListener.transform.localPosition = Vector3.zero;
			TAudioManager.instance.AudioListener.transform.localScale = Vector3.one;
			UIUtil.HideOpenClik();
			GC.Collect();
		}

		public void SetCameraView()
		{
			if (m_gameObjects == null || !UIToggle.current.value)
			{
				return;
			}
			bool flag = false;
			switch (UIToggle.current.name)
			{
			case "Far":
				if (DataCenter.Save().CameraView != 0)
				{
					DataCenter.Save().CameraView = Defined.CameraView.Far;
					flag = true;
				}
				break;
			case "Default":
				if (DataCenter.Save().CameraView != Defined.CameraView.Default)
				{
					DataCenter.Save().CameraView = Defined.CameraView.Default;
					flag = true;
				}
				break;
			case "Close":
				if (DataCenter.Save().CameraView != Defined.CameraView.Close)
				{
					DataCenter.Save().CameraView = Defined.CameraView.Close;
					flag = true;
				}
				break;
			}
			if (flag)
			{
				GameBattle.m_instance.SetCameraView(DataCenter.Save().CameraView);
				Vector3 targetPos = new Vector3(GameBattle.m_instance.CameraFocus.GetTransform().position.x, GameBattle.m_instance.CameraFocus.GetTransform().position.y + GameBattle.m_instance.CameraViewDisY, GameBattle.m_instance.CameraFocus.GetTransform().position.z - GameBattle.m_instance.CameraViewDisZ);
				GameBattle.m_instance.SetCameraMove(targetPos, 15f, GameBattle.m_instance.CameraFocus, true);
				Save.Write();
			}
		}

		public void SetManualSkill()
		{
			int playerObjCount = GameBattle.m_instance.GetPlayerObjCount();
			for (int i = 1; i < m_gameObjects.Length; i++)
			{
				if (i < playerObjCount)
				{
					m_gameObjects[i].SetActive(DataCenter.Save().m_bManualUseSkill);
				}
				else
				{
					m_gameObjects[i].SetActive(false);
				}
			}
		}

		public void DialogInit()
		{
			m_bRunOnlyOnce = false;
			m_sprite = m_gameObjects[0].GetComponent<UISprite>();
			m_labelText = m_gameObjects[1].GetComponent<UILabel>();
			m_iValue = 0;
			if (GameBattle.m_instance.GameState == GameBattle.State.DialogStart)
			{
				int playerID = DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].playerID;
				if (playerID != -1)
				{
					DataConf.HeroData heroDataByIndex = DataCenter.Conf().GetHeroDataByIndex(playerID);
					m_sprite.spriteName = heroDataByIndex.iconFileName;
					m_labelText.text = heroDataByIndex.name + ":" + DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].dialog;
				}
				else
				{
					m_labelText.text = DataCenter.Conf().GetCurrentGameLevelData().dialogStart[m_iValue].dialog;
				}
			}
			else if (GameBattle.m_instance.GameState == GameBattle.State.DialogEnd)
			{
				int playerID2 = DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].playerID;
				if (playerID2 != -1)
				{
					DataConf.HeroData heroDataByIndex2 = DataCenter.Conf().GetHeroDataByIndex(playerID2);
					m_sprite.spriteName = heroDataByIndex2.iconFileName;
					m_labelText.text = heroDataByIndex2.name + ":" + DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].dialog;
				}
				else
				{
					m_labelText.text = DataCenter.Conf().GetCurrentGameLevelData().dialogEnd[m_iValue].dialog;
				}
			}
			TypewriterEffectEx typewriterEffectEx = m_labelText.gameObject.GetComponent<TypewriterEffectEx>();
			if (typewriterEffectEx == null)
			{
				typewriterEffectEx = m_labelText.gameObject.AddComponent<TypewriterEffectEx>();
			}
			typewriterEffectEx.Reset();
		}
	}
}
