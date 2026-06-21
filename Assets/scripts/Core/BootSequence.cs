using UnityEngine;

public class BootSequence : MonoBehaviour
{
    //[SerializeField] private string nextSceneName = "01_HorseSelect";
    [SerializeField] private string nextSceneName = "01_HorseSelect";       //遺伝的アルゴリズムのシーンに移動

    void Start()
    {
        SupabaseAuth.Instance.SignInAnonymously(
            onComplete: () => {
                Debug.Log("匿名サインイン完了。UserId: " + SupabaseAuth.Instance.UserId);
                SceneLoader.Instance.LoadScene(nextSceneName);
            },
            onError: (err) => {
                Debug.LogError("サインインに失敗したため起動を中断しました: " + err);
            }
        );
    }
}