using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 用于构建树结构列表
    /// 适用于使用 jstree 控件（https://github.com/vakata/jstree）
    /// </summary>
    public class TreeNodeForJsTree
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parent")]
        public string Parent { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("nodes")]
        public List<TreeNodeForJsTree> Nodes { get; set; } = new List<TreeNodeForJsTree>();

        public static TreeNodeForJsTree GetTreeRootNode(string id,string nodeText)
        {
            var node = new TreeNodeForJsTree();
            node.Parent = "#";
            node.Text = nodeText;
            node.Icon = "far fa-edits";
            return node;
        }

        public static TreeNodeForJsTree GetTreeRootNode(string id,SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNodeForJsTree();
            node.Id = id;
            node.Parent = "#";
            node.Text = selfReferentialItem.DisplayName;
            node.Icon = "far fa-edit";
            return node;
        }

    }
}
