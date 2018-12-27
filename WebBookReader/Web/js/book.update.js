var $ = layui.$ //重点处;
var layer = layui.layer;
var form = layui.form;
var table = layui.table;
var element = layui.element;

var downLoadData = JSON.parse(localStorage.getItem("downLoadData"));    //parent.updateList;

function layerAlert(msg) {
    layer.alert(msg, { offset: '100px' });
}
function layerMsg(msg) {
    layer.msg(msg, { offset: '100px' });
}

function SetProgressValue(value) {
    element.progress('downLoadProgress', value + "%");
    if (value != 0) $(".layui-progress").show();
    else $(".layui-progress").hide();
}

function Init() {
    SetProgressValue(0);
    table.render({
        id: 'menuList'
        , elem: '#menuList'
        , toolbar: '#toolbar'
        , cols: [[ //标题栏
            { type: 'checkbox' }
            , { field: 'Id', title: 'ID', width: 70, sort: true }
            , { field: 'Title', title: '标题', width: 200, templet: '#TitleTpl' }
            //, { field: 'Volume', title: '分卷名', minWidth: 150 }
            , { field: 'ComeFrom', title: '地址', width: 400 }
            //, { field: 'city', title: '城市', width: 100 }
            //, { field: 'experience', title: '积分', width: 80, sort: true }
        ]]
        //, data: []
        //,skin: 'line' //表格风格
        , even: true
        , page: true //是否显示分页
        , limits: [100, 200, 500, 1000]
        , limit: 500 //每页默认显示的数量
        , height: 'full-40'

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
            case 'reload':                
                BindData(downLoadData);
                layerMsg('数据已刷新');
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
                        var ret = parent.DownLoadContent(JSON.stringify(data));
                        if (ret == "-1") {
                            layerMsg('下载已取消!'); return;
                        } else if (ret == "0") {
                            var value = 0;
                            var setInterval = self.setInterval(function () {
                                value = parent.UpdateProgressValue();
                                SetProgressValue(value);
                                if (value >= 100) {
                                    window.clearInterval(setInterval)
                                    layerMsg('下载完成!');
                                }
                            }, 500)
                        } else {
                            layerAlert(ret);
                        }
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
                        $(data).each(function (i, item) {
                            $(downLoadData).each(function (j, update) {
                                if (item.ComeFrom == update.ComeFrom) {
                                    downLoadData.splice(j, 1);
                                }
                            });
                        });
                        layer.close(index);
                        BindData(downLoadData);    
                    });
                break;
        };
    });
}

/**
 * 绑定数据
 * @param {any} data
 */
function BindData(data) {

    //alert(data.length);
    table.reload('menuList', { data: data });
}
/**
 * 重置大小
 * */
function Resize() {
    //alert('resize');
    table.reload('menuList', { height: 'full-40'});
}


$(document).ready(function () {
    Init();
    BindData(downLoadData);
    
});
