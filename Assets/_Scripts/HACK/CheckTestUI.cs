using _Scripts.UI;
using UnityEngine;

namespace Assets._Scripts.HACK {
	class CheckTestUI : MonoBehaviour 
	{
		
		public void Awake() {
			int isCheckGame = PlayerPrefs.GetInt(StringPlayerPrefs.ISTESTGAME, 0);
			if (isCheckGame == 0) {
				this.gameObject.SetActive(false);
			}
		}
	}

}
