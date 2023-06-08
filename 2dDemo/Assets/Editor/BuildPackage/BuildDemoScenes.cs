using UnityEditor;
using UnityEngine;

namespace PlatformSdk.Editor
{
    class DemoConfig
    {
        public string scenePath;
        public string outputName;
    }

    public class BuildDemoScenes
    {
        public static void Build(BuildTarget buildTarget)
        {
            var sceneList = new[]
            {
                new DemoConfig() {scenePath = "./Assets/done/2048/Scene2048.unity", outputName = "2048第一版"},
                new DemoConfig() {scenePath = "./Assets/done/2048Better/Scene2048.unity", outputName = "2048第二版"},
                new DemoConfig() {scenePath = "./Assets/done/2048Dynamic/Scenes/GameScene.unity", outputName = "2048第三版"},
                new DemoConfig() {scenePath = "./Assets/done/万圣节/Main.unity", outputName = "万圣节"},
                new DemoConfig() {scenePath = "./Assets/done/扫雷/Main.unity", outputName = "扫雷"},
                new DemoConfig() {scenePath = "./Assets/done/拼图/Main.unity", outputName = "拼图"},
                new DemoConfig() {scenePath = "./Assets/done/贪吃蛇/Main.unity", outputName = "贪吃蛇"},
            };
            var initName = PlayerSettings.productName;
            try
            {
                foreach (var i in sceneList)
                {
                    Debug.Log($"正在编译:{i.scenePath}=>{i.outputName}");
                    PlayerSettings.productName = i.outputName;
                    if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
                    {
                        if (!i.outputName.EndsWith(".exe"))
                        {
                            i.outputName += ".exe";
                        }
                    }

                    BuildPipeline.BuildPlayer(new[]
                    {
                        new EditorBuildSettingsScene(i.scenePath, true)
                    }, $"gen/{buildTarget}/{i.outputName}", buildTarget, BuildOptions.Development);
                }
            }
            finally
            {
                PlayerSettings.productName = initName;
            }
        }
    }
}