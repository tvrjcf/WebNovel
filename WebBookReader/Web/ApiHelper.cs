using CYQ.Data.Table;
using CYQ.Data.Tool;
using MiniBlinkPinvoke;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WebBookManage.Common;

namespace WebBookReader.Web
{
    public class ApiHelper
    {
        BookHelper BH = null; 
        public ApiHelper() {
            BH = new BookHelper();
        }

        [JSFunctin]
        public void ApiTest(string msg)
        {
            MessageBox.Show("ApiTest : " + msg);
        }

        [JSFunctin]
        public string GetNovelTypes()
        {
            var data = BH.GetNovelTypes().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        [JSFunctin]
        public string GetNovels()
        {
            var data = BH.GetNovels().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        [JSFunctin]
        public string UpdateNovel(string novelId)
        {
            var list = BH.DownMenuList(novelId);
            var data = JsonHelper.ToJson(list);// JsonConvert.SerializeObject(list).Replace("null", "\"\"");
            return data;
        }

        public string GetBookListHtml() {
            var html = @"<div class='yd-book-item yd-book-item-pull-left'>
                                    <a href='#' target='_blank'> <img src='./res/Default.png' alt='人山人海里，你不必记得我' class='pull-left cover-container' width='114' height='160' /> <h2>人山人海里，你不必记得我</h2> </a>
                                    <div class='author-container'>
                                        <dl class='dl-horizontal-inline'>
                                            <dt>
                                                作者：
                                            </dt>
                                            <dd>
                                                金浩森
                                            </dd>
                                        </dl>
                                    </div>
                                    <div class='rate w-star w-star1'>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span class='no'>&nbsp;</span>
                                    </div>
                                    <div class='price-container f-invi'>
                                        <b class='price'> ￥9.90 </b>
                                    </div>
                                    <div class='summery'>
                                        <p>《人山人海里，你不必记得我》是金浩森在七年沉淀后的内心之作。十篇异国旅途日记、十个国家的写真及摄影作品，描述出这些年在旅行路上对生活的反思，以及收获到对世界的认知。同时，通过对自我的发问，写给六个“你”的真诚书信，全书以文字作品、个人写真、摄影作品的立体形式，呈现了一段褪去浮躁与复杂、归于简单与纯真的成长路途。我想一直这样走下去，去看世界上所有的魅力与孤独，无论在哪里，做什么样的事情，只要能和喜欢的人在一起。</p>
                                    </div>
                                    <div class='badge badge-complete png'></div>
                                </div>";

            return html;
        }
    }
}
