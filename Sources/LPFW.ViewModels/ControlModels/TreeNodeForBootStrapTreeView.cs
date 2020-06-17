using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 适用于 BootTreeView 的树节点模型：https://github.com/jonmiles/bootstrap-treeview
    /// </summary>
    public class TreeNodeForBootStrapTreeView
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("selectedIcon")]
        public string SelectedIcon { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("nodes")]
        public List<TreeNodeForBootStrapTreeView> Nodes { get; set; } = new List<TreeNodeForBootStrapTreeView>();

        public static TreeNodeForBootStrapTreeView GetTreeNode(string nodeText)
        {
            var node = new TreeNodeForBootStrapTreeView();
            node.Text = nodeText;
            node.Icon = "glyphicons glyphicons-folder-minus";
            node.SelectedIcon = "glyphicons glyphicons-folder-plus";
            return node;
        }

        public static TreeNodeForBootStrapTreeView GetTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNodeForBootStrapTreeView();
            node.Text = selfReferentialItem.DisplayName;
            node.Icon = "glyphicons glyphicons-folder-minus";
            //node.SelectedIcon = "glyphicons glyphicons-folder-plus";
            //node.Href = "gotoTypePage(\"" + selfReferentialItem.ID + "\")";
            return node;
        }
    }
}
