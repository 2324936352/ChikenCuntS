/**
 * 初始化使用插件 bootstrap-fileinput 处理上传普通文件的方法，这个方法中初始化的控件 div 位于
 * Views/Shared/_CommonBusinessFilesSelectorModal.cshtml 局部视图。
 */
function appIntitialCommonBusinessFileCollectionSelector() {
    $("#commonBusinessFileCollectionSelector").fileinput({
        theme: "fa",
        language: "zh",
        uploadUrl: "/Upload/FileCollectionSave",
        maxFileCount: 6,
        minFileCount: 1,
        uploadAsync: true,
        showCaption: true,
        showPreview: true,
        showClose: false,
        initialCaption: "请选择上传普通文件",
        allowedFileTypes: ['object'], 
        initialPreviewAsData: true, 
        uploadExtraData: function () {
            // 获取关联的对象的 id
            var idValue = $('#relFileCollectionObjectID').val();
            // 向给定路径 uploadPath 的后端提交上传数据 
            return { id: idValue };
        },
        fileActionSettings: {
            showZoom: false
        }
    });
}

/**
 * 打开普通上传文件的模态对话框
 * @param {any} itemId 关联对象的 Id
 */
function appOpenCommonBusinessFileCollectionSelectorModal(itemId) {
    $('#commonBusinessFileCollectionSelectorModal').modal({
        show: true,
        backdrop: 'static'
    });
    $('#relFileCollectionObjectID').val(itemId);
}

/**
 * 关闭普通上传文件的模态对话框，并调用后端获取列表数据的方法
 * @param {any} refreshUpLoadDataPath 获取刷新前端使用的 Html 的路径
 */
