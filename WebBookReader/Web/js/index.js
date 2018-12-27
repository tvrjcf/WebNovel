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
    $("#bookType").append("<div class=\"nav-item\" key = 'all'><a href=\"#\"><span class=\"icon nav-toggle-trigger\" ></span>全部</a></div>");
    $(bookTypes).each(function (i, item) {
        $("#bookType").append("<div class=\"nav-item\" key = '" + item.DM + "'><a href=\"#\"><span class=\"icon nav-toggle-trigger\" ></span>" + item.MC + "(" + item.DM + ")</a></div>");
    });
    $(".nav-item").bind("click", function () {
        //alert($(this).attr("key"));
        //BindBook($(this).attr("key"));
        var key = $(this).attr("key");
        $(".book").each(function (i, item) {
            if (key == "all") {
                $(item).show();
            } else {
                if ($(item).attr("key") != key)
                    $(item).hide();
                else
                    $(item).show();
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

        var bookTpl = "<div class='yd-book-item yd-book-item-pull-left book' key='" + item.LB + "'>";
        if ((i + 1) % 2 == 0) bookTpl = "<div class='yd-book-item yd-book-item-pull-left edge-right book' key='" + item.LB + "'>";
        bookTpl += "<button id='book_" + item.NovelID + "' class='layui-btn layui-btn-sm layui-btn-primary updatebook' key='" + item.NovelID + "' bookname='" + item.NovelName + "'><i class='layui-icon'>&#xe669;</i></button>";
        bookTpl += "    <a href = '..\\..\\chm\\" + item.NovelID + "\\List.htm' > <img src='./res/Default.png' alt='" + item.NovelName + "' class='pull-left cover-container' width='90' height='120' /> <h2>(" + item.LB + ")" + item.NovelName + "</h2> </a>";
        //bookTpl += "<a href = '#' target = '_blank' onclick = 'OpenBook(" + item.NovelID +")'> <img src='./res/Default.png' alt='" + item.NovelName + "' class='pull-left cover-container' width='114' height='160' /> <h2>" + item.NovelName + "</h2> </a>";
        bookTpl += "    <div class='author-container'>";
        bookTpl += "        <dl class='dl-horizontal-inline'>";
        bookTpl += "            <dt>作者：</dt>";
        bookTpl += "            <dd>" + item.Author + "</dd>";
        bookTpl += "        </dl>";
        bookTpl += "    </div>";
        bookTpl += "    <div class='rate w-star w-star1'> ";
        bookTpl += "        <span>&nbsp;</span > ";
        bookTpl += "        <span>&nbsp;</span > ";
        bookTpl += "        <span>&nbsp;</span > ";
        bookTpl += "        <span>&nbsp;</span > ";
        bookTpl += "        <span class='no' >&nbsp;</span> ";
        bookTpl += "    </div> ";
        bookTpl += "    <div class='price-container f-invi'>";
        bookTpl += "        < b class='price' > ￥9.90 </b> ";
        bookTpl += "    </div> ";
        //bookTpl += "    <div class='summery'> ";
        //bookTpl += "    <p>简介：" + "" + "</p> "; //item.Brief
        //bookTpl += "    </div > ";
        bookTpl += "    <div class='badge badge-complete png' ></div > ";
        bookTpl += "</div >";

        content += bookTpl;
    });
    $("#bookList").html(content);

    $(".updatebook").bind("click", function () {
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
                function ShowDownLoadWinhowDownWin(downLoadData); },
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
 *ShowDownLoadWin ShowDownWin(data) {

    SetProgressValue(0);
    layer.open({
        type: 1,
        title: '更新列表',
        offset: '50px',
        area: ['700px', '470px'],
        //fixed: false, //不固定
        maxmin: true,
        content: $('#downDialog'),
        end: function () {
            $('#downDialog').hide();
        }
    });
    //

    table.render({
        id: 'updateList'
        , elem: '#updateList'
        , toolbar: '#toolbar'
        , cols: [[ //标题栏
            { type: 'checkbox' }
            //, { field: 'Id', title: 'ID', width: 80, sort: true }
            , { field: 'Title', title: '标题', width: 200, templet: '#TitleTpl' }}
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
                        }
                        var value = 0;
                        var setInterval = self.setInterval(function () {
                            value = UpdateProgressValue();
                            SetProgressValue(value);
                            if (value >= 100) {
                                window.clearInterval(setInterval)
                                layerMsg('下载完成!');
                            }
                        }, 500)

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
                            $(downLoadData).each(function (j, update) {
                                if (item.ComeFrom == update.ComeFrom) {
                                    downLoadData.splice(j, 1);
                                }
                            });
                        });
                        layer.close(index);
                        table.reload('updateList', { data: downLoadData });
                    });
                break;
        };
    });
}

function sleep(numberMillis) {
    var now = new Date();
    var exitTime = now.getTime() + numberMillis;
    while (true) {
        now = new Date();
        if (now.getTime() > exitTime)
            return;
    }
}