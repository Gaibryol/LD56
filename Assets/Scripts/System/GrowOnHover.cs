using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GrowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private void OnEnable()
	{
		Button button = GetComponent<Button>();
		if (button != null)
		{
			button.onClick.AddListener(Shrink);
		}
	}

	private void OnDisable()
	{
		Button button = GetComponent<Button>();
		if (button != null)
		{
			button.onClick.RemoveListener(Shrink);
		}
	}

	private void Grow()
	{
		transform.localScale = new Vector3(1.25f, 1.25f);
	}

	private void Shrink()
	{
		transform.localScale = new Vector3(1f, 1f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Grow();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Shrink();
	}
}