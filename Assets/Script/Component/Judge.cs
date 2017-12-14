﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Component {
	using Utility;

	public class Judge : MonoBehaviour {
		private const int SCORE_MAX = 5;
		private const float SHOT_TIME = 2;
		private const float BALL_SPEED = 6;

		[SerializeField]
		private Wall wallA;
		[SerializeField]
		private Wall wallB;
		[SerializeField]
		private Shooter shooterA;
		[SerializeField]
		private Shooter shooterB;
		[SerializeField]
		private AudioClip[] clips;
		public float volume = 0.5f;
		public float acceleration = 0.00005f;

		private AudioSource audioSource;
		private bool aShooted;
		private Timer timer;
		private int scoreA;
		private int scoreB;
		private Ball ball;

		void Awake() {
			this.audioSource = this.GetComponent<AudioSource> ();
			this.timer = new Timer();
			this.timer.Enter (SHOT_TIME);
		}

		void FixedUpdate() {
			this.audioSource.pitch += this.acceleration;

			if (this.ball != null) {
				this.ball.speed = BALL_SPEED * this.audioSource.pitch;
			}

			if (!this.timer.isRunning) {
				return;
			}

			this.timer.Update (Time.fixedDeltaTime);
			
			if (!this.timer.isRunning) {
				GameObject obj;

				if (this.aShooted) {
					obj = this.shooterA.Shoot ();
				} else {
					obj = this.shooterB.Shoot ();
				}

				this.ball = obj.GetComponent<Ball> ();
				this.ball.OnDestroyEvent += ReadyBall;

				if (!this.audioSource.isPlaying) {
					this.audioSource.Play ();
				}

				this.aShooted = !this.aShooted;
			}
		}

		private void ReadyBall(Vector3 position) {
			this.timer.Enter (SHOT_TIME);

			if (position.x > 0) {
				this.scoreB += 1;
				this.wallB.SetLength ((float)this.scoreB / (float)SCORE_MAX);
			} else {
				this.scoreA += 1;
				this.wallA.SetLength ((float)this.scoreA / (float)SCORE_MAX);
			}

			for (int i = 0; i < this.clips.Length; i++) {
				AudioSource.PlayClipAtPoint (this.clips [i], Vector3.zero, this.volume);
			}
		}
	}
}