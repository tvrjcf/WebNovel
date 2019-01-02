var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var book = JSON.parse(localStorage.getItem("editbook"));    //parent.updateList;
//var bookstr = "{\"NovelID\":\"000003\",\"NovelName\":\"吞噬星空\",\"bIsEnd\":false,\"ListUrl\":\"http://www.cilook.net/book/0/31/index.html\",\"ContentStart\":\"<div id=\\\"content\\\">\",\"ContentEnd\":\"</div>\",\"LB\":\"07\",\"Displayorder\":2,\"NeedDelUrl\":\"http://www.cilook.net/book/0/31/1440155.html(第二十九篇 第三十六章 超脱轮回(大结局)上)||http://www.cilook.net/book/0/31/1440163.html(第二十九篇 第三十六章 超脱轮回（大结局）下)||http://www.cilook.net/(思路客小说网)||http://www.cilook.net/b/1_1.html(玄幻魔法)||http://www.cilook.net/book/0/31/(吞噬星空最新章节列表)||http://www.cilook.net/modules/article/addbookcase.php?bid=31(加入书架)||http://www.cilook.net/modules/article/uservote.php?id=31(推荐本书)||http://www.cilook.net/b/31.html(吞噬星空)\"}";
//book = JSON.parse(bookstr);
//alert(localStorage.getItem("editbook").replace("/\\/", "") +"\n\n"+bookstr);

function layerAlert(msg) {
    layer.alert(msg, { /*offset: '100px'*/ });
}
function layerMsg(msg) {
    layer.msg(msg, { /*offset: '100px'*/ });
}

function Init() {
    if (parent.bookTypes != null) {
        $("#LB").empty();
        $(parent.bookTypes).each(function (i, item) {
            $("#LB").append("<option value='" + item.DM + "'>" + item.MC + "</option>");
        });
    }

    if (book != null) {
        BindForm(book);
    }

    $(".btn-http-change").on('click', function () {
        var url = $("#ListUrl").val();
        var novelId = $("#NovelID").val();
        //layerMsg(parent.ChangeHttpMode(url, novelId) == -1);
        if (parent.ChangeHttpMode(url, novelId) == 1) {
            layerMsg("切换成功!");
            if (url.indexOf("https") >= 0) {
                url = url.replace("https", "http");
            } else {
                url = url.replace("http", "https");

            }

            $("#ListUrl").val(url);
        }
    });

    $(".btn-sign-get").on('click', function () {
        var url = $("#ListUrl").val();
        var sign = JSON.parse(parent.GetSiteSign(url));
        if (sign && sign.url) {
            $("#ContentStart").val(sign.ContentStart);
            $("#ContentEnd").val(sign.ContentEnd);
            $("#ListStart").val(sign.ListStart);
            $("#ListEnd").val(sign.ListEnd);
            $("#VolumeStart").val(sign.VolumeStart);
            $("#VolumeEnd").val(sign.VolumeEnd);
            $("#NeedDelStr").val(sign.NeedDelStr);
            layerMsg("取出成功");
        } else {
            layerMsg("还未收录过该站点标志信息");
        }
    });

    //$(".btn-sign-save").on('click', function () {

    //    var result = JSON.parse(parent.SaveSiteSign(JSON.stringify(data.field)));
    //    if (!result.Success) {
    //        layerAlert(result.Message); return false;
    //    }        
    //    layerMsg("正文标志已保存!");
    //    return false;
    //});

    $(".btn-sign-manage").on('click', function () {
        
    });
}

/**
 * 显示更新列表
 * @param {any} data
 */
function BindForm(data) {
    //alert(JSON.stringify(data));
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
    //form.render(); //更新全部
}

//监听提交
form.on('submit(formSubmit)', function (data) {
    //layer.alert(JSON.stringify(data.field), {
    //    title: '最终的提交信息'
    //});
    var result = JSON.parse(parent.SaveNovel(JSON.stringify(data.field)));
    if (!result.Success) {
        layerAlert(result.Message); return false;
    }
    var save = JSON.parse(result.Data);
    parent.PushBook(new Array(save));
    layerMsg("保存成功!\n" + "[" + save.NovelID + "][" + save.NovelName + "]");
    return false;
});
form.on('submit(formSaveToSign)', function (data) {
    var result = JSON.parse(parent.SaveSiteSign(JSON.stringify(data.field)));
    if (!result.Success) {
        layerAlert(result.Message); return false;
    }
    layerMsg("正文标志已保存!");
    return false;
});

$(document).ready(function () {
    Init();
});
