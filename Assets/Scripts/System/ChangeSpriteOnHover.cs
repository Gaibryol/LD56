using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSpriteOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private Sprite hoverSprite;

	private void OnEnable()
	{
		Button button = GetComponent<Button>();
		if (button != null)
		{
			button.onClick.AddListener(Exit);
		}
	}

	private void OnDisable()
	{
		Button button = GetComponent<Button>();
		if (button != null)
		{
			button.onClick.RemoveListener(Exit);
		}
	}

	private void Hover()
	{
		GetComponent<Image>().sprite = hoverSprite;
	}

	private void Exit()
	{
		GetComponent<Image>().sprite = defaultSprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Hover();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Exit();
	}
}