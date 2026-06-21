using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    [SerializeField] private Button transitionButton;
    [SerializeField] private string targetSceneName = "Race_Track01"; // 遷移先シーン名（デフォルト: Race_Track01）

    void Start()
    {
        // アタッチされていない場合は自身から取得を試みる
        if (transitionButton == null)
        {
            transitionButton = GetComponent<Button>();
        }

        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(OnTransitionButtonClick);
        }
        else
        {
            Debug.LogWarning("Transition Button is not assigned or found on this GameObject.");
        }
    }

    private void OnTransitionButtonClick()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target Scene Name is empty!");
            return;
        }

        // SceneLoaderが存在する場合はそれを使用し、存在しない場合は直接ロードする
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene(targetSceneName);
        }
        else
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
