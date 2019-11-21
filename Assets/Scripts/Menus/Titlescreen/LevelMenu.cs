using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : Menu {
	[Header("Sprites")]
	[SerializeField] Sprite uiBox = null;
	[SerializeField] Sprite uiBoxComp = null;

	Transform levels;

	void Start ( ) {
		levels = transform.Find("Levels");
		bool[ ] completedLevels = SaveManager.LoadGame( );

		for (int i = 0; i < levels.childCount; i++) {
			levels.GetChild(i).GetComponent<Image>( ).sprite = completedLevels[i] ? uiBoxComp : uiBox;
		}
	}
}
