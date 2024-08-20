using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class InteractionScript : MonoBehaviour
{
	public delegate void InteractionAction();
	public event InteractionAction OnInteract;


	[Serializable]
	public class InteractText
	{
		public bool enabled = true;
		public string interactionText = "Appuyer sur [BUTTON] pour interagir";
		public GameObject TextPrefab;
	}
	public Transform knob;

	public InteractText interactTexts = new InteractText();

	Transform player;
	bool inZone = false;
	private string PlayerHeadTag = "MainCamera";
	Canvas TextObj;
	Text theText;
	public KeyCode interactButton = KeyCode.E;


	// Start is called before the first frame update
	void Start()
	{
		if (GameObject.FindWithTag(PlayerHeadTag) != null)
		{
			player = GameObject.FindWithTag(PlayerHeadTag).transform;
		}
		else
		{
			Debug.LogWarning(gameObject.name + ": You need to set your player's camera tag to " + "'" + PlayerHeadTag + "'." + " The " + "'" + gameObject.name + "'" + " can't open/close if you don't set this tag");
		}

		AddText();
		DetectInteractKnob();
	}

	void AddText()
	{
		if (interactTexts.enabled)
		{
			if (interactTexts.TextPrefab == null)
			{
				Debug.LogWarning(gameObject.name + ": Text prefab missing, if you want see the text, please, put the text prefab in Text Prefab slot");
				return;
			}
			GameObject go = Instantiate(interactTexts.TextPrefab, Vector3.zero, new Quaternion(0, 0, 0, 0)) as GameObject;
			TextObj = go.GetComponent<Canvas>();

			theText = TextObj.GetComponentInChildren<Text>();
		}
	}

	void DetectInteractKnob()
	{
		if (knob == null)
		{
			Transform[] children = GetComponentsInChildren<Transform>(true);

			foreach (Transform child in children)
			{
				if (child.name == "interactKnob")
				{
					knob = child;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!inZone)
		{
			HideText();
			return;
		}

		if (PLayerIsLookingAtDoorKnob())
		{
			ShowText(interactTexts.interactionText);
		}
		else
		{
			HideText();
			return;
		}

		if (Input.GetKeyDown(interactButton)) 
		{
			OnInteract?.Invoke();
		} 
	}

	bool PLayerIsLookingAtDoorKnob()
	{
		Vector3 forward = player.TransformDirection(Vector3.back);
		Vector3 thisTransform = knob.position - player.transform.position;

		float dotProd = Vector3.Dot(forward.normalized, thisTransform.normalized);
		return (dotProd < 0 && dotProd < -0.9f);
	}



	#region TEXT
	/*
	 * 	TEXT
	 */

	void ShowText(string txt)
	{
		if(!interactTexts.enabled)

			return;

		string tempTxt = txt.Replace("[BUTTON]", "'" + interactButton.ToString() + "'");

		TextObj.enabled = false;
		theText.text = tempTxt;
		TextObj.enabled = true;
	}

	void HideText()
	{
		if (!interactTexts.enabled)
			return;
		if (interactTexts.TextPrefab != null)
		{
			TextObj.enabled = false;
		}
		else
		{
			interactTexts.enabled = false;
		}
	}
	#endregion

	private void OnDisable()
	{
		HideText();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
			return;

		inZone = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player")
			return;

		inZone = false;
	}
}
