using PlatformSdk.Editor;
using UnityEditor;

namespace Editor
{
    public class MenuManager
    {
        [MenuItem("mine/一键编译")]
        public static void buildAllGames()
        {
            BuildDemoScenes.Build();
        }
    }
}