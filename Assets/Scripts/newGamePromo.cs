using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class newGamePromo : MonoBehaviour
{
	private WWW wwwMoreGamesData;

	private WWW wwwMoreGamesImg;

	private GameObject MoreGamesLoading;

	private GameObject MoreGamesRoundLoading;

	private GameObject MoreGamesErrorLoading;

	private GameObject MoreGamesUiParent;

	private RawImage MoreGamesImageComp;

	private GameObject goToMoreGamesStoreBtn;

	private string[] MoreGamesDataArray;

	private bool comingFromMoreGamesBtn;

	public static bool promoActive;

	private void Start()
	{
		StartCoroutine(newGamePromoInit());
	}

	private IEnumerator newGamePromoInit()
	{
		MoreGamesLoading = GameObject.Find("Canvas/AllParent/MoreGames/Loading");
		MoreGamesRoundLoading = GameObject.Find("Canvas/AllParent/MoreGames/Loading/Round");
		MoreGamesErrorLoading = GameObject.Find("Canvas/AllParent/MoreGames/Loading/ErrorText");
		MoreGamesUiParent = GameObject.Find("Canvas/AllParent/MoreGames/BGBlack");
		MoreGamesImageComp = GameObject.Find("Canvas/AllParent/MoreGames/BGBlack/ImagePlace/Image").GetComponent<RawImage>();
		goToMoreGamesStoreBtn = GameObject.Find("Canvas/AllParent/MoreGames/GoToMoreGamesStore");
		MoreGamesLoading.SetActive(false);
		MoreGamesErrorLoading.SetActive(false);
		MoreGamesUiParent.SetActive(false);
		goToMoreGamesStoreBtn.SetActive(false);
		yield return new WaitForEndOfFrame();
		if (mainScript.startupCounter > 1)
		{
			StartCoroutine(loadNewGamePromoWwwImage());
		}
	}

	private IEnumerator loadNewGamePromoWwwImage()
	{
		wwwMoreGamesData = new WWW("http://www.eivaagames.com/gmg/egng-promo.php?id=" + 34);
		yield return wwwMoreGamesData;
		if (string.IsNullOrEmpty(wwwMoreGamesData.error))
		{
			if (wwwMoreGamesData.text.Trim() != string.Empty)
			{
				MoreGamesDataArray = wwwMoreGamesData.text.Split(","[0]);
				wwwMoreGamesImg = new WWW(MoreGamesDataArray[0]);
				yield return wwwMoreGamesImg;
				if (string.IsNullOrEmpty(wwwMoreGamesImg.error))
				{
					if (mainScript.curScreen == "MainMenu")
					{
						promoActive = true;
						Time.timeScale = 0f;
						MoreGamesLoading.SetActive(false);
						if (comingFromMoreGamesBtn)
						{
							goToMoreGamesStoreBtn.SetActive(true);
						}
						MoreGamesUiParent.SetActive(true);
						MoreGamesImageComp.texture = wwwMoreGamesImg.texture;
					}
				}
				else
				{
					MoreGamesErrorLoading.SetActive(true);
					MoreGamesRoundLoading.SetActive(false);
				}
			}
			else
			{
				onClickMoreGamesClose();
			}
		}
		else
		{
			MoreGamesErrorLoading.SetActive(true);
			MoreGamesRoundLoading.SetActive(false);
		}
	}

	public void onClickMoreGamesClose()
	{
		MoreGamesLoading.SetActive(false);
		MoreGamesUiParent.SetActive(false);
		goToMoreGamesStoreBtn.SetActive(false);
		Time.timeScale = 1f;
		promoActive = false;
	}

	public void onClickMoreGamesImage()
	{
		Application.OpenURL(MoreGamesDataArray[2]);
		onClickMoreGamesClose();
	}

	public void onClickGMGBtn()
	{
		goToMoreGamesStore();
	}

	public void goToMoreGamesStore()
	{
		Application.OpenURL("market://search?q=pub:EivaaGames");
	}
}
