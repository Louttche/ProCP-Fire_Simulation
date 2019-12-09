using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneChange
{
    void GoToMainScene();
    void GoToEditorScene(bool newMap);
    void GoToSimulationScene();
    void GoToResultsScene();

}
