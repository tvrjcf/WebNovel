﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace WebBookManage.Common
{
    public static class DoubleBufferDataGridView
    {
        /// <summary>  
        /// 双缓冲，解决闪烁问题  
        /// </summary>  
        /// <param name="dgv"></param>  
        /// <param name="flag"></param>  
        public static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    }

}
