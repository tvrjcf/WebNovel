var $ = layui.$ //重点处;

var selectType = "all";
var bookTypes = [];
var books = [];
var downLoadData = [];

function layerAlert(msg) {
    layer.alert(msg, {
        //offset: '100px'
    });
}
function layerMsg(msg) {
    layer.msg(msg, {
        //offset: '100px'
    });
}

function BindMenuEvent() {
    $("#menu a").bind("click", function () {
        var key = $(this).attr("key");
        layerMsg(key);
        //if (key == undefined) return;
        switch (key) {
            case undefined:
            case "":
                break;
            case "addbook":
                ShowEditdWin({ NovelID: 0 }, '添加书籍');
                break;
            default:
        }

    });
}

/**
 * 查询书籍类型 
 */
function GetBookTypes() {
    bookTypes = JSON.parse(GetNovelTypes());
    $("#bookType").html("");
    $("#bookType").append("<li class='layui-nav-item layui-this' key='all'><a title='全部'>全部</a></li>");
    $(bookTypes).each(function (i, item) {
        $("#bookType").append("<li class='layui-nav-item' key='" + item.DM + "'><a title='" + item.MC + "'>" + item.MC + "</a></li>"); //(" + item.DM + ")
        //$("#bookType").append("<div class=\"nav-item\" key = '" + item.DM + "'><a href=\"#\"><span class=\"icon nav-toggle-trigger\" ></span>" + item.MC + "(" + item.DM + ")</a></div>");
    });
    //$("#bookType").append("<li class='layui-nav-item addbook' key='addbook'><a title='添加书籍'>添加书籍</a></li>");
    $("#bookType li").bind("click", function () {
        var key = $(this).attr("key");
        //layerMsg(key);
        //if (key == undefined) return;
        switch (key) {
            case undefined:
            case "":
                break;
            default:
                selectType = key;
                $(this).addClass("layui-this").siblings(".layui-this").removeClass("layui-this");
                $(".book").hide();
                $(".book").each(function (i, item) {
                    if (key == "all") {
                        $(item).show();
                    } else {
                        if ($(item).attr("LB") != key) {
                            //$(item).hide(); 
                        }
                        else
                            $(item).show();
                    }
                });
        }


    });

}

/**
 * 查询书籍
 * */
function GetBooks() {
    books = JSON.parse(GetNovels());
    //alert(books.length);
    BindBook(selectType);
    BindBookEvent(true);
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
        content += GetBookHtmlTpl(item);

    });
    $("#bookList").html(content);
    $(".book").show();
}

/**
 * 绑定书籍相关事件
 * */
