var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var book = JSON.parse(localStorage.getItem("editbook"));    //parent.updateList;
var listUrl = "";

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
        BindSign(book.ListUrl);
    }

    //$("#ListUrl").on('change', function () {
    //    listUrl = $("#ListUrl").val();
    //});

    $("#ListUrl").on('blur', function () {
        var url = $("#ListUrl").val();
        BindSign(url);
    });

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
        BindSign(url);
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
 * 绑定表单
 * @param {object} data
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

/**
 * 绑定正文标志
 * @param {string} url
 */
function BindSign(url) {
    if ((url && url.length == 0) || listUrl == url) return false;
    var sign = JSON.parse(parent.GetSiteSign(url));
    if (sign && sign.url) {
        $("#ContentStart").val(sign.ContentStart);
        $("#ContentEnd").val(sign.ContentEnd);
        $("#ListStart").val(sign.ListStart);
        $("#ListEnd").val(sign.ListEnd);
        $("#VolumeStart").val(sign.VolumeStart);
        $("#VolumeEnd").val(sign.VolumeEnd);
        $("#NeedDelStr").val(sign.NeedDelStr);

        $("#BriefUrlStart").val(sign.BriefUrlStart);
        $("#BriefUrlEnd").val(sign.BriefUrlEnd);
        $("#AuthorStart").val(sign.AuthorStart);
        $("#AuthorEnd").val(sign.AuthorEnd);
        $("#BookImgUrlStart").val(sign.BookImgUrlStart);
        $("#BookImgUrlEnd").val(sign.BookImgUrlEnd);
        $("#BriefStart").val(sign.BriefStart);
        $("#BriefEnd").val(sign.BriefEnd);

        layerMsg("标志取出成功");
        listUrl = url;
    } else {
        layerMsg("还未收录过该站点标志信息");
    }
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
    parent.ReflushBook(new Array(save));
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
