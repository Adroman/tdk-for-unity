using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log($"{other.gameObject.name} entered.");
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log($"{other.gameObject.name} left.");
	}
}
