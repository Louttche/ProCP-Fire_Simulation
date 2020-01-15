using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneChange
{
    void GoToMainScene();
    void GoToEditorScene();
    void GoToSimulationScene();
    void GoToResultsScene();

}
