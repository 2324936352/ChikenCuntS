/**
 *  根据指定的地址直接链接跳转回退
 * @param {any} urlString 指定跳转的路径
 */
function appGotoNewPage(urlString) {
    window.location.href = urlString;
}

/**
 * 使用局部页渲染编辑或新建数据对象的 UI ,UI 的具体体现，由控制器相应的方法返回的视图决定
 * @param {any} id 视图模型对象的 id
 * @param {any} urlString 后台 action 的完整访问路径
 * @param {any} mainWorkPlaceAreaId 主显示区域 Div 的 id
 */     
function appGotoCreateOrEditWithPartialView(id, urlString, mainWorkPlaceAreaId) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(mainWorkPlaceAreaId).innerHTML = data;
        // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，
        //（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialGotoCreateOrEditWithPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 使用局部页渲染编辑或新建数据对象的 UI ,UI 的具体体现，由控制器相应的方法返回的视图决定
 * @param {any} id 类型 id
 * @param {any} urlString 后台 action 路径
 * @param {any} mainWorkPlaceAreaId 主显示区域 Div 的 id
 * @param {any} province 默认地址之province
 * @param {any} city 默认地址之city
 * @param {any} county 默认地址之county
 */
function appGotoCreateOrEditWithPartialViewWithAddress(id, urlString, mainWorkPlaceAreaId, province, city, county) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(mainWorkPlaceAreaId).innerHTML = data;
        // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialGotoCreateOrEditWithPartialViewWithAddress(province, city, county);

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 根据指定的类型 id 获取包含有指定类型的 CreateOrEdit 局部视图，缺省要求 action 方法的参数表为...(Guid id, Guid typeId)
 * @param {any} id 类型 Id
 * @param {any} urlString 获取视图的 action 路径
 * @param {any} mainWorkPlaceAreaId 局部视图渲染的 div 的 id
 */
