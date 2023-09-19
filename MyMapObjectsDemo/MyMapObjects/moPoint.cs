using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public class moPoint: moGeometry
    {
        #region 字段
        private double _X, _Y;
        #endregion

        #region 构造函数
        /// <summary>
        /// 空构造函数
        /// </summary>
        public moPoint()
        {

        }

        /// <summary>
        /// 带参数的构造函数，初始化xy坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public moPoint(double x, double y)
        {
            _X = x;
            _Y = y;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置X坐标
        /// </summary>
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        /// <summary>
        /// 获取或设置Y坐标
        /// </summary>
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 复制一个点
        /// </summary>
        /// <returns></returns>
        public moPoint Clone()
        {
            moPoint sPoint = new moPoint(_X, _Y);
            return sPoint;
        }
        #endregion
    }
}
