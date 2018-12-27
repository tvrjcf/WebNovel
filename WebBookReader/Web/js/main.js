var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var bookTypes = [];
var books = [];
var updateList = [];

function layerAlert(msg) {
    layer.alert(msg, { offset: '100px' });
}
function layerMsg(msg) {
    layer.msg(msg, { offset: '100px' });
}
/**
 * 查询书籍类型 
 */
function GetBookTypes() {
    //alert(GetNovelTypes())
    bookTypes = JSON.parse(GetNovelTypes());
    //alert(bookTypes);
    $("#bookType").html("");
    $("#bookType").append("<li class='layui-nav-item layui-this' key='all'><a title='全部'>全部</a></li>");
    $(bookTypes).each(function (i, item) {
        $("#bookType").append("<li class='layui-nav-item' key='" + item.DM + "'><a title='" + item.MC + "'>" + item.MC + "</a></li>"); //(" + item.DM + ")
        //$("#bookType").append("<div class=\"nav-item\" key = '" + item.DM + "'><a href=\"#\"><span class=\"icon nav-toggle-trigger\" ></span>" + item.MC + "(" + item.DM + ")</a></div>");
    });
    $(".layui-nav-item").bind("click", function () {
        //alert($(this).attr("key"));
        //BindBook($(this).attr("key"));
        var key = $(this).attr("key");
        //$(".layui-this").removeClass("layui-this");
        $(this).addClass("layui-this").siblings(".layui-this").removeClass("layui-this");
        $(".book").hide();
        $(".book").each(function (i, item) {
            if (key == "all") {
                $(item).show(300);
            } else {
                if ($(item).attr("key") != key) {
                    //$(item).hide(); 
                }
                else
                    $(item).show(300);
            }
        });
    });

}

/**
 * 查询书籍
 * */
function GetBooks() {
    books = JSON.parse(GetNovels());
    //alert(GetNovels());
    BindBook("all");
}

/**
 * 绑定书籍到UI
 * @param {string} bookType
 */
function BindBook(bookType) {
    $("#bookList").html("");
    var content = "";
    $(books).each(function (i, item) {
        //if (bookType != "all" && item.LB != bookType) return true;
        var tpl = "";
        tpl += "<li class=\"layui-col-xs2 book\" style=\"width: 150px; display:none;\" key='" + item.LB + "'>";
        tpl += "    <a class=\"x-admin-backlog-body\" style=\"height: 150px;\" href = '..\\..\\chm\\" + item.NovelID + "\\List.htm'>";
        tpl += "        <img src=\"./images/Default.png\" width=\"80\" height=\"110\" />";
        tpl += "       <p style=\"text-align:center; margin:5px 0;\">";
        tpl += "           <cite>" + item.NovelName + "</cite>";
        tpl += "        </p>";
        tpl += "    </a>";
        tpl += "    <div class=\"edit-tool\" > ";
        tpl += "        <a class=\"update\" style=\"text-decoration:none\" title=\"更新\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
        tpl += "            <i class=\"layui-icon\" style=\"color:green;\">&#xe601;</i>";
        tpl += "        </a>";
        tpl += "        <a class=\"edit\" style=\"text-decoration:none\" title=\"编辑\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
        tpl += "           <i class=\"layui-icon\" style=\"color:darkviolet;\">&#xe642;</i>";
        tpl += "        </a>";
        tpl += "        <a class=\"delete\" style=\"text-decoration:none\" title=\"删除\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
        tpl += "           <i class=\"layui-icon\" style=\"color:red;\">&#xe640;</i>";
        tpl += "       </a>";
        tpl += "   </div> ";
        tpl += "</li> ";
        content += tpl;

    });
    $("#bookList").html(content);
    $(".book").show(300);

    $(".update").bind("click", function () {
        updateList = [];
        var key = $(this).attr("key");
        var bookname = $(this).attr("bookname");
        var result = JSON.parse(UpdateNovel(key));
        if (!result.Success) { layerAlert(result.Message); return; }
        updateList = JSON.parse(result.Data);
        if (updateList.length == 0)
            layerMsg("没有要更新的内容");
        //layer.tips('没有要更新的内容', "#book_" + key);
        else
            layer.confirm("[" + bookname + "] 发现章节更新 [" + updateList.length + "], 是否进行同步?",
                { offset: '100px', title: '章节更新', icon: 3 },
                function (index) { ShowUpdateList(updateList); },
                function (index) { layer.close(index); }
            );
    });
}

