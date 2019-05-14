using UnityEngine;
using Anima2D;

[ExecuteInEditMode]
public class SpriteSwapperManager : MonoBehaviour
{
	[SerializeField]
	[Range(0, 20)]
	int m_Frame = 0;

	int m_OldFrame = 0;

	private SpriteMeshAnimation[] childrenSpriteMeshComponents;
	private SpriteSwapper[] childrenSpriteSwapperComponents;

	private void Awake()
	{
		childrenSpriteMeshComponents = GetComponentsInChildren<SpriteMeshAnimation>();
		childrenSpriteSwapperComponents = GetComponentsInChildren<SpriteSwapper>();
	}

	public int frame
	{
		get
		{
			return (int)m_Frame;
		}
		set
		{
			m_Frame = (int)value;
		}
	}

	void LateUpdate()
	{
		if (m_OldFrame != frame)
		{
			foreach (SpriteMeshAnimation sm in childrenSpriteMeshComponents)
			{
				if (frame < sm.frames.Length)
				{
					sm.frame = frame;
				}
			}
			foreach (SpriteSwapper sw in childrenSpriteSwapperComponents)
			{
				if (frame < sw.Frames.Length)
				{
					sw.frame = frame;
				}
			}
			m_OldFrame = frame;
		}
	}
}
