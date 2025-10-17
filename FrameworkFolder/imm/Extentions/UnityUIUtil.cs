using System;
using UnityEngine;

public static class UnityUIUtil
{
	public static void AnchorAroundObject(this RectTransform target)
	{
		RectTransform parent = target.transform.parent.GetComponent<RectTransform>();
		
		Vector2 offsetMin = target.offsetMin;
		Vector2 offsetMax = target.offsetMax;
		Vector2 _anchorMin = target.anchorMin;
		Vector2 _anchorMax = target.anchorMax;
		
		float parent_width = parent.rect.width;      
		float parent_height = parent.rect.height;  
		
		Vector2 anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
		                            _anchorMin.y + (offsetMin.y / parent_height));
		Vector2 anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
		                            _anchorMax.y + (offsetMax.y / parent_height));
		
		target.anchorMin = anchorMin;
		target.anchorMax = anchorMax;
		
		target.offsetMin = new Vector2(0, 0);
		target.offsetMax = new Vector2(0, 0);
		target.pivot = new Vector2(0.5f, 0.5f);
	}

	public static void SetCenterAnchors(this RectTransform target)
	{
		Vector2 size = new Vector2(target.rect.width, target.rect.height);
        Vector3 pos = new Vector3(target.position.x, target.position.y, target.position.z);

		target.anchorMax = new Vector2(0.5f, 0.5f);
		target.anchorMin = new Vector2(0.5f, 0.5f);
		target.pivot = new Vector2(0.5f, 0.5f);

		target.sizeDelta = size;
		target.position = pos;
	}

	public static void FitToParentSize(this RectTransform target)
	{
		RectTransform parent = target.transform.parent.GetComponent<RectTransform>();
		Vector2 parentSize = new Vector2(parent.rect.width, parent.rect.height);

		target.sizeDelta = parentSize;
		target.localPosition = Vector2.zero;
	}

	public static void RoundTransformValues(this RectTransform target)
	{
		if (target.sizeDelta.magnitude == 0)
		{
			target.offsetMin = target.offsetMin.Round();
			target.offsetMax = target.offsetMax.Round();
		}
		else
		{
			target.anchoredPosition3D = target.anchoredPosition3D.Round();

			if (Mathf.Abs(target.anchoredPosition.x - Mathf.Floor(target.anchoredPosition.x)) > .0001f ||
			    Mathf.Abs(target.anchoredPosition.x - Mathf.Ceil(target.anchoredPosition.x)) > .0001f)
			{
				Vector3 position = target.position;
				position.x = Mathf.Round(target.position.x);
				target.position = position;
			}

			if (Mathf.Abs(target.anchoredPosition.y - Mathf.Floor(target.anchoredPosition.y)) > .0001f ||
			    Mathf.Abs(target.anchoredPosition.y - Mathf.Ceil(target.anchoredPosition.y)) > .0001f)
			{
				Vector3 position = target.position;
				position.y = Mathf.Round(target.position.y);
				target.position = position;
			}

			target.sizeDelta = target.sizeDelta.Round();
		}
		
		target.localScale = target.localScale.Round();
		target.localEulerAngles = target.localEulerAngles.Round();
	}
	
	internal static Vector2 Round(this Vector2 target)
	{
		return new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));
	}
	
	internal static Vector3 Round(this Vector3 target)
	{
		return new Vector3(Mathf.Round(target.x), Mathf.Round(target.y), Mathf.Round(target.z));
	}

	public static void LocalMirrorByX(this Transform target)
	{
		Vector3 oldPos = target.localPosition;
		target.localPosition = new Vector3(-oldPos.x, oldPos.y, oldPos.z);
	}

	public static void LocalMirrorByY(this Transform target)
	{
		Vector3 oldPos = target.localPosition;
		target.localPosition = new Vector3(oldPos.x, -oldPos.y, oldPos.z);
	}

	public static void LocalMirroByXY(this Transform target)
	{
		target.LocalMirrorByX();
		target.LocalMirrorByY();
	}
}