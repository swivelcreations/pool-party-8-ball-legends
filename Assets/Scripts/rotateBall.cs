using UnityEngine;

public class rotateBall : MonoBehaviour
{
	private Transform thisTransform;

	private Rigidbody thisRigidbody;

	private Transform innerMeshTransform;

	private mainScript mainScriptComp;

	private void Start()
	{
		thisTransform = base.transform;
		thisRigidbody = GetComponent<Rigidbody>();
		innerMeshTransform = thisTransform.Find("ballMesh");
		mainScriptComp = GameObject.Find("cueBall").GetComponent<mainScript>();
		innerMeshTransform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
	}

	private void FixedUpdate()
	{
		if (thisRigidbody.isKinematic)
		{
			return;
		}
		if (thisRigidbody.linearVelocity.magnitude > 4f)
		{
			thisRigidbody.linearVelocity *= 0.993f;
		}
		else if (thisRigidbody.linearVelocity.magnitude > 2f)
		{
			thisRigidbody.linearVelocity *= 0.99f;
		}
		else if (thisRigidbody.linearVelocity.magnitude > 0f)
		{
			thisRigidbody.linearVelocity *= 0.96f;
			if (thisRigidbody.linearVelocity.magnitude < 0.1f)
			{
				thisRigidbody.linearVelocity = Vector3.zero;
			}
		}
		if (thisRigidbody.linearVelocity.magnitude > 0.1f)
		{
			innerMeshTransform.Rotate(thisRigidbody.linearVelocity.z * 2f, 0f, (0f - thisRigidbody.linearVelocity.x) * 2f, Space.World);
		}
		thisRigidbody.AddForce(0f, -60f, 0f, ForceMode.Force);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (thisRigidbody.linearVelocity.magnitude > 0.05f && collision.collider.CompareTag("ballTag") && Vector3.Angle(thisRigidbody.linearVelocity, collision.contacts[0].normal) < 84f && collision.contacts[0].thisCollider.GetComponent<Rigidbody>().linearVelocity.magnitude > collision.contacts[0].otherCollider.GetComponent<Rigidbody>().linearVelocity.magnitude)
		{
			mainScriptComp.playBallHitSound(Mathf.Clamp(thisRigidbody.linearVelocity.magnitude / 30f, 0.04f, 1f));
		}
		if (collision.collider.CompareTag("colSideTag") || collision.collider.CompareTag("colSideEndTag") || collision.collider.CompareTag("colHoleTag"))
		{
			if (mainScript.railHitBallArray[int.Parse(base.gameObject.name) - 1] != 1)
			{
				mainScript.railHitCountInThisShot++;
				mainScript.railHitBallArray[int.Parse(base.gameObject.name) - 1] = 1;
			}
			if (Vector3.Angle(thisRigidbody.linearVelocity, collision.contacts[0].normal) < 80f)
			{
				mainScriptComp.playRailHitSound(thisTransform.position, Mathf.Clamp(thisRigidbody.linearVelocity.magnitude / 45f, 0.06f, 1f));
			}
		}
	}
}
