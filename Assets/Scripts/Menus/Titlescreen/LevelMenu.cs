using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : Menu {
	[Header("Sprites")]
	[SerializeField] Sprite uiBox = null;
	[SerializeField] Sprite uiBoxComp = null;

	Transform levels;

	#region Unity Methods

	void Start ( ) {
		levels = transform.Find("Levels");
		bool[ ] completedLevels = SaveManager.LoadLevels( );

		for (int i = 0; i < levels.childCount; i++) {
			levels.GetChild(i).GetComponent<Image>( ).sprite = completedLevels[i] ? uiBoxComp : uiBox;
		}
	}

	#endregion

	#region Methods

	public void DeleteSave ( ) {
		File.Delete(Constants.PATH_LEVELS);
	}

	#endregion
}
