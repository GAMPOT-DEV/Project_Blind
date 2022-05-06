using Blind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI_EventHandler Ŭ�����Դϴ�. UI ���� �������̽����� �̿��ؼ� 
/// �ݺ����� �̺�Ʈ���� ȣ�����ݴϴ�.
/// </summary>

namespace Blind
{
	public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
	{
		public Action<PointerEventData> OnClickHandler = null;
		public Action<PointerEventData> OnDragHandler = null;
		public Action<PointerEventData> OnEndDragHandler = null;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (OnClickHandler != null)
				OnClickHandler.Invoke(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (OnDragHandler != null)
				OnDragHandler.Invoke(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (OnEndDragHandler != null)
				OnEndDragHandler.Invoke(eventData);
		}
	}
}

