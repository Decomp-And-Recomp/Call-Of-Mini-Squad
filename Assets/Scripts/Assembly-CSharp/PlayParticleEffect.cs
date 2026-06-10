/*using System.Collections.Generic;
using UnityEngine;

internal class PlayParticleEffect : MonoBehaviour
{
	public float m_time = 10f;

	private float m_play_time;

	private List<GameObject> m_emits = new List<GameObject>();

	private void Start()
	{
		ParticleEmitter[] componentsInChildren = GetComponentsInChildren<ParticleEmitter>();
		ParticleEmitter[] array = componentsInChildren;
		foreach (ParticleEmitter particleEmitter in array)
		{
			GameObject gameObject = Object.Instantiate(particleEmitter.gameObject) as GameObject;
			gameObject.GetComponent<ParticleEmitter>().emit = false;
			m_emits.Add(gameObject);
		}
	}

	private void Update()
	{
		m_play_time += Time.deltaTime;
		if (!(m_play_time > m_time))
		{
			return;
		}
		foreach (GameObject emit in m_emits)
		{
			GameObject gameObject = Object.Instantiate(emit) as GameObject;
			gameObject.transform.position += base.transform.position;
			gameObject.GetComponent<ParticleEmitter>().Emit();
		}
		m_play_time = 0f;
	}
}
*/