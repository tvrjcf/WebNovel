var $ = layui.$ //重点处;

var selectType = "all";
var bookTypes = [];
var books = [];
var downLoadData = [];
var chartsData = [];

function layerAlert(msg) {
    layer.alert(msg, {
        //offset: '100px'
        //maxmin: true
    });
}
function layerMsg(msg) {
    layer.msg(msg, {
        //offset: '100px'
    });
}
//layer.open({
//    type: 2,
//    content: '33',
//    maxmin: true
//});
function BindMenuEvent() {
    $("#menu a").bind("click", function () {
        var key = $(this).attr("key");
        //layerMsg(key);
        //if (key == undefined) return;
        switch (key) {
            case undefined:
            case "main":
                $("#main").show();
                $("#charts").hide();
                break;
            case "charts":
                $("#main").hide();
                $("#charts").show();
                //ShowChartsWin(null, '榜单下载');
                break;
            case "addbook":
                ShowEditdWin({ NovelID: 0, ListUrl:"" }, '添加书籍');
                break;
            case "downcharts":                
                //alert(GetChartsList());
                ShowChartsWin(null, '榜单下载');
                break;
            default:
        }

    });

    $("#btn_searchChatrs").on("click", function () {
        SearchCharts();
    });
    $("#btn_siteSign").on("click", function () {
        var chartsUrl = $("#chartsUrl").val();
        ShowChartsWin({ url: chartsUrl }, "网站标志设置");
    });
}

function Resize() {
    //alert('resize');
    table.reload('chartsList', { height: 'full-140' });
}

function InitChartsTable() {
    table.render({
        id: 'chartsList'
        , elem: '#chartsList'
        , toolbar: '#toolbar'
        , cols: [[ //标题栏
            { type: 'numbers' }
            , { type: 'checkbox' }
            , { field: 'NovelID', title: 'ID', width: 70, sort: true }
            , { field: 'NovelName', title: '名称', width: 200 }       //, templet: '#TitleTpl'
            //, { field: 'Volume', title: '分卷名', minWidth: 150 }
            , { field: 'ListUrl', title: '地址', width: 350 }
            , { field: 'Info', title: '状态', width: 100, sort: true }
            //, { field: 'experience', title: '积分', width: 80, sort: true }
        ]]
        , data: []
        , size: 'sm'    //表格尺寸
        //, skin: 'line'  //表格风格(line、row、nob)
        , even: true    //隔行背景
        , page: true    //是否显示分页
        , limits: [100, 200, 500, 1000]
        , limit: 500 //每页默认显示的数量
        , height: 'full-140'

    });
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
            case 'search':
                //layerMsg('搜索');
                //SearchCharts()
                break;
            case 'reload':
                BindData(chartsData);
                layerMsg('数据已刷新');
                break;
            case 'downLoad':
                var data = checkStatus.data;
                downLoadCharts(data);
                break;
            case 'delete':
                var data = checkStatus.data;
                if (data.length == 0) {
                    layerMsg("没有要选中任何数据");
                    return;
                }
                layer.confirm('确认要删除[' + data.length + ' ]行数据么?'
                    , {
                        //offset: '100px',
                        title: '章节删除', icon: 3
                    }
                    , function (index) {
                        var ids = [];
                        $(data).each(function (i, item) {
                            $(chartsData).each(function (j, down) {
                                if (item.ListUrl == down.ListUrl) {
                                    chartsData.splice(j, 1);
                                    if (item.Id != 0)
                                        ids.push(item.Id);
                                }
                            });
                        });
                        //parent.DelNovelContents(ids.join(','));
                        //parent.DelNovelContents(JSON.stringify(ids));
                        layer.close(index);
                        BindData(chartsData);
                    });
                break;
        };
    });
}

function downLoadCharts(data) {

    if (data.length == 0) {
        layerMsg("没有要选中任何数据");
        return;
    }
    layer.confirm('确认要下载[' + data.length + ' ]行数据么?'
        , {
            //offset: '100px',
            title: '榜单下载', icon: 3
        }
        , function (index) {
            //layer.close(index);
            layerMsg('开始下载...');
            $(data).each(function (j, down) {
                //if (item.Key == down.Id) {
                down.Info = "";
                //}
            });
            SetProgressValue(0);
            var ret = DownLoadCharts(JSON.stringify(data));
            if (ret == "-1") {
                layerMsg('下载已取消!'); return;
            } else if (ret == "0") {
                var value = 0;
                var setInterval = self.setInterval(function () {
                    value = UpdateProgressValue();
                    SetProgressValue(value);
                    if (value >= 100) {
                        window.clearInterval(setInterval);
                        layerAlert('下载完成!');
                        //var erro = JSON.parse(GetErroList());
                        //$(erro).each(function (i, item) {

                        //    $(data).each(function (j, down) {
                        //        if (item.Key == down.Id) {
                        //            down.Info = item.Message;
                        //        }
                        //    });
                        //    //layerMsg(item.Key + "->" + item.Message); return false;
                        //});
                        //BindChartsData(chartsData);
                    }
                }, 500)
            } else {
                layerAlert(ret);
            }
        });
}

/**
 * 搜索榜单
 * */
function SearchCharts() {
    var chartsUrl = $("#chartsUrl").val();
    var hrefPattern = $("#hrefPattern").val();
    if (chartsUrl.length == 0) chartsUrl = "http://www.hebao22.com/qitaleixing/";
    if (hrefPattern.length == 0) hrefPattern = "/book/\\d+/";

    var sign = JSON.parse(GetSiteSign(chartsUrl));
    if (sign && sign.url) {
    } else {
        layerMsg("还未收录过该站点标志信息");
        ShowChartsWin({ url:chartsUrl }, "网站标志设置");
        return false;
    }

    var result = JSON.parse(parent.GetChartsList(chartsUrl, hrefPattern));
    if (!result.Success) {
        layerAlert(result.Message); return false;
    }
    chartsData = JSON.parse(result.Data);
    //table.reload('chartsList', { data: chartsData });
    BindChartsData(chartsData);
}

function BindChartsData(data) {

    table.reload('chartsList', { data: data });
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
 * 显示编辑页面
 * @param {object} data 编辑对象
 * @param {string} title 标题   
 */
function ShowChartsWin(data, title) {
    //var result = JSON.parse(GetChartsList());
    //if (!result.Success) {
    //    layerAlert(result.Message); return false;
    //}
    //localStorage.setItem("charts", result.Data);
    localStorage.setItem("sitesign", JSON.stringify(data));

    layer.open({
        type: 2,
        title: title || 'gq',
        //offset: '50px',
        area: ['700px', '470px'],
        //fixed: false, //不固定
        maxmin: true,
        content: 'book_sign.html',
        //content: $('#downDialog'),
        //end: function () {
        //    $('#downDialog').hide();
        //},
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
 * 设置进度值
 * @param {Number} value
 */
function SetProgressValue(value) {
    element.progress('downLoadProgress', value + "%");
    if (value != 0) $(".layui-progress").show();
    else $(".layui-progress").hide();
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

        Concurrent.Thread.create(function () {

        BindMenuEvent();
        GetBookTypes();
        GetBooks();
        InitChartsTable();

        });
        //alert(3);
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
    $(window).resize(function () {
        Resize();
    });

    var layer = layui.layer;
    var form = layui.form;
    var table = layui.table;
    var element = layui.element;
});
