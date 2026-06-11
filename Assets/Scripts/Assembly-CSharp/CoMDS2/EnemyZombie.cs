using UnityEngine;

namespace CoMDS2
{
	public class EnemyZombie : Enemy
	{
		public override void Initialize(GameObject prefab, string name, Vector3 position, Quaternion rotation, int layer)
		{
			base.Initialize(prefab, name, position, rotation, layer);
			AIStateMelee aIStateMelee = new AIStateMelee(this, "Melee");
			aIStateMelee.SetCustomFunc(OnMelee);
			float num = baseAttribute.moveSpeed * Random.Range(-0.25f, 0.25f);
			base.meleeRange = (m_baseMeleeRange = 1.3f);
			SetAttackCollider(false);
			AddAIState(aIStateMelee.name, aIStateMelee);
		}

		protected override void SetAnimationsMixing()
		{
			if (enemyAnimTag == null || enemyAnimTag == string.Empty)
			{
				return;
			}
			Transform mix = m_transform.Find("Bip01/Bip01 Spine");
			DataConf.AnimData enemyAnim = DataCenter.Conf().GetEnemyAnim(enemyAnimTag, "Hurt");
			if (enemyAnim.count > 1)
			{
				for (int i = 0; i < enemyAnim.count; i++)
				{
					m_gameObject.GetComponent<Animation>()[enemyAnim.name + "0" + (i + 1)].layer = 2;
					m_gameObject.GetComponent<Animation>()[enemyAnim.name + "0" + (i + 1)].AddMixingTransform(mix);
				}
			}
		}

		public new virtual void SetEnemy(Vector3 position, Quaternion rotation)
		{
			m_transform.position = position;
			m_transform.rotation = rotation;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}

		private void AddAnimationEvents()
		{
		}

		public override HitResultInfo OnHit(HitInfo hitInfo)
		{
			HitResultInfo result = new HitResultInfo();
			AIState currentAIState = GetCurrentAIState();
			if (currentAIState != null && currentAIState.name == "Born")
			{
				return result;
			}
			return base.OnHit(hitInfo);
		}
	}
}
