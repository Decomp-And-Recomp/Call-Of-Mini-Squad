using UnityEngine;

public class ColorAnimationScript : MonoBehaviour
{
	public float m_AnimPeriod = 1f;

	public Color m_StartColor;

	public Color m_EndColor;

	private float m_splashTime = 0.4f;

	private float m_timer;

	private bool m_bSplash;

	private bool m_reset;

	private string m_propertyName;

	private Color m_defaultColor;

	private Color m_changeColor;

	private float m_AnimPeriodChange = 0.8f;

	private bool m_bChange;

	private void Awake()
	{
		SetColorAnimation();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_bSplash)
		{
			m_timer -= Time.deltaTime;
			if (m_timer <= 0f)
			{
				m_timer = 0f;
				m_bSplash = false;
				ResetColorAnimation();
			}
		}
	}

	public void SetColorAnimation()
	{
		if (base.GetComponent<Renderer>().enabled)
		{
			m_propertyName = "_TintColor";
			if (base.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				m_propertyName = "_TintColor";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				m_propertyName = "_Color";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_texBase"))
			{
				m_propertyName = "_texBase";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			if (!base.GetComponent<Animation>())
			{
				base.gameObject.AddComponent<Animation>();
			}
			AnimationCurve curve = new AnimationCurve(new Keyframe(0f, m_StartColor.r, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.r, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.r, 0f, 0f));
			AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, m_StartColor.g, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.g, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.g, 0f, 0f));
			AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, m_StartColor.b, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.b, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.b, 0f, 0f));
			AnimationClip animationClip = new AnimationClip();
			animationClip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".r", curve);
			animationClip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".g", curve2);
			animationClip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".b", curve3);
			animationClip.wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(animationClip, "ColorAnimation");
			AnimationClip clip = new AnimationClip();
			base.GetComponent<Animation>().AddClip(clip, "ChangeColorAnimation");
			AnimationClip clip2 = new AnimationClip();
			base.GetComponent<Animation>().AddClip(clip2, "ResetColorAnimation");
		}
	}

	public void PlayColorAnimation(float time = 0.4f)
	{
		if (!m_bChange && base.GetComponent<Animation>()["ColorAnimation"] != null)
		{
			base.GetComponent<Animation>()["ColorAnimation"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().Play("ColorAnimation");
			m_bSplash = true;
			m_reset = false;
			m_timer = m_splashTime;
		}
	}

	public void ResetColorAnimation()
	{
		if (base.GetComponent<Animation>()["ColorAnimation"] != null)
		{
			base.GetComponent<Animation>().Stop("ColorAnimation");
			if (base.GetComponent<Renderer>().enabled)
			{
				base.GetComponent<Renderer>().material.SetColor(m_propertyName, m_StartColor);
			}
		}
	}

	public void ChangeColor(Color color)
	{
		if (base.GetComponent<Renderer>().enabled)
		{
			m_propertyName = "_TintColor";
			if (base.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				m_propertyName = "_TintColor";
				m_defaultColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				m_propertyName = "_Color";
				m_defaultColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_texBase"))
			{
				m_propertyName = "_texBase";
				m_defaultColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			m_changeColor = color;
			AnimationCurve curve = new AnimationCurve(new Keyframe(0f, m_defaultColor.r, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_changeColor.r, 0f, 0f));
			AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, m_defaultColor.g, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_changeColor.g, 0f, 0f));
			AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, m_defaultColor.b, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_changeColor.b, 0f, 0f));
			AnimationCurve curve4 = new AnimationCurve(new Keyframe(0f, m_defaultColor.a, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_changeColor.a, 0f, 0f));
			AnimationClip clip = base.GetComponent<Animation>().GetClip("ChangeColorAnimation");
			if (clip == null)
			{
				clip = new AnimationClip();
				clip.legacy = true;
				base.GetComponent<Animation>().AddClip(clip, "ChangeColorAnimation");
			}
			clip.ClearCurves();
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".r", curve);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".g", curve2);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".b", curve3);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".a", curve4);
			clip.wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().Play("ChangeColorAnimation");
			m_bChange = true;
		}
	}

	public void ResetColor()
	{
		if (!(base.GetComponent<Renderer>() == null) && base.GetComponent<Renderer>().enabled)
		{
			m_propertyName = "_TintColor";
			if (base.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				m_propertyName = "_TintColor";
				m_changeColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				m_propertyName = "_Color";
				m_changeColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_texBase"))
			{
				m_propertyName = "_texBase";
				m_changeColor = base.GetComponent<Renderer>().material.GetColor(m_propertyName);
			}
			AnimationCurve curve = new AnimationCurve(new Keyframe(0f, m_changeColor.r, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_StartColor.r, 0f, 0f));
			AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, m_changeColor.g, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_StartColor.g, 0f, 0f));
			AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, m_changeColor.b, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_StartColor.b, 0f, 0f));
			AnimationCurve curve4 = new AnimationCurve(new Keyframe(0f, m_changeColor.a, 0f, 0f), new Keyframe(m_AnimPeriodChange, m_StartColor.a, 0f, 0f));
			AnimationClip clip = base.GetComponent<Animation>().GetClip("ResetColorAnimation");
			if (clip == null)
			{
				clip = new AnimationClip();
				clip.legacy = true;
				base.GetComponent<Animation>().AddClip(clip, "ResetColorAnimation");
			}
			clip.ClearCurves();
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".r", curve);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".g", curve2);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".b", curve3);
			clip.SetCurve(string.Empty, typeof(Material), m_propertyName + ".a", curve4);
			clip.wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().Play("ResetColorAnimation");
			m_bChange = false;
		}
	}
}
