using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulationStarter : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject geneticManagerPrefab;

    [Header("UI References")]
    [SerializeField] private Slider parameterSlider;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI p; // UI用のTextMeshProUGUIに変更
    [SerializeField] private GameObject canvasObject; // 非アクティブにするCanvas
    [SerializeField] private TextMeshProUGUI maxFitnessText; // 最大評価値表示用テキスト

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartSimulation);
        }
        else
        {
            Debug.LogWarning("Start Button is not assigned on SimulationStarter.");
        }

        if (parameterSlider != null)
        {
            // スライダーの値が変更された時にテキストをリアルタイム更新するイベントを登録
            parameterSlider.onValueChanged.AddListener(UpdateSliderText);
            // 初期表示を反映
            UpdateSliderText(parameterSlider.value);
        }
    }

    private void UpdateSliderText(float value)
    {
        if (p != null)
        {
            p.text = $"{value:F0}%"; // 小数点以下なしの%表記で表示
        }
    }

    public void StartSimulation()
    {
        if (geneticManagerPrefab == null)
        {
            Debug.LogError("GeneticManager Prefab is not assigned on SimulationStarter!");
            return;
        }

        // GeneticManagerの生成
        GameObject managerObj = Instantiate(geneticManagerPrefab);
        GeneticManager manager = managerObj.GetComponent<GeneticManager>();

        if (manager != null)
        {
            if (parameterSlider != null)
            {
                // 生成されたManagerにスライダーの参照を渡す
                manager.Slider = parameterSlider;
            }
            else
            {
                Debug.LogWarning("Parameter Slider is not assigned on SimulationStarter. GeneticManager will run with default mutation rate.");
            }

            if (canvasObject != null)
            {
                // 終了時に再表示できるようにCanvasの参照を渡す
                manager.canvasObject = canvasObject;
            }

            if (maxFitnessText != null)
            {
                // 最大評価値を表示できるようにテキストの参照を渡す
                manager.maxFitnessText = maxFitnessText;
            }
        }
        else
        {
            Debug.LogError("The instantiated prefab does not have a GeneticManager component!");
        }

        // ボタンが押されたら指定されたCanvasを非アクティブにする
        if (canvasObject != null)
        {
            canvasObject.SetActive(false);
        }
        else
        {
            // Canvasの指定がない場合は、このStarterアタッチオブジェクトを非アクティブにする
            gameObject.SetActive(false);
        }
    }
}
