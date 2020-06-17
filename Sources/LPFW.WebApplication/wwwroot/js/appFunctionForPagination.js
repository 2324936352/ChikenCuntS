/*
 * 提取页面分页规格数据,构建 ListParaJson 对象
 * @returns {} 
 */
function appGetPaginateListParaJson() {
    // 提取缺省的页面规格参数
    var lionPageTypeID = $("#lionTypeID").val();
    var lionPageTypeName = $("#lionTypeName").val();
    var lionPagePageIndex = $("#lionPageIndex").val();
    var lionPagePageSize = $("#lionPageSize").val();
    var lionPagePageAmount = $("#lionPageAmount").val();
    var lionPageObjectAmount = $("#lionObjectAmount").val();
    var lionPageKeyword = $("#lionKeyword").val();
    var lionPageSortProperty = $("#lionSortProperty").val();
    var lionPageSortDesc = $("#lionSortDesc").val();
    var lionPageSelectedObjectID = $("#lionSelectedObjectID").val();
    var lionPageIsSearch = $("#lionIsSearch").val();
    var lionOtherProperty01 = $("#lionOtherProperty01").val();
    var lionOtherProperty02 = $("#lionOtherProperty02").val();
    var lionOtherProperty03 = $("#lionOtherProperty03").val();

    // 创建前端 json 数据对象
    var listParaJson = "{" +
        "TypeID:\"" + lionPageTypeID + "\", " +
        "TypeName:\"" + lionPageTypeName + "\", " +
        "PageIndex:\"" + lionPagePageIndex + "\", " +
        "PageSize:\"" + lionPagePageSize + "\", " +
        "PageAmount:\"" + lionPagePageAmount + "\", " +
        "ObjectAmount:\"" + lionPageObjectAmount + "\", " +
        "Keyword:\"" + lionPageKeyword + "\", " +
        "SortProperty:\"" + lionPageSortProperty + "\", " +
        "SortDesc:\"" + lionPageSortDesc + "\", " +
        "IsSearch:\"" + lionPageIsSearch + "\", " +
        "SelectedObjectID:\"" + lionPageSelectedObjectID + "\"," +
        "OtherProperty01:\"" + lionOtherProperty01 + "\"," +
        "OtherProperty02:\"" + lionOtherProperty02 + "\"," +
        "OtherProperty03:\"" + lionOtherProperty03 + "\"" +
        "}";

    return listParaJson;
}

/*
 * 提取数据列表相关的参数，构成分页器数据，以 json 后向后端提交
 * @param {} urlString 后台 action 路径
 * @param {} mainWorkPlaceAreaId 主显示区域 Div 的 id
 */
function appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId) {
    var listParaJson = appGetPaginateListParaJson();
    var jsonData = { "listPageParaJson": listParaJson };

    $.ajax({
        cache: false,
        type: "get",
        async: true,
        url: urlString,
        data: jsonData,
        beforeSend: function () {
            document.getElementById(mainWorkPlaceAreaId).innerHTML = "<p style='margin-top:80px;' align='center'> <i class='fa fa-spinner fa-pulse fa-2x'></i></p>" +
                "<p style='margin-top:20px;' align='center'>  数据加载中，请稍候...</p>";
        }
    }).done(function (data) {
        document.getElementById(mainWorkPlaceAreaId).innerHTML = data;
        resetDivHeight();

    }).fail(function (jqXHR, textStatus, errorThrown) {
        // 调试时候观察错误
        console.error("调试错误:" + errorThrown);
    }).always(function () {
    });
}

/*
 * 使用分页器的，缺省的列表数据处理，通常用于在新建或编辑、明细或者其他操作处理后，返回到列表页面时候使用，以局部页返回检索结果到 mainWorkPlaceAreaId 指定的 div
 * @param {any} urlString
 * @param {any} mainWorkPlaceAreaId
 */
function appGotoDefaultPaginatePage(urlString,mainWorkPlaceAreaId) {
    appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId);
}

function appGotoDefaultPaginatePageForModal(urlString, mainWorkPlaceAreaId,modalId) {
    $('#' + modalId).modal('hide');
    appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId);
}

/*
 * 单击常规翻页器时触发的翻页方法
 * @param {} pageIndex 页码
 * @param {} urlString 后台 action 路径
 * @param {} mainWorkPlaceAreaId 主显示区域 Div 的 id
 */
function appGotoPaginatePage(pageIndex, urlString, mainWorkPlaceAreaId) {
    $('#lionPageIndex').val(pageIndex);
    appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId);
}

