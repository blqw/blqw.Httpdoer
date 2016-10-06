using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.Web
{
    /// <summary>
    /// 表示影响正文延迟反序列的参数集合,集合是只读的
    /// </summary>
    internal class LazyDeserializeBodyParameters : HttpParameterContainer, IHttpParameterContainer
    {
        /// <summary>
        /// 尚未反序列化的字节流
        /// </summary>
        private readonly byte[] _bytes;
        /// <summary>
        /// 响应正文内容类型 
        /// </summary>
        private readonly HttpContentType _contentType;
        /// <summary>
        /// 是否已经初始化完成
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="contentType">响应正文内容类型</param>
        /// <param name="bytes">是否已经初始化完成</param>
        public LazyDeserializeBodyParameters(HttpContentType contentType, byte[] bytes)
        {
            _contentType = contentType;
            _bytes = bytes;
            _isInitialized = false;
        }


        void IHttpParameterContainer.AddValue(HttpParamLocation location, string name, object value)
        {
            if (location == HttpParamLocation.Body)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized)
            {
                Initialize();
            }
            AddValue(location, name, value);
        }

        bool IHttpParameterContainer.Contains(HttpParamLocation location, string name)
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return Contains(location, name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return GetEnumerator();
        }

        IEnumerator<HttpParamValue> IEnumerable<HttpParamValue>.GetEnumerator()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return GetEnumerator();
        }

        object IHttpParameterContainer.GetValue(HttpParamLocation location, string name)
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return GetValue(location, name);
        }

        IEnumerable IHttpParameterContainer.GetValues(HttpParamLocation location, string name)
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            return Get(location, name).Values;
        }

        void IHttpParameterContainer.Remove(HttpParamLocation location, string name)
        {
            if (location == HttpParamLocation.Body)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized)
            {
                Initialize();
            }
            Remove(location, name);
        }

        void IHttpParameterContainer.SetValue(HttpParamLocation location, string name, object value)
        {
            if (location == HttpParamLocation.Body)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized)
            {
                Initialize();
            }
            SetValue(location, name, value);
        }

        private void Initialize()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            var parser = _contentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            var @params = parser?.Deserialize(_bytes, _contentType);
            if (@params != null)
            {
                foreach (var item in @params)
                {
                    SetValue(HttpParamLocation.Body, item.Key, item.Value);
                }
            }
            _isInitialized = true;
        }
    }
}