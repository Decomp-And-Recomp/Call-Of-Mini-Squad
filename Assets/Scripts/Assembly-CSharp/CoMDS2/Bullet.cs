using UnityEngine;

namespace CoMDS2
{
	public class Bullet : DS2Object, ICollider
	{
		public enum BULLET_TYPE
		{
			FAKE = 0,
			RIFLE_FIRELINE = 1,
			SNIPER = 2,
			GRENADE = 3,
			EMPLACEMENT_LAST_FIRE = 4,
			NINJIA = 5,
			LASER_01 = 6,
			LASER_02 = 7,
			LASER_03 = 8,
			LASER_04 = 9,
			GRENADE_04 = 10,
			NINJIA_01 = 11,
			NINJIA_02 = 12,
			NINJIA_03 = 13,
			NINJIA_04 = 14,
			NINJIA_ICE_01 = 15,
			NINJIA_ICE_02 = 16,
			NINJIA_ICE_03 = 17,
			NINJIA_ICE_04 = 18,
			PISTOL_04_BULLET = 19,
			Virus_01_Bullet = 20,
			Virus_02_Bullet = 21,
			Virus_03_Bullet = 22,
			Virus_04_Bullet = 23,
			ZOMBIE_NURSE_VENOM = 24,
			FIRE_BALL = 25,
			ICE_BALL = 26,
			PISTOL_COLLIDE = 27,
			Putridifer_Attack = 28,
			DeadLight_Attack = 29,
			SporeGreen = 30,
			SporePurple = 31,
			Pyro_Attack = 32,
			pest_Attack_Bullet = 33,
			Doctor_Attack_Bullet = 34,
			Missile = 35,
			BlackholeBullet = 36,
			DeadLight_Attack_Purple = 37,
			Casting = 38,
			COUNT = 39
		}

		public enum BULLET_DAMAGE_TYPE
		{
			SINGLE = 0,
			MULTI = 1,
			FROZEN = 2
		}

		public class BulletAttribute
		{
			public float speed;

			public float life;

			public Defined.EFFECT_TYPE effectHit;

			public BULLET_DAMAGE_TYPE damageType;

			public float damageRange;

			public bool isPenetrate;

			public float damageDecayRange;

			public float damageDecayPercent;
		}

		protected DS2ActiveObject m_creator;

		public BulletAttribute attribute;

		private EffectParticleContinuous m_effectParticle;

		public HitInfo hitInfo { get; set; }

		public BULLET_TYPE bulletType { get; set; }

		public Bullet()
		{
		}

		public Bullet(DS2Object creator)
		{
		}

		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			m_effectParticle = GetGameObject().GetComponentInChildren<EffectParticleContinuous>();
			hitInfo = new HitInfo();
			base.objectType = Defined.OBJECT_TYPE.OBJECT_TYPE_BULLET;
		}

		public virtual void SetBullet(DS2ActiveObject creator, BulletAttribute attribute, Vector3 position, Quaternion rotation)
		{
			GetGameObject().transform.position = position;
			GetGameObject().transform.rotation = rotation;
			m_creator = creator;
			if (attribute != null)
			{
				this.attribute = attribute;
			}
		}

		public virtual void SetBullet(DS2ActiveObject creator, BulletAttribute attribute, Vector3 position, Vector3 direction)
		{
			GetGameObject().transform.position = position;
			if (direction.sqrMagnitude > 0.0001f)
			{
				GetGameObject().transform.forward = direction;
			}
			m_creator = creator;
			if (attribute != null)
			{
				this.attribute = attribute;
			}
		}

		public virtual void SetBullet(DS2ActiveObject creator, Vector3 position, Quaternion rotation)
		{
			GetGameObject().transform.position = position;
			GetGameObject().transform.rotation = rotation;
			m_creator = creator;
		}

		public virtual void OnCollide(ICollider collider)
		{
		}

		public virtual void TriggerEnter(DS2Object obj)
		{
			IFighter fighter = obj.GetFighter();
			if (fighter == null)
			{
				return;
			}
			DS2ActiveObject dS2ActiveObject = (DS2ActiveObject)obj;
			if (m_creator.clique == dS2ActiveObject.clique || !dS2ActiveObject.Alive())
			{
				return;
			}
			HitInfo hitInfo = this.hitInfo;
			hitInfo.hitPoint = GetTransform().position;
			hitInfo.repelDirection = GetModelTransform().forward;
			hitInfo.source = GetCreator();
			if (fighter.OnHit(hitInfo).isHit)
			{
				ICollider collider = obj.GetCollider();
				if (collider != null)
				{
					collider.OnCollide(this);
					HitEffect();
				}
			}
		}

		public virtual void HitEffect()
		{
			BattleBufferManager.Instance.GenerateEffectFromBuffer(attribute.effectHit, GetTransform().position, 0f);
			Destroy();
		}

		public DS2ActiveObject GetCreator()
		{
			return m_creator;
		}

		public virtual void Emit(float distanceLife = 0f)
		{
			float speed = attribute.speed;
			LinearMoveToDestroy component = GetGameObject().GetComponent<LinearMoveToDestroy>();
			component.Move(speed, GetModelTransform().forward, distanceLife);
			GetGameObject().SetActive(true);
			if (m_effectParticle != null)
			{
				m_effectParticle.StartEmit();
			}
		}

		public virtual void EmitHoming(GameObject target, float homing_interval, float homing_life, float distanceLife = 0f)
		{
			float speed = attribute.speed;
			HomingMoveToDestroy component = GetGameObject().GetComponent<HomingMoveToDestroy>();
			component.Move(speed, GetModelTransform().forward, target, homing_interval, homing_life, distanceLife);
			GetGameObject().SetActive(true);
			if (m_effectParticle != null)
			{
				m_effectParticle.StartEmit();
			}
		}

		public override void Destroy(bool destroy = false)
		{
			if (destroy)
			{
				Object.Destroy(m_wrapGameObject);
				m_wrapGameObject = null;
				m_gameObject = null;
				m_transform = null;
				m_creator = null;
				hitInfo = null;
			}
			else
			{
				m_wrapGameObject.SetActive(false);
			}
			if (m_effectParticle != null)
			{
				m_effectParticle.StopEmit();
			}
		}
	}
}
