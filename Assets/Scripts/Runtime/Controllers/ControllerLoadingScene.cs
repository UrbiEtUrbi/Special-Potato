using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerLoadingScene : MonoBehaviour
{

    [SerializeField, SceneDetails]
    SerializedScene Scene;
    void Start()
    {
        ControllerGameFlow.Instance.LoadNewScene(Scene.BuildIndex);
    }
}
