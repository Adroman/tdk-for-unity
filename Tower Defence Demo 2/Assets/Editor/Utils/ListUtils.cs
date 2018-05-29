namespace Editor.Utils
{
    public static class ListUtils
    {
        public static float GetWaveListWidth(int maxClusters, float mainButtonWidth, float smallButtonWidth, float space, float iconWidth, bool includeScrollBar)
        {
            return space + maxClusters * (iconWidth + space) + mainButtonWidth + space + 2 * (smallButtonWidth + space) + (includeScrollBar ? 15 : 0);
        }

        public static float GetScrollHeight(int count, float space, float buttonHeight)
        {
            return space + count * (buttonHeight + space);
        }
    }
}
