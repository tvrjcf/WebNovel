var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var book = JSON.parse(localStorage.getItem("editbook"));    //parent.updateList;
var bookstr = "{\"NovelID\":\"000003\",\"NovelName\":\"吞噬星空\",\"bIsEnd\":false,\"ListUrl\":\"http://www.cilook.net/book/0/31/index.html\",\"ContentStart\":\"<div id=\\\"content\\\">\",\"ContentEnd\":\"</div>\",\"LB\":\"07\",\"Displayorder\":2,\"NeedDelUrl\":\"http://www.cilook.net/book/0/31/1440155.html(第二十九篇 第三十六章 超脱轮回(大结局)上)||http://www.cilook.net/book/0/31/1440163.html(第二十九篇 第三十六章 超脱轮回（大结局）下)||http://www.cilook.net/(思路客小说网)||http://www.cilook.net/b/1_1.html(玄幻魔法)||http://www.cilook.net/book/0/31/(吞噬星空最新章节列表)||http://www.cilook.net/modules/article/addbookcase.php?bid=31(加入书架)||http://www.cilook.net/modules/article/uservote.php?id=31(推荐本书)||http://www.cilook.net/b/31.html(吞噬星空)\"}";
//book = JSON.parse(bookstr);
alert(localStorage.getItem("editbook").replace("/\\/", "") +"\n\n"+bookstr);

function layerAlert(msg) {
    layer.alert(msg, { offset: '100px' });
}
function layerMsg(msg) {
    layer.msg(msg, { offset: '100px' });
}

function Init() {
    BindForm(book);
    if (book != null) {
        
    }
}

/**
 * 显示更新列表
 * @param {any} data
 */
function BindForm(data) {
    alert(JSON.stringify(data));
    //表单初始赋值
    form.val('form', {
        "NovelID": data.NovelID
        , "NovelName": data.NovelName
        , "ListUrl": data.ListUrl
        , "LB": data.LB
        , "Author": data.Author
        , "Brief": data.Brief
        , "BookImg": data.BookImg
        , "ListStart": data.ListStart
        , "ListEnd": data.ListEnd
        , "VolumeStart": data.VolumeStart
        , "VolumeEnd": data.VolumeEnd
        , "ContentStart": data.ContentStart
        , "ContentEnd": data.ContentEnd
        , "NeedDelStr": data.NeedDelStr
    })
    form.render(); //更新全部
    $("#test").val("124");
    //$("#NovelID").val(data.NovelID);
    //$("#NovelName").val(data.NovelName);
    //$("#ListUrl").val(data.ListUrl);
    //$("#LB").val(data.LB);
    //$("#Author").val(data.Author);
    //$("#Brief").val(data.Brief);
    //$("#BookImg").val(data.BookImg);
    //$("#ListStart").val(data.ListStart);
    //$("#ListEnd").val(data.ListEnd);
    //$("#VolumeStart").val(data.VolumeStart);
    //$("#VolumeEnd").val(data.VolumeEnd);
    //$("#ContentStart").val(data.ContentStart);
    //$("#ContentEnd").val(data.ContentEnd);
    //$("#NeedDelStr").val(data.NeedDelStr);
}

//监听提交
form.on('submit(form)', function (data) {
    layer.alert(JSON.stringify(data.field), {
        title: '最终的提交信息'
    })
    return false;
});

$(document).ready(function () {
    Init();    
});
