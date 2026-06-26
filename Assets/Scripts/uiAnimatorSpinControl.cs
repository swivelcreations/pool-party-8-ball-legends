using UnityEngine;

public class uiAnimatorSpinControl : MonoBehaviour
{
	private RectTransform rectTransform;

	private CanvasGroup canvasGroupComponent;

	private float animValue;

	private float animTarget = 1f;

	private float animVel;

	[HideInInspector]
	public HAND_MODE handModeSpinControl;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasGroupComponent = GetComponent<CanvasGroup>();
	}

	public void showSpinControl()
	{
		base.transform.parent.gameObject.SetActive(true);
		animValue = 0f;
		animTarget = 1f;
		canvasGroupComponent.alpha = 0f;
	}

	public void hideSpinControl()
	{
		animValue = 1f;
		animTarget = 0f;
	}

	private void Update()
	{
		canvasGroupComponent.alpha = animValue;
		animValue = Mathf.SmoothDamp(animValue, animTarget, ref animVel, 0.14f);
		rectTransform.localScale = new Vector3(animValue, animValue, 1f);
		if (handModeSpinControl == HAND_MODE.Right)
		{
			rectTransform.anchoredPosition = new Vector2(50f, 245f) + new Vector2(240f, 40f) * animValue;
		}
		else if (handModeSpinControl == HAND_MODE.Left)
		{
			rectTransform.anchoredPosition = new Vector2(-50f, 245f) + new Vector2(-240f, 40f) * animValue;
		}
		if (animVel < 0f && animValue < 0.02f && animTarget == 0f)
		{
			base.transform.parent.gameObject.SetActive(false);
		}
	}
}
