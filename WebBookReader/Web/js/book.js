var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var bookTypes = [];
var books = [];
var downLoadData = [];

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
                if ($(item).attr("LB") != key) {
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
        tpl += "<li class=\"layui-col-xs2 book\" style=\"width: 150px; display:none;\" LB='" + item.LB + "' key='" + item.NovelID + "'>";
        tpl += "    <a class=\"x-admin-backlog-body\" style=\"height: 150px;\" href = '..\\..\\chm\\" + item.NovelID + "\\List.htm'>";
        tpl += "        <img src=\"./images/Default.png\" width=\"80\" height=\"110\" />";
        tpl += "       <p style=\"text-align:center; margin:5px 0;\">";
        tpl += "           <cite>" + item.NovelName + "</cite>";
        tpl += "        </p>";
        tpl += "    </a>";
        tpl += "    <div class=\"edit-tool\" > ";
        tpl += "        <a class=\"update\" style=\"text-decoration:none\" title=\"更新\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
        tpl += "            <i class=\"layui-icon\" style=\"color:green;\">&#xe666;</i>";
        tpl += "        </a>";
        tpl += "        <a class=\"download\" style=\"text-decoration:none\" title=\"下载\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
        tpl += "            <i class=\"layui-icon\" style=\"color:orange;\">&#xe601;</i>";
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
        downLoadData = [];
        var key = $(this).attr("key");
        var bookname = $(this).attr("bookname");
        var result = JSON.parse(UpdateNovel(key));
        if (!result.Success) { layerAlert(result.Message); return; }
        downLoadData = JSON.parse(result.Data);
        if (downLoadData.length == 0)
            layerMsg("没有要更新的内容");
        //layer.tips('没有要更新的内容', "#book_" + key);
        else
            layer.confirm("[" + bookname + "] 发现章节更新 [" + downLoadData.length + "], 是否进行同步?",
                { offset: '100px', title: '章节更新', icon: 3 },
                function (index) { ShowDownLoadWin(downLoadData, '更新列表', 'book_down.html'); },
                function (index) { layer.close(index); }
            );
    });

    $(".download").bind("click", function () {
        downLoadData = [];
        var key = $(this).attr("key");
        var bookname = $(this).attr("bookname");
        var result = JSON.parse(GetNovelContents(key));
        if (!result.Success) { layerAlert(result.Message); return; }
        downLoadData = JSON.parse(result.Data);
        if (downLoadData.length == 0)
            layerMsg("没有要下载的内容");
        //layer.tips('没有要更新的内容', "#book_" + key);
        else
            ShowDownLoadWin(downLoadData, '目录列表', 'book_down.html');
    });

    $(".edit").bind("click", function () {
        downLoadData = [];
        var key = $(this).attr("key");
        var bookname = $(this).attr("bookname");
        var data = GetBook(key);
        localStorage.setItem("editbook", JSON.stringify(data));

        layer.open({
            type: 2,
            title: '编辑 [' + bookname + ']',
            offset: '50px',
            area: ['700px', '470px'],
            //fixed: false, //不固定
            maxmin: true,
            content: 'book_edit.html',
            //content: $('#downDialog'),
            //end: function () {
            //    $('#downDialog').hide();
            //}
        });
    });

    $(".delete").bind("click", function () {
        downLoadData = [];
        var key = $(this).attr("key");
        var bookname = $(this).attr("bookname");

        layer.confirm("确认要删除 [" + bookname + "] 吗?",
            { offset: '100px', title: '书籍删除', icon: 3 },
            function (index) {
                var result = JSON.parse(DelNovel(key));
                if (!result.Success) {
                    layerAlert(result.Message); return;
                }
                $(books).each(function (i, item) {
                    if (item.NovelID === key) {
                        books.slice(i, 1);
                        $(".book[key='" + key + "']").remove();
                        layerMsg("[" + bookname + "] 已删除");
                        return false;
                    }
                });
                layer.close(index);
            },
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
function ShowDownLoadWin(data, title, content) {
    localStorage.setItem("downLoadData", JSON.stringify(data));
    //SetProgressValue(0);
    var layerOpen = null;
    layer.open({
        type: 2,
        title: title || "下载",
        offset: '50px',
        area: ['750px', '500px'],
        //fixed: false, //不固定
        maxmin: true,
        content: content || 'book_down.html',
        //content: $('#downDialog'),
        end: function () {
            $('#downDialog').hide();
        },
        success: function (layero, index) {
            layerOpen = layero;
        },
        full: function () {
            var iframeWin = window[layerOpen.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法
            iframeWin.Resize()
        },
        restore: function () {
            var iframeWin = window[layerOpen.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法
            iframeWin.Resize()
        },
    });
}

function GetBook(id) {
    $(books).each(function (i, item) {
        if (item.NovelID === id) {
            return item;
        }
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
