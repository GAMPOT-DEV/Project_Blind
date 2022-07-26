﻿using Blind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI_EventHandler 클래스입니다. UI 관련 인터페이스들을 이용해서 
/// 콜벡으로 이벤트들을 호출해줍니다.
/// </summary>

namespace Blind
{
	public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
	{
		public Action OnClickHandler = null;
		public Action OnDragHandler = null;
		public Action OnEndDragHandler = null;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (OnClickHandler != null)
				OnClickHandler.Invoke();
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (OnDragHandler != null)
				OnDragHandler.Invoke();
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (OnEndDragHandler != null)
				OnEndDragHandler.Invoke();
		}
	}
}

