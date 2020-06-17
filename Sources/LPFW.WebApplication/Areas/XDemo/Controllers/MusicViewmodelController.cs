using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LPFW.EntitiyModels.XDemo;
using LPFW.ViewModels.ControlModels;
using LPFW.ViewModels.XDemo;
using LPFW.ViewModelServices;
using LPFW.WebApplication.BaseTemplateControllers;
using Microsoft.AspNetCore.Mvc;

namespace LPFW.WebApplication.Areas.XDemo.Controllers
{
    [Area("XDemo")]
    public class MusicViewmodelController : BaseTemplateController<MusicDemo, MusicViewmodel>
    {
      public MusicViewmodelController(IViewModelService<MusicDemo, MusicViewmodel> service) : base(service)
        {
            ModuleName = "常规实体对象 MusicDemo 数据管理";
            FunctionName = "MusicDemo";


            // 3. 设置列表视图参数
            ListItems = new List<TableListItem>()
            {
                new TableListItem() { PropertyName = "Name", DsipalyName="任务名称", Width=150, IsSort = false, SortDesc=""  },
                new TableListItem() { PropertyName = "Description", DsipalyName="简要说明", Width=200, IsSort = false  },
                new TableListItem() { PropertyName = "UndertakerName", DsipalyName = "任务承担人名称", IsSort = false, SortDesc = "", Width = 100 },
                new TableListItem() { PropertyName = "EndTime", DsipalyName = "结束日期", IsSort = false, SortDesc = "", Width = 100, DataType= ViewModelDataType.日期 },
                new TableListItem() { PropertyName = "IsFinished", DsipalyName="完成状态", Width=70, IsSort = false, SortDesc=""  }
            };
            // 4. 设置编辑和新建数据视图参数
            CreateOrEditItems = new List<CreateOrEditItem>()
            {
                new CreateOrEditItem() { PropertyName = "Name", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "Description", TipsString="", DataType = ViewModelDataType.多行文本 },
                new CreateOrEditItem() { PropertyName = "UndertakerName", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "EndTime", TipsString="", DataType = ViewModelDataType.日期 },
                new CreateOrEditItem() { PropertyName = "SortCode", TipsString="", DataType = ViewModelDataType.单行文本 },
                new CreateOrEditItem() { PropertyName = "IsFinished", TipsString="", DataType = ViewModelDataType.是否},
            };

          

        }
    }
}