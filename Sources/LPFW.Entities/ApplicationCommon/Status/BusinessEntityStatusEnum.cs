namespace LPFW.EntitiyModels.BusinessCommon.Status
{
    /// <summary>
    /// 用于定义业务对象状态
    /// </summary>
    public enum BusinessEntityStatusEnum
    {
        内建,
        挂起, 
        编辑中,
        进行中,
        待审核,
        变更,  
        移交,
        停止,
        作废,
        批准,
        完成 
    }
}