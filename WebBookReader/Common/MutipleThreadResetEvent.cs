﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BR.Common
{
    /// <summary>
    ///  封装ManualResetEvent
    /// </summary>
    public class MutipleThreadResetEvent : IDisposable
    {
        private readonly ManualResetEvent done;
        private readonly int total;
        private long current;
        public int index;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="total">需要等待执行的线程总数</param>
        public MutipleThreadResetEvent(int total)
        {
            this.total = total;
            current = total;
            index = 0;
            done = new ManualResetEvent(false);
        }

        /// <summary>
        /// 唤醒一个等待的线程
        /// </summary>
        public void SetOne()
        {
            // Interlocked 原子操作类 ,此处将计数器减1
            if (Interlocked.Decrement(ref current) == 0)
            {
                //当所以等待线程执行完毕时，唤醒等待的线程
                done.Set();
            }
            Interlocked.Increment(ref index);
            Console.WriteLine(index);
        }

        /// <summary>
        /// 等待所以线程执行完毕
        /// </summary>
        public void WaitAll()
        {
            done.WaitOne();
        }

        /// <summary>
        /// 释放对象占用的空间
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)done).Dispose();
        }
    }
}
