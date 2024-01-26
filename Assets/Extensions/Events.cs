using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireflyVR
{
	public class Events : Singleton<Events> {
		public static event System.Action update;
		public static event System.Action<bool> pause;

        private void Start()
        {
#if UNITY_EDITOR
            // simulate pausing in the editor
            if (Application.runInBackground)
                UnityEditor.EditorApplication.pauseStateChanged += x => OnApplicationPause(x == UnityEditor.PauseState.Paused);
#endif
        }

        private void Update() => update?.Invoke();

        private void OnApplicationPause(bool pauseState) => pause?.Invoke(pauseState);

        public static void Delay(float seconds, System.Action action) => i.AddDelayed(seconds, action);
	}
}
