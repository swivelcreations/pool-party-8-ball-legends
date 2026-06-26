using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class egHighScoresScript : MonoBehaviour
{
	private string egUniqueUID;

	private const int TOTAL_SCORES_COUNT = 25;

	private const int ROW_HEIGHT = 80;

	private const int ROW_SPACING = 2;

	private int scoreToSubmit;

	private string playerNameString = string.Empty;

	private string formText = string.Empty;

	private string phpSubmitUrl = "http://www.eivaagames.com/games/highscores/submit-score.php";

	private string secretKey = "3df56B87b13sGhe21Hc";

	private RectTransform eghsPanelRectTrans;

	private GameObject eghsScrollAreaObj;

	private RectTransform eghsScrollContentsRectTrans;

	private GameObject eghsErrorText;

	private GameObject eghsLoadingRound;

	private GameObject[] eghsRowsObjArray;

	private Text[] eghsNamesTextArray;

	private Text[] eghsScoresTextArray;

	private Transform eghsYouLabelTrans;

	private GameObject eghsOutPlaceRowObj;

	private Text eghsOutPlaceRankText;

	private Text eghsOutPlaceNameText;

	private Text eghsOutPlaceScore;

	private Sprite[] medalImagesArray = new Sprite[3];

	private Vector2 eghsPanelRectOffsetMin;

	private WWW wwwEGHighScoresData;

	private string phpGetScoresUrl = "http://www.eivaagames.com/games/highscores/get-scores.php?id=" + 34;

	private string[,] egHighScoresArray = new string[25, 3];

	private string[] egHighScoresTempArray = new string[25];

	private bool egHighScoresGetError;

	private int egHighScoresMinScoreVal;

	public void init(int scoreVal, string nameVal)
	{
		scoreToSubmit = scoreVal;
		getUniqueID();
		if (nameVal != string.Empty)
		{
			playerNameString = nameVal;
		}
		else
		{
			playerNameString = "Player";
		}
		eghsPanelRectTrans = GameObject.Find("HighScoresParent/Panel").GetComponent<RectTransform>();
		eghsScrollAreaObj = GameObject.Find("HighScoresParent/Panel/ScrollArea");
		eghsScrollContentsRectTrans = GameObject.Find("HighScoresParent/Panel/ScrollArea/Contents").GetComponent<RectTransform>();
		eghsErrorText = GameObject.Find("HighScoresParent/Panel/ErrorText");
		eghsLoadingRound = GameObject.Find("HighScoresParent/Panel/LoadingRound");
		eghsRowsObjArray = new GameObject[25];
		eghsRowsObjArray[0] = GameObject.Find("HighScoresParent/Panel/ScrollArea/Contents/Row0");
		eghsYouLabelTrans = GameObject.Find("HighScoresParent/Panel/ScrollArea/Contents/YouLabel").transform;
		eghsOutPlaceRowObj = GameObject.Find("HighScoresParent/Panel/OutPlace");
		eghsOutPlaceRankText = GameObject.Find("HighScoresParent/Panel/OutPlace/Row/Rank/Text").GetComponent<Text>();
		eghsOutPlaceNameText = GameObject.Find("HighScoresParent/Panel/OutPlace/Row/Name").GetComponent<Text>();
		eghsOutPlaceScore = GameObject.Find("HighScoresParent/Panel/OutPlace/Row/Score").GetComponent<Text>();
		eghsNamesTextArray = new Text[25];
		eghsScoresTextArray = new Text[25];
		medalImagesArray[0] = Resources.Load<Sprite>("egHSMedals/medalGold");
		medalImagesArray[1] = Resources.Load<Sprite>("egHSMedals/medalSilver");
		medalImagesArray[2] = Resources.Load<Sprite>("egHSMedals/medalBronze");
		eghsPanelRectOffsetMin = eghsPanelRectTrans.offsetMin;
		eghsScrollContentsRectTrans.SetSizeDeltaY(2048f);
		for (int i = 0; i < 25; i++)
		{
			if (i > 0)
			{
				eghsRowsObjArray[i] = Object.Instantiate(eghsRowsObjArray[0]);
				eghsRowsObjArray[i].transform.SetParent(eghsRowsObjArray[0].transform.parent, false);
				eghsRowsObjArray[i].name = "Row" + i;
				eghsRowsObjArray[i].GetComponent<RectTransform>().SetAnchoredPositionY(i * -82);
				eghsRowsObjArray[i].transform.Find("Rank/Text").GetComponent<Text>().text = i + 1 + string.Empty;
			}
			Vector2 offsetMin = eghsRowsObjArray[i].transform.Find("Name").GetComponent<RectTransform>().offsetMin;
			if (i < 3)
			{
				eghsRowsObjArray[i].transform.Find("Medal").GetComponent<Image>().sprite = medalImagesArray[i];
				offsetMin.x = 150f;
				eghsRowsObjArray[i].transform.Find("Name").GetComponent<RectTransform>().offsetMin = offsetMin;
			}
			else
			{
				offsetMin.x = 102f;
				eghsRowsObjArray[i].transform.Find("Name").GetComponent<RectTransform>().offsetMin = offsetMin;
				Object.Destroy(eghsRowsObjArray[i].transform.Find("Medal").gameObject);
			}
			eghsNamesTextArray[i] = GameObject.Find("HighScoresParent/Panel/ScrollArea/Contents/Row" + i + "/Name").GetComponent<Text>();
			eghsScoresTextArray[i] = GameObject.Find("HighScoresParent/Panel/ScrollArea/Contents/Row" + i + "/Score").GetComponent<Text>();
		}
	}

	public void submitScore(int scoreVal, string nameVal)
	{
		scoreToSubmit = scoreVal;
		if (nameVal != string.Empty)
		{
			playerNameString = nameVal;
		}
		else
		{
			playerNameString = "Player";
		}
		StartCoroutine(egSubmitScore());
	}

	private IEnumerator egSubmitScore()
	{
		if (scoreToSubmit > egHighScoresMinScoreVal)
		{
			formText = "Please Wait...";
			WWWForm form = new WWWForm();
			form.AddField("secretKey", secretKey);
			form.AddField("playerName", playerNameString);
			form.AddField("gameScore", scoreToSubmit);
			form.AddField("gameId", 34);
			form.AddField("uid", egUniqueUID);
			WWW wwwData = new WWW(phpSubmitUrl, form);
			yield return wwwData;
			if (string.IsNullOrEmpty(wwwData.error))
			{
				formText = wwwData.text;
				MonoBehaviour.print(formText);
				wwwData.Dispose();
			}
			else
			{
				MonoBehaviour.print(wwwData.error);
			}
			StartCoroutine(egGetHighScores());
		}
		else if (egHighScoresGetError)
		{
			StartCoroutine(egGetHighScores());
		}
		else
		{
			refreshHighScoresGui();
		}
	}

	private void getUniqueID()
	{
		if (PlayerPrefs.HasKey("egUniqueUID"))
		{
			egUniqueUID = PlayerPrefs.GetString("egUniqueUID");
			return;
		}
		egUniqueUID = Random.Range(0, 9999999).ToString();
		PlayerPrefs.SetString("egUniqueUID", egUniqueUID);
		PlayerPrefs.Save();
	}

	public void getScores()
	{
		StartCoroutine(egGetHighScores());
	}

	private IEnumerator egGetHighScores()
	{
		eghsErrorText.gameObject.SetActive(false);
		eghsLoadingRound.gameObject.SetActive(true);
		eghsScrollAreaObj.gameObject.SetActive(false);
		eghsOutPlaceRowObj.SetActive(false);
		wwwEGHighScoresData = new WWW(phpGetScoresUrl);
		yield return wwwEGHighScoresData;
		if (string.IsNullOrEmpty(wwwEGHighScoresData.error) && wwwEGHighScoresData.text.Trim() != string.Empty)
		{
			egHighScoresTempArray = wwwEGHighScoresData.text.Trim().Split("\n"[0]);
			if (egHighScoresTempArray.Length == 25)
			{
				for (int i = 0; i < 25; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						egHighScoresArray[i, j] = egHighScoresTempArray[i].ToString().Split(","[0])[j];
					}
				}
				egHighScoresGetError = false;
				egHighScoresMinScoreVal = int.Parse(egHighScoresArray[24, 2]);
			}
			else
			{
				egHighScoresGetError = true;
			}
		}
		else
		{
			egHighScoresGetError = true;
		}
		refreshHighScoresGui();
	}

	private void refreshHighScoresGui()
	{
		if (!egHighScoresGetError)
		{
			eghsScrollAreaObj.gameObject.SetActive(true);
			eghsErrorText.gameObject.SetActive(false);
			eghsLoadingRound.gameObject.SetActive(false);
			eghsYouLabelTrans.gameObject.SetActive(false);
			bool flag = false;
			for (int i = 0; i < 25; i++)
			{
				eghsNamesTextArray[i].text = egHighScoresArray[i, 1];
				eghsScoresTextArray[i].text = egHighScoresArray[i, 2];
				if (egHighScoresArray[i, 0] == egUniqueUID)
				{
					eghsRowsObjArray[i].GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.47058824f);
					eghsYouLabelTrans.SetParent(eghsRowsObjArray[i].transform, false);
					eghsYouLabelTrans.gameObject.SetActive(true);
					eghsScrollContentsRectTrans.SetAnchoredPositionY(i * 80 - 80);
					flag = true;
				}
				else
				{
					eghsRowsObjArray[i].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.3529412f);
				}
			}
			if (flag)
			{
				eghsOutPlaceRowObj.SetActive(false);
				eghsPanelRectTrans.offsetMin = eghsPanelRectOffsetMin;
				return;
			}
			eghsOutPlaceRankText.text = calculateOutPlaceRank();
			eghsOutPlaceNameText.text = playerNameString;
			eghsOutPlaceScore.text = scoreToSubmit + string.Empty;
			eghsOutPlaceRowObj.SetActive(true);
			eghsPanelRectTrans.offsetMin = eghsPanelRectOffsetMin + new Vector2(0f, 82f);
		}
		else
		{
			eghsScrollAreaObj.gameObject.SetActive(false);
			eghsErrorText.gameObject.SetActive(true);
			eghsLoadingRound.gameObject.SetActive(false);
			eghsOutPlaceRowObj.SetActive(false);
		}
	}

	private string calculateOutPlaceRank()
	{
		string empty = string.Empty;
		return 27 + (egHighScoresMinScoreVal - scoreToSubmit) * 123 + string.Empty;
	}
}
