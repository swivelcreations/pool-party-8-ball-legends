using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class notificationScript : MonoBehaviour
{
	public static string notifText = string.Empty;

	public static float timeout;

	private RectTransform rectTransform;

	private float targetPosY;

	private Text notifTextComp;

	private LayoutElement notifTextLayoutElementComp;

	private GameObject[] avatarObjs = new GameObject[2];

	private Image[] avatarImages = new Image[2];

	private float notifAnimValue = 1f;

	private float notifAnimVel;

	private float notifAnimTarget;

	private mainScript mainScriptScript;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		targetPosY = rectTransform.anchoredPosition.y;
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetPosY - notifAnimValue * targetPosY);
		notifTextComp = base.transform.Find("Text").GetComponent<Text>();
		notifTextLayoutElementComp = base.transform.Find("Text").GetComponent<LayoutElement>();
		avatarObjs[0] = base.transform.Find("Avatar1").gameObject;
		avatarObjs[1] = base.transform.Find("Avatar2").gameObject;
		avatarImages[0] = avatarObjs[0].GetComponent<Image>();
		avatarImages[1] = avatarObjs[1].GetComponent<Image>();
		mainScriptScript = GameObject.Find("cueBall").GetComponent<mainScript>();
	}

	public void showNotification()
	{
		if (notifText != string.Empty)
		{
			notifAnimValue = 1f;
			notifAnimTarget = 0f;
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetPosY - notifAnimValue * targetPosY);
			base.gameObject.SetActive(true);
			StartCoroutine(resizeTheMessage());
			CancelInvoke("hideNotif");
			Invoke("hideNotif", timeout);
		}
	}

	private IEnumerator resizeTheMessage()
	{
		notifTextComp.text = notifText;
		avatarImages[(int)mainScript.currentTurn].sprite = mainScript.avatarTexArray[mainScript.chosenAvatar[(int)mainScript.currentTurn]];
		if (mainScript.chosenAvatar[(int)mainScript.currentTurn] == 10)
		{
			avatarObjs[0].SetActive(false);
			avatarObjs[1].SetActive(false);
		}
		else if (mainScript.currentTurn == TURN.PLAYER_1)
		{
			avatarObjs[0].SetActive(true);
			avatarObjs[1].SetActive(false);
		}
		else
		{
			avatarObjs[0].SetActive(false);
			avatarObjs[1].SetActive(true);
		}
		yield return new WaitForEndOfFrame();
		notifTextLayoutElementComp.preferredHeight = notifTextComp.preferredHeight + 80f;
	}

	private void hideNotif()
	{
		notifAnimTarget = 1f;
	}

	private void Update()
	{
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetPosY - notifAnimValue * (targetPosY * 1.1f));
		notifAnimValue = Mathf.SmoothDamp(notifAnimValue, notifAnimTarget, ref notifAnimVel, 0.2f);
		if (notifAnimTarget == 1f && notifAnimValue > 0.96f)
		{
			notifText = string.Empty;
			notifAnimValue = 1f;
			mainScriptScript.onNotificationComplete();
			base.gameObject.SetActive(false);
		}
		if (notifAnimValue < 0.15f && Input.GetMouseButtonDown(0))
		{
			CancelInvoke("hideNotif");
			hideNotif();
		}
	}
}
