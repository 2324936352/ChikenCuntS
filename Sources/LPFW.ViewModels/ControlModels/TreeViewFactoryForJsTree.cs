using LPFW.DataAccess;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModels.ControlModels
{
    public static class TreeViewFactoryForJsTree
    {
        public static List<TreeNodeForJsTree> GetTreeNodes(List<SelfReferentialItem> sourceItems)
        {
            var treeViewItems = new List<TreeNodeForJsTree>();
            foreach (var item in sourceItems)
            {
                var node = new TreeNodeForJsTree
                {
                    Id = item.ID,
                    Text = item.DisplayName
                };

                if (item.ID == item.ParentID)
                    node.Parent = "#";
                else
                    node.Parent = item.ParentID;

                treeViewItems.Add(node);
            }
            return treeViewItems;
        }


        /// <summary>
        /// 通过数据访问服务获取树节点集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityRepository"></param>
        /// <returns></returns>
        public static async Task<List<TreeNodeForJsTree>> GetTreeNodesAsyn<T>(IEntityRepository<T> entityRepository) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNodeForJsTree>();
            var sourceItems = await SelfReferentialItemFactory<T>.GetCollectionAsyn(entityRepository,false);
            //treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        public static async Task<List<TreeNodeForJsTree>> GetTreeNodesAsyn<T>(IEntityRepository<T> entityRepository,Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNodeForJsTree>();
            var sourceItems = await SelfReferentialItemFactory<T>.GetCollectionAsyn(entityRepository, predicate, false);
            //treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        /// <summary>
        /// 根据子引用的元素集合创建树节点集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static List<TreeNodeForJsTree> GetTreeNodesByrecursion(List<SelfReferentialItem> sourceItems)
        {
            var treeViewItems = new List<TreeNodeForJsTree>();
            var rootItems = sourceItems.Where(x => x.ParentID == x.ID || x.ParentID == "");
            foreach (var item in rootItems)
            {
                var treeNode = _GetRootTreeNode(item);
                treeViewItems.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }
            return treeViewItems;
        }

        /// <summary>
        /// 递归处理
        /// </summary>
        /// <param name="rootTreeNode"></param>
        /// <param name="rootSourceNode"></param>
        /// <param name="sourceItems"></param>
        private static void _GetSubNodes(TreeNodeForJsTree rootTreeNode, SelfReferentialItem rootSourceNode, List<SelfReferentialItem> sourceItems)
        {
            var subItems = sourceItems.Where(sn => sn.ParentID == rootSourceNode.ID && sn.ID != rootSourceNode.ParentID).OrderBy(o => o.SortCode);
            foreach (var item in subItems)
            {
                var treeNode = _GetSubTreeNode(item);
                rootTreeNode.Nodes.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }

        }

        private static TreeNodeForJsTree _GetRootTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNodeForJsTree
            {
                Id = selfReferentialItem.ID,
                Parent = "#",
                Text = selfReferentialItem.DisplayName,
                Icon = "glyphicons glyphicons-folder-minus"
            };
            return node;
        }

        private static TreeNodeForJsTree _GetSubTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNodeForJsTree
            {
                Id = selfReferentialItem.ID,
                Parent = "#",
                Text = selfReferentialItem.DisplayName,
                Icon = "glyphicons glyphicons-folder-minus"
            };
            return node;
        }

    }
}
