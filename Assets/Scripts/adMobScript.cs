using System;
 
using UnityEngine;

public class adMobScript : MonoBehaviour
{
	private bool ADMOB_DEBUG;

	private bool interstitialRequested;

	private float adDisplayedTime = -60f;

	private float minDelayBetweenAds = 60f;

 

	private void Awake()
	{
		 
	}

 

	public void RequestBanner()
	{
		 
	}

	public void DestroyBanner()
	{
	 
	}

	public void HandleAdLoaded(object sender, EventArgs args)
	{
		admobPrint("HandleAdLoaded event received.");
	}

 
	public void HandleAdOpened(object sender, EventArgs args)
	{
		admobPrint("HandleAdOpened event received");
	}

	public void HandleAdClosed(object sender, EventArgs args)
	{
		admobPrint("HandleAdClosed event received");
	}

	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		admobPrint("HandleAdLeftApplication event received");
	}

	public void RequestInterstitial()
	{
		 
	}

	public void ShowInterstitial()
	{
		 
	}

	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
 
	}
 

	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		 
	}

	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		 
	}

	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		 
	}

	private void admobPrint(string msg)
	{
		 
	}
}