function BindBookEvent() {

    $(".layui-col-xs2").each(function (i, item) {
        var e = $._data(this, "events");//是this 而不是 $(this)
        //layerMsg(JSON.stringify($(this).data("events"))); 
        if (e && e["mouseover"]) {
        } else {
            $(this).on("mouseover mouseout", function () {
                $(this).find(".edit-tool").toggle();
            });
            //var content = $("#edit-tool").html();
            //$(this).on("hover",
            //    function () {
            //        layer.tips(content, this, { tips: [4, "#fff"] });
            //    }, function () {

            //    }
            //);
        }
        //return false;
    });

    $(".update").each(function (i, item) {
        var e = $._data(this, 'events');
        if (e && e["click"]) {
            //layerMsg("已绑定 click");
        } else {
            //layerMsg("未绑定 click");
            $(this).on("click", function () {
                var key = $(this).attr("key");
                var bookname = $(this).attr("bookname");
                downLoadData = [];
                var result = JSON.parse(UpdateNovel(key));
                if (!result.Success) { layerAlert(result.Message); return; }
                downLoadData = JSON.parse(result.Data);
                if (downLoadData.length == 0)
                    layerMsg("没有要更新的内容");
                //layer.tips('没有要更新的内容', "#book_" + key);
                else
                    layer.confirm("[" + bookname + "] 发现章节更新 [" + downLoadData.length + "], 是否进行同步?",
                        {
                            //offset: '100px', 
                            title: '章节更新',
                            icon: 3
                        },
                        function (index) { ShowDownLoadWin(downLoadData, '更新列表', 'book_down.html'); },
                        function (index) { layer.close(index); }
                    );
            });
        }
    });

    $(".download").each(function (i, item) {
        var e = $._data(this, 'events');
        if (e && e["click"]) {
            //layerMsg("已绑定 click");
        } else {
            //layerMsg("未绑定 click");
            $(this).on("click", function () {
                var key = $(this).attr("key");
                var bookname = $(this).attr("bookname");
                layer.msg('书籍同步', {
                    time: 0 //不自动关闭
                    , content: '书籍同步'
                    , btn: ['更新', '下载', '制作TXT', '取消']
                    , yes: function (index, layero) {
                        UpdateBook(key, bookname);
                    }
                    , btn2: function (index, layero) {
                        DownLoadBook(key, bookname);
                    }
                    , btn3: function (index, layero) {
                        CreateTXTBook(key, bookname);
                    }
                    , btn4: function (index, layero) {
                        //按钮【按钮三】的回调

                        //return false 开启该代码可禁止点击该按钮关闭
                    }
                    , cancel: function () {
                        //右上角关闭回调

                        //return false 开启该代码可禁止点击该按钮关闭
                    }
                });
            });
        }
    });

    $(".edit").each(function (i, item) {
        var e = $._data(this, 'events');
        if (e && e["click"]) {
            //layerMsg("已绑定 click");
        } else {
            //layerMsg("未绑定 click");
            $(this).on("click", function () {
                var key = $(this).attr("key");
                var bookname = $(this).attr("bookname");
                var data = FindBook(key);
                localStorage.setItem("editbook", JSON.stringify(data));
                ShowEditdWin(data, '编辑 [' + bookname + ']');
            });
        }
        //return false;
    });

    $(".delete").each(function (i, item) {
        var e = $._data(this, 'events');
        if (e && e["click"]) {
            //layerMsg("已绑定 click");
        } else {
            //layerMsg("未绑定 click");
            $(this).on("click", function () {
                var key = $(this).attr("key");
                var bookname = $(this).attr("bookname");

                DeleteBook(key, bookname);
            });
        }
    });

    $(".link-book").each(function (i, item) {
        var e = $._data(this, 'events');
        if (e && e["click"]) {
            //layerMsg("已绑定 click");
        } else {
            //layerMsg("未绑定 click");
            $(this).on("click", function () {
                var key = $(this).attr("src");
                var bookname = $(this).attr("bookname");
                if (fileExists(key) == -1)
                    layerMsg("目录[" + key + "]不存在");
                else {
                    //window.location.href = key;
                    var index = layer.open({
                        title: bookname || '',
                        type: 2,
                        content: key,
                        area: ['700px', '500px'],
                        maxmin: true
                    });
                    layer.full(index);
                }
            });
        }
    });
}

/**
 * 判断文件是否存在
 * @param {string} url
 */
function fileExists(url) {
    var isExists;
    $.ajax({
        url: url,
        async: false,
        type: 'HEAD',
        error: function () {
            isExists = -1;
        },
        success: function () {
            isExists = 1;
        }
    });
    return isExists;
}

/**
 * 更新书籍
 * @param {string} key 书籍Id
 * @param {string} bookname 书籍名称
 */
function UpdateBook(key, bookname) {
    downLoadData = [];
    var result = JSON.parse(UpdateNovel(key));
    if (!result.Success) { layerAlert(result.Message); return; }
    downLoadData = JSON.parse(result.Data);
    if (downLoadData.length == 0)
        layerMsg("没有要更新的内容");
    //layer.tips('没有要更新的内容', "#book_" + key);
    else
        layer.confirm("[" + bookname + "] 发现章节更新 [" + downLoadData.length + "], 是否进行同步?",
            {
                //offset: '100px', 
                title: '章节更新',
                icon: 3
            },
            function (index) { ShowDownLoadWin(downLoadData, '更新列表', 'book_down.html'); },
            function (index) { layer.close(index); }
        );
}

/**
 * 下载书籍
 * @param {string} key 书籍Id
 * @param {string} bookname 书籍名称
 */
function CreateTXTBook(key, bookname) {
    var result = CreateEBook(key);
    
    layerAlert(result);
    
}

/**
 * 制作电子书籍
 * @param {string} key 书籍Id
 * @param {string} bookname 书籍名称
 */
function DownLoadBook(key, bookname) {
    downLoadData = [];
    var result = JSON.parse(GetNovelContents(key));
    if (!result.Success) { layerAlert(result.Message); return; }
    downLoadData = JSON.parse(result.Data);
    if (downLoadData.length == 0)
        layerMsg("没有要下载的内容");
    //layer.tips('没有要更新的内容', "#book_" + key);
    else
        ShowDownLoadWin(downLoadData, '目录列表', 'book_down.html');
}

