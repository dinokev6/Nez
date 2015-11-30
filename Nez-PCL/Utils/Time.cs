﻿using System;


namespace Nez
{
	public static class Time
	{
		public static float time;
		public static float deltaTime;
		public static float unscaledDeltaTime;
		public static float altDeltaTime;
		public static float timeSinceSceneLoad;
		public static float timeScale = 1f;
		public static float altTimeScale = 1f;
		/// <summary>
		/// total number of frames that have passed
		/// </summary>
		public static uint frameCount;


		public static void update( float dt )
		{
			time += dt;
			deltaTime = dt * timeScale;
			altDeltaTime = dt * altTimeScale;
			unscaledDeltaTime = dt;
			timeSinceSceneLoad += dt;
			frameCount++;
		}


		public static void sceneChanged()
		{
			timeSinceSceneLoad = 0f;
		}

	}
}
