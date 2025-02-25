using UnityEngine;
using Steamworks;

public class SteamOverlay : MonoBehaviour
{
	public static SteamOverlay Instance;
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	private void OnEnable() {
		if (SteamManager.Initialized) {
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		}
	}

    private void OnDestroy() {
		if (SteamManager.Initialized) {
			if (m_GameOverlayActivated != null)
				m_GameOverlayActivated.Dispose();
		}
	}

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
		if (pCallback.m_bActive != 0) {
#if UNITY_EDITOR
			Debug.Log("Steam Overlay has been activated");
#endif
		}
		else {
#if UNITY_EDITOR
			Debug.Log("Steam Overlay has been closed");
#endif
		}
	}

	void Start() {
		if(Instance != null) {
			Destroy(gameObject);
			return;
        }
		Instance = this;
		DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
		if (SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();
			Debug.Log(name);
			//Debug.Log()
			Debug.Log("Does"+ (SteamApps.BIsSubscribedApp((AppId_t)374320)?" ":"n't ") + "Own Dark Souls 3");
		}
#endif
	}

}
