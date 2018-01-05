﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace Game.Component.UI {
	using Utility;

	public class Poster : MonoBehaviour {
		public AudioClip[] showingClips;
		public GameObject next;
		public float livingTime;
		public bool willDestroy = true;
		[SerializeField]
		private Slot[] onAwakeSlots;
		[SerializeField]
		private Slot[] onEndSlots;

		protected void Awake () {
			for (int i = 0; i < this.showingClips.Length; i++) {
				Sound.Play (this.showingClips [i]);
			}

			Sequence s = DOTween.Sequence ();
			s.AppendInterval (this.livingTime);
			s.AppendCallback (this.OnEnd);

			for (int i = 0; i < this.onAwakeSlots.Length; i++) {
				this.onAwakeSlots [i].Run (this.gameObject);
			}
		}

		private void OnEnd () {
			if (this.next != null) {
				GameObject.Instantiate (this.next, this.transform.parent);
			}

			if (this.willDestroy) {
				Destroy (this.gameObject);
			}

			for (int i = 0; i < this.onEndSlots.Length; i++) {
				this.onEndSlots [i].Run (this.gameObject);
			}
		}
	}
}