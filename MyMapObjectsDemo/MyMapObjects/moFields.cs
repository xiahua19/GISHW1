﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 字段集合类型
    /// </summary>
    public class moFields
    {
        #region 字段
        private List<moField> _Fields;      // 字段集合
        private string _PrimaryField = "";  // 主字段名称
        private bool _ShowAlias = false;    // 是否显示别名
        #endregion

        #region 构造函数
        public moFields()
        {
            _Fields = new List<moField>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取字段数目
        /// </summary>
        public Int32 Count
        {
            get { return _Fields.Count; }
        }

        /// <summary>
        /// 获取或设置主字段
        /// </summary>
        public string PrimaryField
        {
            get { return _PrimaryField; }
            set { _PrimaryField = value; }
        }

        /// <summary>
        /// 指示是否显示别名
        /// </summary>
        public bool ShowAlias
        {
            get { return _ShowAlias; }
            set { _ShowAlias = value; }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 查找指定名称的字段并返回其索引号，如无则返回-1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Int32 FindField(string name)
        {
            Int32 sFieldCount = _Fields.Count;
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                if (_Fields[i].Name.ToLower() == name.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取指定索引号的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moField GetItem(Int32 index)
        {
            return _Fields[index];
        }

        /// <summary>
        /// 获取指定名称的字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public moField GetItem(string name)
        {
            Int32 sIndex = FindField(name);
            if (sIndex >= 0)
                return _Fields[sIndex];
            else
                return null;
        }

        /// <summary>
        /// 追加一个字段
        /// </summary>
        /// <param name="field"></param>
        public void Append(moField field)
        {
            if(FindField(field.Name) >= 0)
            {
                string sMessage = MyMapObjects.Properties.Resources.String001;
                throw new Exception(sMessage);
            }
            _Fields.Add(field);

            // 触发事件
            if(FieldAppended != null)
                FieldAppended(this, field);
        }

        /// <summary>
        /// 删除指定索引号的字段
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            moField sField = _Fields[index];
            _Fields.RemoveAt(index);

            // 触发事件
            if (FieldRemoved != null)
                FieldRemoved(this, index, sField);
        }

        #endregion

        #region 事件
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="sender">发出事件的对象</param>
        /// <param name="fieldIndex">被删除的字段在删除之前的索引号</param>
        /// <param name="fieldRemoved">被删除的字段对象</param>
        internal delegate void FieldRemovedHandle(object sender, Int32 fieldIndex, moField fieldRemoved);

        internal delegate void FieldAppendedHandle(object sender, moField fieldAppended);


        /// <summary>
        /// 有字段被删除了
        /// </summary>
        internal event FieldRemovedHandle FieldRemoved;

        /// <summary>
        /// 有字段被加入
        /// </summary>
        internal event FieldAppendedHandle FieldAppended;
        #endregion

        #region 私有函数

        #endregion
    }
}
