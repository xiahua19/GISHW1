﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public class moFeatures
    {
        #region 字段
        private List<moFeature> _Features;  // 要素集合
        #endregion

        #region 构造函数
        public moFeatures()
        {
            _Features = new List<moFeature>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取要素数目
        /// </summary>
        public Int32 Count
        {
            get { return _Features.Count; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取指定索引号的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moFeature GetItem(Int32 index)
        {
            return _Features[index];
        }

        /// <summary>
        /// 设置指定索引号的元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="feature"></param>
        public void SetItem(Int32 index, moFeature feature)
        {
            _Features[index] = feature;
        }

        /// <summary>
        /// 添加一个元素
        /// </summary>
        /// <param name="feature"></param>
        public void Add(moFeature feature)
        {
            _Features.Add(feature);
        }

        /// <summary>
        /// 删除指定索引号的元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            _Features.RemoveAt(index);
        }

        /// <summary>
        /// 获取指定要素的索引
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public Int32 FindIndex(moFeature feature)
        {
            return _Features.FindIndex(e => e == feature);
        }

        /// <summary>
        /// 清除所有元素
        /// </summary>
        public void Clear()
        {
            _Features.Clear();
        }
        #endregion
    }
}
