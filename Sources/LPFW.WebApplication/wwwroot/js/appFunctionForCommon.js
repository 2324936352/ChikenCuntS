/**
 * 提取数据列表相关的参数，构成分页器数据，以 json 后向后端提交
 * @param {any} urlString 后台 action 路径
 * @param {any} mainWorkPlaceAreaId 主显示区域 Div 的 id
 */
function appGotoDefaultCommonPage(urlString, mainWorkPlaceAreaId) {
    var listParaJson = appGetListSinglePageParameterJson();
    var jsonData = { "listPageParaJson": listParaJson };
    $.ajax({
        cache: false,
        type: "post",
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

/**
 * 关闭指定的模态会话框，返回到缺省的试图数据列表
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 * @param {any} modalId 会话框模态框的 id
 */
function appGotoDefaultCommonPageForModal(defaultString, mainWorkPlaceAreaId, modalId) {
    // 关闭模态框
    $('#' + modalId).modal('hide');
    appGotoDefaultCommonPage(defaultString,mainWorkPlaceAreaId);
}


/**
 * 普通的根据关键词检索数据，以局部页返回检索结果到 mainWorkPlaceAreaId 指定的 div
 * @param {any} keyword 关键词
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appGotoCommonSearchPage(keyword, defaultString, mainWorkPlaceAreaId) {
    $('#lionKeyword').val(keyword);
    appGotoDefaultCommonPage(defaultString,mainWorkPlaceAreaId);
}

/**
 * 普通的根据所选类型的 id，以局部页返回检索结果到 mainWorkPlaceAreaId 指定的 div
 * 通常这个方法在 initialBootStrapTreeViewForCommon(divId,dataUrl) 或者 initialjsTree 中调用
 * @param {any} typeId 类型数据对象 Id
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appGotoCommonTypePage(typeId, defaultString, mainWorkPlaceAreaId) {
    //alert(typeId);
    $('#lionTypeID').val(typeId);
    appGotoDefaultCommonPage(defaultString,mainWorkPlaceAreaId);
}

/**
 * 根据排序属性和排序方向，以局部页方式返回数据处理结果
 * @param {any} sortProperty 排序属性
 * @param {any} sortDesc 排序方向
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appGotoCommonSortPage(sortProperty,sortDesc, defaultString, mainWorkPlaceAreaId) {
    $('#lionSortProperty').val(sortProperty);
    $('#lionSortDesc').val(sortDesc);
    appGotoDefaultCommonPage(defaultString,mainWorkPlaceAreaId);
}


/**
 * 使用 ajax 来提交 Form 数据 
 * @param {any} formId 表单的 id
 * @param {any} defaultString 表单提交成功后获取缺省视图模型数据渲染局部视图的路径
 * @param {any} mainWorkPlaceAreaId 缺省视图模型数据渲染局部视图所在的 div 的 Id
 */
function appCommonSubmitCreateOrEditForm(formId,defaultString,mainWorkPlaceAreaId) {
    var boVMCreateFormOptions = {
        success: function (data) {
            document.getElementById(mainWorkPlaceAreaId).innerHTML = data;
            // 检查是否保存成功，保存成功的话跳转回到列表页
            var isSaved = $('#IsSaved').val();
            if (isSaved === "true") {
                // 成功的话就跳转回到列表视图
                appGotoDefaultCommonPage(defaultString, mainWorkPlaceAreaId);
            }
            // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，
            //（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
            initialGotoCreateOrEditWithPartialView();
        }
    };
    $('#'+formId).ajaxSubmit(boVMCreateFormOptions);
}

/**
 * 在模态会话框中使用 ajax 来提交 Form 数据
 * @param {string} formId 表单的 id
 * @param {any} defaultString 表单提交成功后获取缺省视图模型数据渲染局部视图的路径
 * @param {any} mainWorkPlaceAreaId 缺省视图模型数据渲染局部视图所在的 div 的 Id
 * @param {any} modalId 模态会话框的 id
 * @param {any} modalContentId 模态会话框中需要显示 form 内容区域
 */      
function appCommonSubmitCreateOrEditFormForModal(formId, defaultString, mainWorkPlaceAreaId, modalId, modalContentId) {
    var boVMCreateFormOptions = {
        success: function (data) {
            document.getElementById(modalContentId).innerHTML = data;

            // 检查是否保存成功，保存成功的话跳转回到列表页
            var isSaved = $('#IsSaved').val();
            if (isSaved === "true") {
                $('#' + modalId).modal('hide');
                appGotoDefaultCommonPage(defaultString,mainWorkPlaceAreaId);
            }

            // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
            initialGotoCreateOrEditWithPartialView();
        }
    };

    $('#' + formId).ajaxSubmit(boVMCreateFormOptions);
}

/**
 * 对于列表元素执行实际的删除
 */
function appGotoDeleteCommonBo() {
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
            appGotoDefaultCommonPage(urlString,mainWorkPlaceAreaId);
        } else {
            document.getElementById("deleteModalErrMessage").innerText = delStatus.message;
        }
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 提取页面规格数据,构建 ListParaJson 对象
 * @returns {string}  以字符串方式构成的 listParaJson 参数
 */
function appGetListSinglePageParameterJson() {
    // 提取缺省的页面规格参数
    var lionPageTypeID = $("#lionTypeID").val();
    var lionPageTypeName = $("#lionTypeName").val();
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

/**
 * 根据指定的 div 和节点数据，初始化 BootStrapTreeView 插件
 * @param {any} divId 待初始化的 div 的标识
 * @param {any} dataUrl 获取节点数据集合的后端服务数据路径
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appInitialBootStrapTreeViewForCommon(divId, dataUrl, defaultString, mainWorkPlaceAreaId) {
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
                appGotoCommonTypePage(node.id, defaultString, mainWorkPlaceAreaId);
            }
        });

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 根据指定的 div 和节点数据，初始化 jsTreeView 插件
 * @param {any} divId 待初始化的 div 的标识
 * @param {any} dataUrl 获取节点数据集合的后端服务数据路径
 * @param {any} defaultString 获取缺省列表视图数据的后端 url
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */
function appInitialjsTreeForCommon(divId,dataUrl, defaultString, mainWorkPlaceAreaId) {
    // 提取树节点数据
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: dataUrl,
        beforeSend: function () {
        }
    }).done(function (dataReusult) {
        $('#'+divId).jstree({
            "core": {
                "themes" : { "stripes" : false },
                "data": dataReusult
            },
            "types": {
                "default": {
                    "icon": {
                        "image":""
                    }
                }
            }
        });
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
    // 处理单击事件
    $('#' + divId).on("changed.jstree", function (e, data) {
        // 提取类型 Id，跳转至列表局部视图
        appGotoCommonTypePage(node.id, defaultString, mainWorkPlaceAreaId);
    });
}
