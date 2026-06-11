using UnityEngine;

public class EffectParticleContinuous : EffectControl
{
	// private ParticleEmitter[] m_emitter;

	private ParticleSystem[] m_particleSystem;

	private Animation[] m_animations;

	private Animator[] m_animator;

	public bool loop;

	public bool enableEmission;

	private EffectAudioBehaviour audioEvents;

	private bool play;

	public float m_Time;

	public float m_maxTime;

	public void Awake()
	{
		/*
		ParticleAnimator[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleAnimator>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].autodestruct = false;
		}
		m_emitter = base.gameObject.GetComponentsInChildren<ParticleEmitter>(true);
		for (int j = 0; j < m_emitter.Length; j++)
		{
			m_emitter[j].emit = enableEmission;
		}
		*/

		m_particleSystem = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		for (int k = 0; k < m_particleSystem.Length; k++)
		{
			var mainModule = m_particleSystem[k].main;
			mainModule.playOnAwake = enableEmission;
			mainModule.loop = loop;
			// m_particleSystem[k].enableEmission = enableEmission;
			var emissionModule = m_particleSystem[k].emission;
			emissionModule.enabled = enableEmission;
		}

		m_animations = base.gameObject.GetComponentsInChildren<Animation>(true);
		for (int l = 0; l < m_animations.Length; l++)
		{
			m_animations[l].wrapMode = ((!loop) ? WrapMode.Once : WrapMode.Loop);
		}
		m_animator = base.gameObject.GetComponentsInChildren<Animator>(true);
		audioEvents = base.Root.GetComponentInChildren<EffectAudioBehaviour>();
	}

	public override void StartEmit(bool playAudio = true)
	{
		base.StartEmit();
		if (m_particleSystem == null)
		{
			Awake();
		}
		/*
		for (int i = 0; i < m_emitter.Length; i++)
		{
			m_emitter[i].emit = true;
		}
		*/

		for (int j = 0; j < m_particleSystem.Length; j++)
		{
			// m_particleSystem[j].enableEmission = true;
			var emissionModule = m_particleSystem[j].emission;
			emissionModule.enabled = true;

			m_particleSystem[j].Play();
		}
		for (int k = 0; k < m_animations.Length; k++)
		{
			m_animations[k].Play();
		}
		if (DataCenter.Save().PlaySound && audioEvents != null && playAudio)
		{
			audioEvents.PlaySFX();
		}
	}

	public override void StopEmit()
	{
		if (!(GetComponent<TimerDestroy>() != null))
		{
			if (m_particleSystem == null)
			{
				Awake();
			}
			/*
			for (int i = 0; i < m_emitter.Length; i++)
			{
				m_emitter[i].emit = enableEmission;
			}
			*/

			for (int j = 0; j < m_particleSystem.Length; j++)
			{
				// m_particleSystem[j].enableEmission = enableEmission;
				var emissionModule = m_particleSystem[j].emission;
				emissionModule.enabled = enableEmission;

				m_particleSystem[j].Stop();
				m_particleSystem[j].Clear();
			}
			for (int k = 0; k < m_animations.Length; k++)
			{
				m_animations[k].Stop();
			}
			base.StopEmit();
			if (DataCenter.Save().PlaySound && audioEvents != null)
			{
				audioEvents.StopSFX();
			}
		}
	}

	public void Update()
	{
		if (m_particleSystem == null)
		{
			return;
		}
		if (!loop && (m_animator.Length > 0 || m_animations.Length > 0))
		{
			bool flag = true;
			for (int i = 0; i < m_animator.Length; i++)
			{
				if (m_animator[i].GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
				{
					flag = false;
					break;
				}
			}
			for (int j = 0; j < m_animations.Length; j++)
			{
				if (m_animations[j].isPlaying)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				base.Root.SetActive(false);
				return;
			}
		}
		if (play)
		{
			m_Time += Time.deltaTime;
			if (m_maxTime > 0f && m_Time >= m_maxTime)
			{
				StopEffect();
				m_Time = 0f;
				play = false;
			}
		}
	}

	public void PlayEffect()
	{
		play = true;
		m_Time = 0f;
		m_particleSystem = GetComponentsInChildren<ParticleSystem>();
		m_animations = GetComponentsInChildren<Animation>();
		m_animator = GetComponentsInChildren<Animator>();
		if (m_maxTime == 0f)
		{
			if (m_animations.Length > 0)
			{
				float num = 0f;
				Animation[] animations = m_animations;
				foreach (Animation animation in animations)
				{
					num = ((!(animation.clip.length > num)) ? num : animation.clip.length);
				}
				m_maxTime = num;
			}
			else if (m_particleSystem.Length > 0)
			{
				float num2 = 0f;
				ParticleSystem[] array = m_particleSystem;
				foreach (ParticleSystem particleSystem in array)
				{
					var mainModule = particleSystem.main;
					num2 = ((!(mainModule.duration > num2)) ? num2 : mainModule.duration);
				}
				m_maxTime = num2;
			}
			else if (m_animator.Length > 0)
			{
				float num3 = 0f;
				Animator[] animator = m_animator;
				foreach (Animator animator2 in animator)
				{
					num3 = ((!(animator2.GetCurrentAnimatorStateInfo(0).length > num3)) ? num3 : animator2.GetCurrentAnimatorStateInfo(0).length);
				}
				m_maxTime = num3;
			}
		}
		Animation[] animations2 = m_animations;
		foreach (Animation animation2 in animations2)
		{
			animation2.Play();
		}
		ParticleSystem[] array2 = m_particleSystem;
		foreach (ParticleSystem particleSystem2 in array2)
		{
			// particleSystem2.enableEmission = true;
			var emissionModule = particleSystem2.emission;
			emissionModule.enabled = true;

			particleSystem2.Play();
		}
		EffectAudioBehaviour component = base.gameObject.GetComponent<EffectAudioBehaviour>();
		if (component != null)
		{
			component.PlaySFX();
		}
	}

	public void StopEffect()
	{
		Animation[] animations = m_animations;
		foreach (Animation animation in animations)
		{
			animation.Stop();
		}
		ParticleSystem[] array = m_particleSystem;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Stop();
		}
		EffectAudioBehaviour component = base.gameObject.GetComponent<EffectAudioBehaviour>();
		if (component != null)
		{
			component.StopSFX();
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
	}
}