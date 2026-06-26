using UnityEngine;

public class musicPlayer : MonoBehaviour
{
	private AudioSource thisAudioSource;

	private string[] musicArray = new string[4] { "MorningGoodNews", "LifeIsForLiving", "LiftMeUp", "DriftAway" };

	private void Start()
	{
		thisAudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (mainScript.musicVolVal > 0f && !thisAudioSource.isPlaying)
		{
			thisAudioSource.clip = Resources.Load("Musics/" + musicArray[Random.Range(0, musicArray.Length)]) as AudioClip;
			thisAudioSource.Play();
		}
	}
}