/*
 * 根据列表元素关联的类型处理列表数据的方法
 * @param {} id 类型 id
 * @param {} urlString 后台 action 路径
 * @param {} mainWorkPlaceAreaId 主显示区域 Div 的 id
 */
function appGotoTypePaginatePage(id, urlString, mainWorkPlaceAreaId) {
    $('#lionTypeID').val(id);
    appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId);
}

/*
 * 根据关键词检索数据，使用分页器，以局部页返回检索结果到 mainWorkPlaceAreaId 指定的 div
 * @param {any} keyword
 * @param {any} urlString
 * @param {any} mainWorkPlaceAreaId
 */
function appGotoSearchPaginatePage(keyword, urlString, mainWorkPlaceAreaId) {
    $('#lionKeyword').val(keyword);
    $('#lionPageIndex').val('1');
    $('#lionSortDesc').val('');
    appGotoPaginateListWithJson(urlString, mainWorkPlaceAreaId);
}


/*
 * 使用 ajax 来提交 Form 数据 
 * @param {any} formId
 * @param {any} mainWorkPlaceAreaId
 */
function appPaginateSubmitCreateOrEditForm(formId,defaultString,mainWorkPlaceAreaId) {
    var boVMCreateFormOptions = {
        success: function (data) {
            document.getElementById(mainWorkPlaceAreaId).innerHTML = data;
            // 检查是否保存成功，保存成功的话跳转回到列表页
            var isSaved = $('#IsSaved').val();
            if (isSaved === "true") {
                appGotoDefaultPaginatePage(defaultString, mainWorkPlaceAreaId);
            }
            // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
            initialGotoCreateOrEditWithPartialView();
        }
    };
    $('#'+formId).ajaxSubmit(boVMCreateFormOptions);
}

/*
 * 使用 ajax 来提交 Form 数据 
 * @param {any} formId
 * @param {any} mainWorkPlaceAreaId
 * @param {any} modalId 模态会话框的 id
 * @param {any} modalContentId 模态会话框中需要显示 form 内容区域
 */
function appPaginateSubmitCreateOrEditFormForModal(formId,defaultString,mainWorkPlaceAreaId,modalId,modalContentId) {
    var boVMCreateFormOptions = {
        success: function (data) {
            document.getElementById(modalContentId).innerHTML = data;
            // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
            initialGotoCreateOrEditWithPartialView();

            // 检查是否保存成功，保存成功的话跳转回到列表页
            var isSaved = $('#IsSaved').val();
            if (isSaved === "true") {
                $('#' + modalId).modal('hide');
                appGotoDefaultPaginatePage(defaultString,mainWorkPlaceAreaId);
            }
        }
    };
    $('#'+formId).ajaxSubmit(boVMCreateFormOptions);
}

/*
 * 对于列表元素执行实际的删除
 */
function appGotoDeletePaginateBo() {
    var boVMID = $('#businessObjectID').val();
    var deleteUrl = $('#deleteUrlString').val();
    var urlString = $('#defaultUrlString').val();
    var mainWorkPlaceAreaId = $('#mainWorkPlaceAreaId').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: deleteUrl + boVMID,
        beforeSend: function () {
        }
    }).done(function (delStatus) {
        if (delStatus.deleteSatus === true) {
            $('#deleteItemModal').modal('hide');
            // 刷新列表页
            appGotoDefaultPaginatePage(urlString,mainWorkPlaceAreaId);
        } else {
            document.getElementById("deleteModalErrMessage").innerText = delStatus.message;
        }
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 根据指定的 div 和节点数据，初始化 BootStrapTreeView 插件
 * @param {any} divId 待初始化的 div 的标识
 * @param {any} dataUrl 获取节点数据集合的后端服务数据路径
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appInitialBootStrapTreeViewForPagination(divId, dataUrl, defaultString, mainWorkPlaceAreaId) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: dataUrl,
        beforeSend: function () {
        }
    }).done(function (dataReusult) {
        $('#'+divId).treeview({
            levels: 2,
            expandIcon: 'fa fa-angle-down',
            collapseIcon: 'fa fa-angle-right',
            emptyIcon: 'fa',
            nodeIcon: 'fa fa-list',
            selectedIcon: 'fa fa-list-alt',
            showBorder: false,
            data: dataReusult,
            onNodeSelected: function (event, node) {
                // 提取类型 Id，跳转至列表局部视图
                appGotoTypePaginatePage(node.id, defaultString, mainWorkPlaceAreaId);
            }
        });

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}
