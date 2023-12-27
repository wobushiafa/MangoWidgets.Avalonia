using Avalonia.Controls;

namespace MangoWidgets.Avalonia.Extensions;

public static class ItemsControlExtensions
{
    /// <summary>
    /// 折叠Item
    /// </summary>
    /// <param name="itemsControl"></param>
    /// <param name="flag">Collapse Children</param>
    public static void CollapseAllItems(this ItemsControl itemsControl,bool flag = false)
    {
        var count = itemsControl.GetRealizedContainers();
        foreach (var control in itemsControl.GetRealizedContainers())
        {
            if(control is not TreeViewItem treeviewItem)
                continue;
            treeviewItem.IsExpanded = false;
            if (flag)
                CollapseAllItems(treeviewItem,flag);
        }
    }
    
    /// <summary>
    /// 展开Item
    /// </summary>
    /// <param name="itemsControl"></param>
    /// <param name="flag">Expand Children</param>
    public static void ExpandAllItems(this ItemsControl itemsControl,bool flag = false)
    {
        foreach (var control in itemsControl.GetRealizedContainers())
        {
            if(control is not TreeViewItem treeviewItem)
                continue;
            treeviewItem.IsExpanded = true;
            if (flag)
                ExpandAllItems(treeviewItem,flag);
        }
    }
}