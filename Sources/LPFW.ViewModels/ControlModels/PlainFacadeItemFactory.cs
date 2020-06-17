using LPFW.DataAccess;
using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModels.ControlModels
{
    /// <summary>
    /// 用于将一些需要的对象集合转换为简单的 PlainFacdeItem 集合或者个体的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PlainFacadeItemFactory<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// 根据泛型类型指定的对象来提取数据库中对应的全部对象，并转换为 PlainFacdeItem 集合
        /// </summary>
        /// <returns></returns>
        public static async Task<List<PlainFacadeItem>> GetAsyn(IEntityRepository<T> boRepository)
        {
            var sourceItems = await boRepository.GetBoCollectionAsyn();

            var items = new List<PlainFacadeItem>();
            foreach (var pItem in sourceItems.OrderBy(s => s.SortCode))
            {
                var item = new PlainFacadeItem()
                {
                    ID = pItem.Id.ToString(),
                    Name = pItem.Name,
                    DisplayName = pItem.Name,
                    Description = pItem.Description,
                    SortCode = pItem.SortCode
                };
                items.Add(item);
            }
            return items;

        }

        /// <summary>
        /// 将已经提取的对应的泛型集合对象，直接转换为 PlainFacdeItem 集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> Get(List<T> sourceItems)
        {
            var items = new List<PlainFacadeItem>();
            foreach (var pItem in sourceItems)
            {
                var item = new PlainFacadeItem()
                {
                    ID = pItem.Id.ToString(),
                    Name = pItem.Name,
                    DisplayName = pItem.Name,
                    Description = pItem.Description,
                    SortCode = pItem.SortCode
                };
                items.Add(item);
            }
            return items;

        }

        /// <summary>
        /// 将泛型类型中指定的枚举类型转换为 PlainFacdeItem 集合
        /// </summary>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetByEnum(Object enumObject)
        {
            var enumItems = Enum.GetValues(enumObject.GetType());
            var items = new List<PlainFacadeItem>();
            foreach (var eItem in enumItems)
            {
                var item = new PlainFacadeItem()
                {
                    ID = eItem.ToString(),// ((int)eItem).ToString(),
                    Name = eItem.ToString(),
                    DisplayName = eItem.ToString()
                };
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// 将布尔类型，转换为 PlainFacadeItem 集合，并提供缺省值设置
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetByBool(bool defaultValue)
        {
            var t = new PlainFacadeItem() { ID = "true", Name = "是", DisplayName = "是" };
            var f = new PlainFacadeItem() { ID = "false", Name = "否", DisplayName = "否" };

            if (defaultValue)
                t.OperateName = "checked";
            else
                f.OperateName = "checked";

            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        public static List<PlainFacadeItem> GetByBool()
        {
            var t = new PlainFacadeItem() { ID = "true", Name = "是", DisplayName = "是", OperateName = "" };
            var f = new PlainFacadeItem() { ID = "false", Name = "否", DisplayName = "否", OperateName = "" };
            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        /// <summary>
        /// 处理男女，将布尔类型，转换为 PlainFacadeItem 集合，并提供缺省值设置
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetBySex(bool defaultValue)
        {
            var t = new PlainFacadeItem() { ID = "true", Name = "男", DisplayName = "男" };
            var f = new PlainFacadeItem() { ID = "false", Name = "女", DisplayName = "女" };

            if (defaultValue)
                t.OperateName = "checked";
            else
                f.OperateName = "checked";

            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        public static List<PlainFacadeItem> GetBySex()
        {
            var t = new PlainFacadeItem() { ID = "true", Name = "男", DisplayName = "男", OperateName = "checked" };
            var f = new PlainFacadeItem() { ID = "false", Name = "女", DisplayName = "女", OperateName = "" };
            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }
    }
}
