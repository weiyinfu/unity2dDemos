using System;
using UnityEngine;
using UnityEngine.UI;

public class UseUi : MonoBehaviour
{
    // Start is called before the first frame update
    private Button printInfo;
    private Button printError;
    private Button printWarning;
    private int clickCount = 0;
    private Text myText;

    void Start()
    {
        printInfo = GameObject.Find("打印Info").GetComponent<Button>();
        printError = GameObject.Find("打印error").GetComponent<Button>();
        printWarning = GameObject.Find("打印warning").GetComponent<Button>();
        myText = GameObject.Find("天下大势，为我所控").GetComponent<Text>();
        var button = GameObject.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            print("button is clicked");
            this.clickCount++;
            myText.text = $"点击按钮次数{this.clickCount}";
        });
        myText.text = "先有印福后有天，天不生印福，万古如长夜。";
        print($@"
printInfo={printInfo}
printError={printError}
printWarning={printWarning}
");
        printInfo.onClick.AddListener(delegate()
        {
            Debug.Log("Info");
            this.clickCount++;
            myText.text = String.Format("点击按钮次数{0}", this.clickCount);
        });
        printError.onClick.AddListener(delegate()
        {
            Debug.LogError("Error");
            this.clickCount++;
            myText.text = String.Format("点击按钮次数{0}", this.clickCount);
        });
        printWarning.onClick.AddListener(delegate()
        {
            Debug.LogWarning("warning");
            this.clickCount++;
            myText.text = String.Format("点击按钮次数{0}", this.clickCount);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}