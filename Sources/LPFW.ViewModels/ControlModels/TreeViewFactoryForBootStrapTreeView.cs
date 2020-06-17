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
    public class TreeViewFactoryForBootSrapTreeView
    {
        /// <summary>
        /// 通过数据访问服务获取树节点集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityRepository"></param>
        /// <returns></returns>
        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeNodesAsyn<T>(IEntityRepository<T> entityRepository) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNodeForBootStrapTreeView>();
            var sourceItems = await SelfReferentialItemFactory<T>.GetCollectionAsyn(entityRepository, false);
            treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        public static async Task<List<TreeNodeForBootStrapTreeView>> GetTreeNodesAsyn<T>(IEntityRepository<T> entityRepository, Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNodeForBootStrapTreeView>();
            var sourceItems = await SelfReferentialItemFactory<T>.GetCollectionAsyn(entityRepository, predicate, false);
            treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        /// <summary>
        /// 根据子引用的元素集合创建树节点集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static List<TreeNodeForBootStrapTreeView> GetTreeNodes(List<SelfReferentialItem> sourceItems)
        {
            var treeViewItems = new List<TreeNodeForBootStrapTreeView>();
            var rootItems = sourceItems.Where(x => x.ParentID == x.ID || x.ParentID == "");
            foreach (var item in rootItems)
            {
                var treeNode = _GetTreeNode(item);
                treeViewItems.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }
            return treeViewItems;
        }

        public static List<TreeNodeForBootStrapTreeView> GetTreeNodes(List<PlainFacadeItem> sourceItems)
        {
            var treeViewItems = new List<TreeNodeForBootStrapTreeView>();
            foreach (var item in sourceItems)
            {
                var treeNode = _GetTreeNode(item);
                treeViewItems.Add(treeNode);
            }
            return treeViewItems;
        }

        /// <summary>
        /// 递归处理
        /// </summary>
        /// <param name="rootTreeNode"></param>
        /// <param name="rootSourceNode"></param>
        /// <param name="sourceItems"></param>
        private static void _GetSubNodes(TreeNodeForBootStrapTreeView rootTreeNode, SelfReferentialItem rootSourceNode, List<SelfReferentialItem> sourceItems)
        {
            var subItems = sourceItems.Where(sn => sn.ParentID == rootSourceNode.ID && sn.ID != rootSourceNode.ParentID).OrderBy(o => o.SortCode);
            foreach (var item in subItems)
            {
                var treeNode = _GetTreeNode(item);
                rootTreeNode.Nodes.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }

        }

        private static TreeNodeForBootStrapTreeView _GetTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNodeForBootStrapTreeView();
            node.Id = selfReferentialItem.ID;
            node.Text = selfReferentialItem.DisplayName;
            node.Href = "";
            return node;
        }

        private static TreeNodeForBootStrapTreeView _GetTreeNode(PlainFacadeItem plainFacadeItem)
        {
            var node = new TreeNodeForBootStrapTreeView();
            node.Id     = plainFacadeItem.ID;
            node.Text   = plainFacadeItem.DisplayName;
            node.Href = "";
            return node;
        }

    }
}
