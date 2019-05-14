using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSwapper : MonoBehaviour
{
	[SerializeField]
	float m_Frame = 0f;

	[SerializeField]
	Sprite[] m_Frames;

	int m_OldFrame = 0;

	public Sprite[] Frames
	{
		get
		{
			return m_Frames;
		}
		set
		{
			m_Frames = value;
		}
	}

	public int frame
	{
		get
		{
			return (int)m_Frame;
		}
		set
		{
			m_Frame = (float)value;
		}
	}

	SpriteRenderer m_SpriteRenderer;
	public SpriteRenderer cachedSpriteRenderer
	{
		get
		{
			if (!m_SpriteRenderer)
			{
				m_SpriteRenderer = GetComponent<SpriteRenderer>();
			}

			return m_SpriteRenderer;
		}
	}

	void LateUpdate()
	{
        SpriteMask sm = transform.GetComponent<SpriteMask>();
		if (m_OldFrame != frame &&
		   m_Frames != null &&
		   m_Frames.Length > 0 && m_Frames.Length > frame &&
		   cachedSpriteRenderer)
		{
			m_OldFrame = frame;
			cachedSpriteRenderer.sprite = m_Frames[frame];
			if (sm)	sm.sprite = cachedSpriteRenderer.sprite;
		}
	}
}
