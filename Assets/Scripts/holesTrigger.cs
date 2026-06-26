using UnityEngine;

public class holesTrigger : MonoBehaviour
{
	private mainScript mainScriptScript;

	public bool onlyForSound;

	private void Start()
	{
		mainScriptScript = GameObject.Find("cueBall").GetComponent<mainScript>();
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.GetComponent<Collider>().CompareTag("ballTag"))
		{
			if (!onlyForSound)
			{
				mainScriptScript.holesTriggerOnEnter(int.Parse(collision.GetComponent<Collider>().name));
			}
			else
			{
				mainScriptScript.holesSoundTriggerOnEnter();
			}
		}
	}
}