function DownLoad(data) {
}

function SetProgressValue(value) {
    element.progress('downLoadProgress', value + "%");
}

/**
 * 显示更新列表
 * @param {any} data
 */
function ShowUpdateList(data) {
    localStorage.setItem("updateList", JSON.stringify(data));
    SetProgressValue(0);
    layer.open({
        type: 2,
        title: '更新列表',
        offset: '50px',
        area: ['700px', '470px'],
        //fixed: false, //不固定
        maxmin: true,
        content: 'book_update.html',
        //content: $('#downDialog'),
        end: function () {
            $('#downDialog').hide();
        }
    });
    //
    return;

    table.render({
        id: 'updateList'
        , elem: '#updateList'
        , toolbar: '#toolbar'
        , cols: [[ //标题栏
            { type: 'checkbox' }
            //, { field: 'Id', title: 'ID', width: 80, sort: true }
            , { field: 'Title', title: '标题', width: 200, templet: '#TitleTpl' }
            //, { field: 'Volume', title: '分卷名', minWidth: 150 }
            , { field: 'ComeFrom', title: '地址', width: 400 }
            //, { field: 'city', title: '城市', width: 100 }
            //, { field: 'experience', title: '积分', width: 80, sort: true }
        ]]
        , data: data
        //,skin: 'line' //表格风格
        , even: true
        , page: true //是否显示分页
        , limits: [100, 200, 500, 1000]
        , limit: 200 //每页默认显示的数量
        , height: '380'
    });
    //头工具栏事件
    table.on('toolbar(update)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'getCheckData':
                var data = checkStatus.data;
                layerAlert(JSON.stringify(data));
                break;
            case 'getCheckLength':
                var data = checkStatus.data;
                layerMsg('选中了：' + data.length + ' 个');
                break;
            case 'isAll':
                layerMsg(checkStatus.isAll ? '全选' : '未全选');
                break;
            case 'downLoad':
                var data = checkStatus.data;

                if (data.length == 0) {
                    layerMsg("没有要选中任何数据");
                    return;
                }
                layer.confirm('确认要下载[' + data.length + ' ]行数据么?'
                    , { offset: '100px', title: '章节下载', icon: 3 }
                    , function (index) {
                        //layer.close(index);
                        layerMsg('开始下载...');
                        SetProgressValue(0);
                        var ret = DownLoadContent(JSON.stringify(data));
                        if (ret == "-1") {
                            layerMsg('下载已取消!'); return;
                        } else if (ret == "0") {
                            var value = 0;
                            var setInterval = self.setInterval(function () {
                                value = UpdateProgressValue();
                                SetProgressValue(value);
                                if (value >= 100) {
                                    window.clearInterval(setInterval)
                                    layerMsg('下载完成!');
                                }
                            }, 500)
                        } else {
                            layerAlert(ret);
                        }
                        //SetProgressValue(30);
                        //table.reload('updateList', { data: updateList });
                    });
                break;
            case 'delete':
                var data = checkStatus.data;
                if (data.length == 0) {
                    layerMsg("没有要选中任何数据");
                    return;
                }
                layer.confirm('确认要删除[' + data.length + ' ]行数据么?'
                    , { offset: '100px', title: '章节删除', icon: 3 }
                    , function (index) {
                        //$(".layui-form-checked").not('header').parents('tr').remove();
                        //layerMsg(JSON.stringify(updateList));
                        $(data).each(function (i, item) {
                            $(updateList).each(function (j, update) {
                                if (item.ComeFrom == update.ComeFrom) {
                                    updateList.splice(j, 1);
                                }
                            });
                        });
                        layer.close(index);
                        table.reload('updateList', { data: updateList });
                    });
                break;
        };
    });
}


$(document).ready(function () {

    setTimeout(function () {
        layer.closeAll('loading');
    }, 500);
    GetBookTypes();
    GetBooks();
    setTimeout(function () {
        layer.closeAll('loading');
    }, 500);
});
