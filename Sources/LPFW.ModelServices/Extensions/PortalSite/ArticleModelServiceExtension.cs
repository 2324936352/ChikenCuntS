using LPFW.DataAccess.Tools;
using LPFW.EntitiyModels.ApplicationCommon.RoleAndUser;
using LPFW.EntitiyModels.PortalSite;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.PortalSite;
using LPFW.ViewModelServices.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LPFW.ViewModelServices.Extensions.PortalSite
{
    public static class ArticleModelServiceExtension
    {
        public static async Task<List<ArticleVM>> GetArticleVMCollection(
            this IViewModelService<Article, ArticleVM> service, 
            ListPageParameter listPageParameter, 
            Expression<Func<ArticleInTopic, bool>> predicate, 
            params Expression<Func<ArticleInTopic, object>>[] includeProperties)
        {
            var boVMCollection = new List<ArticleVM>();
            var articleCollection = await service.EntityRepository.GetOrtherBoPaginateAsyn<ArticleInTopic>(listPageParameter,predicate, includeProperties);
            if (articleCollection.Count() > 0) 
            {
                var tempCollection = articleCollection.Select(x => x.MasterArticle);

                // 初始化视图模型起始序号
                int count = (int.Parse(listPageParameter.PageIndex) - 1) * int.Parse(listPageParameter.PageSize);
                foreach (var bo in tempCollection)
                {
                    var boVM = new ArticleVM();
                    bo.MapToViewModel<Article, ArticleVM>(boVM);
                    boVM.OrderNumber = (++count).ToString();

                    if (bo.CreatorUser != null)
                    {
                        boVM.CreatorUserId   = bo.CreatorUser.Id;
                        boVM.CreatorUserName = bo.CreatorUser.Name;
                    }

                    boVMCollection.Add(boVM);
                }
            }
            return boVMCollection;
        }


        public static async Task GetboVMRelevanceData(this IViewModelService<Article, ArticleVM> service, ArticleVM boVM)
        {
            var boId = boVM.Id;

            var article= await service.EntityRepository.GetOtherBoAsyn<ArticleInTopic>(x => x.MasterArticle.Id == boId, i => i.MasterArticle, i => i.MasterArticle.CreatorUser, i => i.ArticleTopic);
            var bo = new Article();
            if (article != null)
                bo = article.MasterArticle;

            if (article != null)
            {
                boVM.ArticleTopicId   = article.ArticleTopic.Id.ToString();
                boVM.ArticleTopicName = article.ArticleTopic.Name;
            }
            var sourceItems = await service.EntityRepository.GetOtherBoCollectionAsyn<ArticleTopic>();
            boVM.ArticleTopicItemCollection = PlainFacadeItemFactory<ArticleTopic>.Get(sourceItems.OrderBy(x => x.SortCode).ToList());

            if (bo.CreatorUser != null)
            {
                boVM.CreatorUserId = bo.CreatorUser.Id;
                boVM.CreatorUserName = bo.CreatorUser.Name;
            }
        }

        public static async Task<SaveStatusModel> SaveBoWithRelevanceDataAsyn(this IViewModelService<Article, ArticleVM> service, ArticleVM boVM)
        {
            var topicId = Guid.Parse(boVM.ArticleTopicId);
            var article = await service.EntityRepository.GetOtherBoAsyn<ArticleInTopic>(
                x => x.MasterArticle.Id == boVM.Id && x.ArticleTopic.Id==topicId, 
                i => i.MasterArticle, 
                i => i.MasterArticle.CreatorUser,
                i => i.ArticleTopic);

            // 根据视图模型的 Id 获取实体对象
            //var bo = await service.EntityRepository.GetBoAsyn(boVM.Id);
            if (article == null)
                article = new ArticleInTopic() { MasterArticle = new Article(), ArticleTopic = await service.EntityRepository.GetOtherBoAsyn<ArticleTopic>(topicId) };

            // 将视图模型的数据映射到实体模型
            boVM.MapToEntityModel<ArticleVM, Article>(article.MasterArticle);

            if (boVM.CreatorUserId != Guid.Empty)
            {
                var item = await service.EntityRepository.GetOtherBoAsyn<ApplicationUser>(boVM.CreatorUserId);
                article.MasterArticle.CreatorUser = item;
            }

            // 执行持久化处理
            var saveStatus = await service.EntityRepository.SaveOtherBoWithStatusAsyn(article);

            return saveStatus;
        }

    }
}