function appCloseCommonBusinessFileCollectionSelectorModal(refreshUpLoadDataPath) {
    $('#commonBusinessFileCollectionSelectorModal').modal('hide');
    var id = $('#relFileCollectionObjectID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: refreshUpLoadDataPath + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonBusinessFileCollectionArea").innerHTML = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appDeleteBusinessFile(id, relevanceObjectId) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/Upload/DeleteBusinessFile/" + id+"?relevanceObjectId="+relevanceObjectId,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonBusinessFileCollectionArea").innerHTML = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 初始化头像图片文件上传插件，头像文件上传每次只能上传一个文件
 * @param {any} uploadPath 上传路径
 */
function appIntitialUploadAvatar() {
    $("#commonAvatarSelector").fileinput({
        theme: "fa",
        language: "zh",
        uploadUrl: "/Upload/AvatarSave",
        maxFileCount: 1,
        minFileCount: 1,
        uploadAsync: true,
        showCaption: true,
        showPreview: true,
        showClose: false,
        initialCaption: "请选择上传图片文件",
        allowedFileTypes: ["image"], 
        initialPreviewAsData: false, 
        uploadExtraData: function () {
            var idValue = $('#relAvatarObjectID').val();
            return { id: idValue };
        },
        fileActionSettings: {
            showZoom: false
        }
    });
}

/**
 * 打开上传头像文件选择器的模态对话框
 * @param {any} itemId 关联对象的 Id
 */
function appOpenCommonAvatarSelectorModal(itemId) {
    $('#commonAvatarSelectorModal').modal({
        show: true,
        backdrop: 'static'
    });
    $('#relAvatarObjectID').val(itemId);
}

/**
 * 关闭头像文件选择模态会话框，并更新图片
 * @param {any} refreshUpLoadDataPath 获取更新的图片路径
 */
function appCloseCommonAvatarSelectorModal(refreshUpLoadDataPath) {
    $('#commonAvatarSelectorModal').modal('hide');
    var id = $('#relAvatarObjectID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: refreshUpLoadDataPath + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("AvartarImage").src = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 初始化普通的图片文件上传插件
 * @param {any} uploadPath 上传路径
 */
function appIntitialCommonBusinessImageCollectionSelector(uploadPath) {
    $("#commonBusinessImageCollectionSelector").fileinput({
        theme: "fa",
        language: "zh",
        uploadUrl: "/Upload/ImageCollectionSave",
        maxFileCount: 6,
        minFileCount: 1,
        uploadAsync: true,
        showCaption: true,
        showPreview: true,
        showClose: false,
        initialCaption: "请选择上传图片文件",
        allowedFileTypes: ["image"], 
        initialPreviewAsData: false, 
        uploadExtraData: function () {
            var idValue = $('#relImageCollectionObjectID').val();
            return { id: idValue };
        },
        fileActionSettings: {
            showZoom: false
        }
    });
}

/**
 * 打开上传图片文件选择器的模态对话框
 * @param {any} itemId 关联对象的 Id
 */
function appOpenCommonBusinessImageCollectionSelectorModal(itemId) {
    $('#commonBusinessImageCollectionSelectorModal').modal({
        show: true,
        backdrop: 'static'
    });
    $('#relImageCollectionObjectID').val(itemId);
}

/**
 * 关闭普通上传图片文件的模态对话框，并调用后端获取列表数据的方法
 * @param {any} refreshUpLoadDataPath 获取刷新前端使用的 Html 的路径
 */
function appCloseCommonBusinessImageCollectionSelectorModal(refreshUpLoadDataPath) {
    $('#commonBusinessImageCollectionSelectorModal').modal('hide');
    var id = $('#relImageCollectionObjectID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: refreshUpLoadDataPath + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonImageFileCollectionArea").innerHTML = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appDeleteBusinessImageCollection(relevanceObjectId) {
    // 获取勾选待删除的图片的Id集合
    var imageIdCollection = new Array();
    var i = 0;
    $("input[name='imageVMCollectionName']").each(function () {
        if (this.checked === true) {
            imageIdCollection[i] = $(this).val();
            i++;
        }
    });

    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/Upload/DeleteBusinessImageCollection/" + relevanceObjectId+"?idCollection="+imageIdCollection,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonImageFileCollectionArea").innerHTML = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

/**
 * 初始化使用插件 bootstrap-fileinput 处理上传视频的方法，这个方法中初始化的控件 div 位于
 * Views/Shared/_CommonBusinessVideoesSelectorModal.cshtml 局部视图。
 * 上传视频文件，每次限制上传一份文件。
 * @param {any} uploadPath 控件选择的文件上传提交的路径，缺省的路径是：/Upload/VideoSave/Id
 */
function appIntitialCommonBusinessVideoSelector() {
    $("#commonBusinessVideoSelector").fileinput({
        theme: "fa",
        language: "zh",
        uploadUrl: "/Upload/VideoSave",
        maxFileCount: 1,
        minFileCount: 1,
        uploadAsync: true,
        showCaption: true,
        showPreview: true,
        initialCaption: "请选择上传视频文件",
        allowedFileTypes: ["video"], 
        initialPreviewAsData: false, 
        uploadExtraData: function () {
            // 提取关联数据对象的 Id
            var idValue = $('#relVideoObjectID').val();
            // 返回上传数据
            return { id: idValue };
        },
        fileActionSettings: {
            showZoom: false
        }
    });
}

/**
 * 打开在 Views/Shared/_CommonBusinessVideoesSelectorModal.cshtml 定义的上传视频文件的会话框
 * @param {any} itemId 关联数据对象的 Id
 */
function appOpenCommonBusinessVideoSelectorModal(itemId) {
    $('#commonBusinessVideoeSelectorModal').modal({
        show: true,
        backdrop: 'static'
    });
    $('#relVideoObjectID').val(itemId);
}

/**
 * 上传完成之后关闭模态会话框 commonBusinessVideoesSelector 之后执行刷新上传视频
 * @param {any} refreshPath 获取上传结果数据结果的路径，缺省路径：controllerName/DataForCommonBusinessVideo
 * @param {any} refreshDivId 用于刷新显示视频的 div，缺省值为：视图模型属性名称 + "_CommonBusinessVideo"
 */
function appCloseCommonBusinessVideoSelectorModal() {
    $('#commonBusinessVideoeSelectorModal').modal('hide');
    var id = $('#relVideoObjectID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/upload/DataForUploadVideo/" + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        document.getElementById("commonBusinessVideo").src = data;

    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appDeleteCommonBusinessVideo(id) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/Upload/DeleteVideo/" + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonBusinessVideo").src = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}


/**
 * 初始化封面图片文件上传插件，头像文件上传每次只能上传一个文件
 * @param {any} uploadPath 上传路径
 */
function appIntitialCommonBusinessVideoCoverSelector() {
    $("#commonBusinessVideoCoverSelector").fileinput({
        theme: "fa",
        language: "zh",
        uploadUrl: "/Upload/VideoCoverSave",
        maxFileCount: 1,
        minFileCount: 1,
        uploadAsync: true,
        showCaption: true,
        showPreview: true,
        showClose: false,
        initialCaption: "请选择上传图片文件",
        allowedFileTypes: ["image"], 
        initialPreviewAsData: false, 
        uploadExtraData: function () {
            var idValue = $('#relBusinessVideoCoverObjectID').val();
            return { id: idValue };
        },
        fileActionSettings: {
            showZoom: false
        }
    });
}

/**
 * 打开上传头像文件选择器的模态对话框
 * @param {any} itemId 关联对象的 Id
 */
function appOpenCommonBusinessVideoCoverSelectorModal(itemId) {
    $('#commonBusinessVideoCoverSelectorModal').modal({
        show: true,
        backdrop: 'static'
    });
    $('#relBusinessVideoCoverObjectID').val(itemId);
}

/**
 * 关闭头像文件选择模态会话框，并更新图片
 * @param {any} refreshUpLoadDataPath 获取更新的图片路径
 */
function appCloseCommonBusinessVideoCoverSelectorModal(refreshUpLoadDataPath) {
    $('#commonBusinessVideoCoverSelectorModal').modal('hide');
    var id = $('#relBusinessVideoCoverObjectID').val();
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/Upload/DataForUploadVideoCover/" + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonBusinessVideoCover").src = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appDeleteCommonBusinessVideoCover(id) {
    $.ajax({
        cache: false,
        type: 'get',
        async: true,
        url: "/Upload/DeleteVideoCover/" + id,
        beforeSend: function () {
        }
    }).done(function (data) {
        // 更新上传文件数据列表内容
        document.getElementById("commonBusinessVideoCover").src = data;
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}

function appInitialUpload() {
    appIntitialCommonBusinessFileCollectionSelector();
    appIntitialCommonBusinessImageCollectionSelector();
    appIntitialUploadAvatar();
    appIntitialCommonBusinessVideoCoverSelector();
    appIntitialCommonBusinessVideoSelector();
}