using UnityEditor;

namespace Editor.Utils
{
    public class SceneUtils
    {
        [MenuItem("Tools/Tower defence kit/New Level")]
        public static void NewScene()
        {
            FileUtil.CopyFileOrDirectory("Assets/Basic.unity", "Assets/NewLevel.unity");


        }
    }
}