using PlatformSdk.Editor;
using UnityEditor;

namespace Editor
{
    public class MenuManager
    {
        [MenuItem("mine/一键编译Mac")]
        public static void buildAllGames()
        {
            BuildDemoScenes.Build(BuildTarget.StandaloneOSX);
        }

        [MenuItem("mine/一键编译web")]
        public static void buildAllGamesForWeb()
        {
            BuildDemoScenes.Build(BuildTarget.WebGL);
        }

        [MenuItem("mine/一键编译Windows")]
        public static void buildWindows()
        {
            BuildDemoScenes.Build(BuildTarget.StandaloneWindows);
        }
    }
}