function appGotoCreateOrEditByTypeIdWithPartialView(id, urlString, mainWorkPlaceAreaId) {
    // 获取类型 Id
    var typeId= $('#lionTypeID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id + "?typeId=" + typeId,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(mainWorkPlaceAreaId).innerHTML = data;

        // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialGotoCreateOrEditWithPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}


/**
 * 为编辑和新建视图模型数据打开模态框，模态框代码文件：_DataOperationModal
 * @param {any} id              视图模型标识
 * @param {any} urlString       获取编辑或新建数据的局部视图 
 * @param {any} modalId         模态组件标识
 * @param {any} modalContentId  模态组件内容区域标识
 */
function appGotoCreateOrEditWithModal(id, urlString, modalId, modalContentId) {
    // 打开模态框
    $('#' + modalId).modal({
        show: true,
        backdrop: 'static'
    });

    // 执行向后台请求数据，并返回相关数据，根据数据的情况，渲染模态框的 dataOperationModalContent
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(modalContentId).innerHTML = data;
        // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialGotoCreateOrEditWithPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appGotoCreateOrEditByTypeWithModal(id, urlString, modalId, modalContentId) {
    // 获取类型 Id
    var typeId = $('#lionTypeID').val();
    // 打开模态框
    $('#' + modalId).modal({
        show: true,
        backdrop: 'static'
    });

    // 执行向后台请求数据，并返回相关数据，根据数据的情况，渲染模态框的 dataOperationModalContent
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id + "?typeId=" + typeId,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(modalContentId).innerHTML = data;
        // 初始化控件,这方法需要定义在 Index 页面，具体的执行内容，可以根据不同的控制器对应的值进行独立的处理，（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialGotoCreateOrEditWithPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}


/**
 * 使用局部页渲染数据对象详细数据的 UI ,UI 的具体体现，由控制器相应的方法返回的视图决定
 * @param {any} id 数据对象 id
 * @param {any} urlString 获取对象明细数据的后台 action 路径
 * @param {any} mainWorkPlaceAreaId  主显示区域 Div 的 id
 */
function appGotoDetailPartialView(id, urlString, mainWorkPlaceAreaId) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(mainWorkPlaceAreaId).innerHTML = data;

        // 初始化控件（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialDetailPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 在指定的会话模态框中，使用局部视图渲染数据对象详细数据的 UI,UI 的具体体现，由控制器相应的方法返回的视图决定
 * @param {any} id 数据对象 id 
 * @param {any} urlString 获取对象明细数据的后台 action 路径
 * @param {any} modalId 会话模态框的 Id
 * @param {any} modalContentId 用于承载局部视图渲染结果的内容 div 的 Id
 */
function appGotoDetailPartialModal(id, urlString, modalId, modalContentId) {
    // 打开模态框
    $('#' + modalId).modal({
        show: true,
        backdrop: 'static'
    });
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: urlString + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 渲染局部页
        document.getElementById(modalContentId).innerHTML = data;
        // 初始化控件（如果在 UI 上使用了第三方的控件，需要进行处理的，要在这里做一些初始化处理）
        initialDetailPartialView();

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 打开删除元素的提示对话框 
 * @param {any} modalId 待打开的会话模态框的 id
 * @param {any} itemId 待删除的数据元素的 Id
 * @param {any} message 提示信息
 * @param {any} deleteUrlString 后端执行删除操作的 Url
 * @param {any} defaultUrlString 执行删除操作以后，返回的缺省页面对应的 action 路径
 * @param {any} mainWorkPlaceAreaId 主显示区 div 的 id
 */      
function appOpenDeleteItemModal(modalId, itemId, message,deleteUrlString,defaultUrlString,mainWorkPlaceAreaId) {
    $('#' + modalId).modal({
        show: true,
        backdrop: 'static'
    });
    document.getElementById("deleteModalMessage").innerHTML = message;
    $('#businessObjectID').val(itemId);
    $('#deleteUrlString').val(deleteUrlString);
    $('#defaultUrlString').val(defaultUrlString);
    $('#mainWorkPlaceAreaId').val(mainWorkPlaceAreaId);
    document.getElementById("deleteModalErrMessage").innerText = '';
}


/**
 * 初始化一选多的 bootstrap-multiselect 插件
 * @param {any} divId 待初始化处理的 div 的 id
 */
function appInitialBootstrapMultiselect(divId) {
    $('#'+divId).multiselect({
        includeSelectAllOption: true,
        nonSelectedText: '没有任何选择项',
        allSelectedText: '全部选中',
        selectAllText: ' 全选'
    });
}

/**
 * 初始化富文本编辑器 Summernote 插件
 * @param {any} divId 待初始化处理的 div 的 id
 */
function appInitialSummernote(divId){
    $("#"+divId).summernote({
        height: 260,
        lang:"zh-CN"
    });
}

function appSetDivHeight(divId) {
    // 获取浏览器内容区高度
    var browserHeight = window.innerHeight;
    var divHeight = browserHeight - 210;
    if (document.getElementById(divId)) {
        document.getElementById(divId).style.height = divHeight + "px";
    }
    
}

function appSetDivHeightWithFooter(divId) {
    // 获取浏览器内容区高度
    var browserHeight = window.innerHeight;
    var divHeight = browserHeight - 259;
    if (document.getElementById(divId)) {
        document.getElementById(divId).style.height = divHeight + "px";
    }
}

/**
 * 设置相关 Div 高度
 */
function resetDivHeight() {

    var navigatorTreeViewContainer = document.getElementById("navigatorTreeViewContainer");
    if (navigatorTreeViewContainer !== null) {
        appSetDivHeight("navigatorTreeViewContainer");
    }

    var listContainer = document.getElementById("listContainer");
    if (listContainer !== null) {
        appSetDivHeight("listContainer");
    }

    var createOrEditContainer = document.getElementById("createOrEditContainer");
    if (createOrEditContainer !== null) {
        appSetDivHeightWithFooter("createOrEditContainer");
    }

    var detailContainer = document.getElementById("detailContainer");
    if (createOrEditContainer !== null) {
        appSetDivHeightWithFooter("detailContainer");
    }

}
