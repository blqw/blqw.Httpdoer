namespace blqw.Web
{
    /// <summary>
    /// 表示值为字符串的参数集合
    /// </summary>
    public sealed class HttpStringParams : HttpParamsBase<string>
    {
        internal HttpStringParams(IHttpParameterContainer @params, HttpParamLocation location)
            : base(@params, location)
        {
        }


        /// <summary>
        /// 获取或设置指定名称的参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public override string this[string name]
        {
            get
            {
                var r = Params.Get(Location, name);
                if (r.IsMultiValue)
                {
                    return string.Join(",", r.Values);
                }
                return (string) r.FirstValue;
            }
            set { base[name] = value; }
        }
    }
}