/**
 * 删除书籍
 * @param {string} key 书籍Id
 * @param {string} bookname 书籍名称
 */
function DeleteBook(key, bookname) {
    layer.confirm("确认要删除 [" + bookname + "] 吗?",
        {
            //offset: '100px', 
            title: '书籍删除',
            icon: 3
        },
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
}

/**
 * 显示编辑页面
 * @param {object} data 编辑对象
 * @param {string} title 标题   
 */
function ShowEditdWin(data, title) {
    localStorage.setItem("editbook", JSON.stringify(data));

    layer.open({
        type: 2,
        title: title || '编辑',
        //offset: '50px',
        area: ['700px', '470px'],
        //fixed: false, //不固定
        maxmin: true,
        content: 'book_edit.html',
        //content: $('#downDialog'),
        //end: function () {
        //    $('#downDialog').hide();
        //}
    });
}

/**
 * 设置进度值
 * @param {Number} value
 */
function SetProgressValue(value) {
    element.progress('downLoadProgress', value + "%");
}

/**
 * 显示更新列表
 * @param {object} data 更新列表
 * @param {string} title    标题
 * @param {string} content  内容|URL|DOM
 */
function ShowDownLoadWin(data, title, content) {
    localStorage.setItem("downLoadData", JSON.stringify(data));
    //SetProgressValue(0);
    var layerOpen = null;
    layer.open({
        type: 2,
        title: title || "下载",
        //offset: '50px',
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

/**
 * 查询书籍
 * @param {string} 书籍id
 */
function FindBook(id) {
    //alert(JSON.stringify(books));
    for (var i = 0; i < books.length; i++) {
        if (books[i].NovelID == id) {
            return books[i];
        }
    }
    //$(books).each(function (i, item) {
    //    if (item.NovelID == id) {
    //        return item;
    //    }
    //});
    return null;
}

/**
 * 添加书籍
 * @param {any} data    书籍列表
 */
function ReflushBook(data) {
    //alert(books.length);
    $(data).each(function (i, item) {
        if (FindBook(item.NovelID)) {

        } else {
            books.push(item);
            var tpl = GetBookHtmlTpl(item)
            $("#bookList").append(tpl);
            //layerMsg(JSON.stringify(item));
            BindBookEvent(false);
        }
    });

}

/**
 * 获取书籍HTML模板
 * @param {any} item
 */
function GetBookHtmlTpl(item) {

    var tpl = "";
    tpl += "<li class=\"layui-col-xs2 layui-anim layui-anim-scale book\" style=\"width: 150px; display:none;\" LB='" + item.LB + "' key='" + item.NovelID + "'>";
    tpl += "    <a class=\"x-admin-backlog-body link-book\" style=\"height: 150px;\" src='..\\..\\chm\\" + item.NovelID + "\\List.htm' bookname='" + item.NovelName + "'>";  //href = '..\\..\\chm\\" + item.NovelID + "\\List.htm'
    tpl += "        <img src=\"./images/Default.png\" width=\"80\" height=\"110\" />";
    tpl += "       <p style=\"text-align:center; margin:5px 0;\">";
    tpl += "           <cite>" + item.NovelName + "</cite>";
    tpl += "        </p>";
    tpl += "    </a>";
    tpl += "    <div class=\"edit-tool \" > ";  //layui-anim layui-anim-scale
    //tpl += "        <a class=\"update\" style=\"text-decoration:none\" title=\"更新\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
    //tpl += "            <i class=\"layui-icon\" style=\"color:green;\">&#xe666;</i>";
    //tpl += "        </a>";
    tpl += "        <a class=\"download\" style=\"text-decoration:none\" title=\"同步\" key='" + item.NovelID + "' bookname='" + item.NovelName + "'>";
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
    return tpl;

}


$(document).ready(function () {

    try {
        //tryCode - 尝试执行代码块
        BindMenuEvent();
        GetBookTypes();
        GetBooks();
    }
    catch (err) {
        //catchCode - 捕获错误的代码块
        layerMsg("erro: " + err.message);
    }
    finally {
        //finallyCode - 无论 try / catch 结果如何都会执行的代码块
        //初始化高度
        $(".full").height($(window).height() - $(".layui-nav").height());
        setTimeout(function () {
            layer.closeAll('loading');
        }, 200);
    }

    var layer = layui.layer;
    var form = layui.form;
    var table = layui.table;
    var element = layui.element;
});
