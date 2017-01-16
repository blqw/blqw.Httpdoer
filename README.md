# blqw.HttpRequest
简化http请求操作

### Demo
```csharp
static void Main(string[] args)
{
    var www = new Httpdoer("https://api.datamarket.azure.com");
    www.Method = HttpRequestMethod.GET;
    www.AutoRedirect = false; //是否自动跳转
    www.Path = "Bing/MicrosoftTranslator/v1/Translate";
    www.Query.AddModle(new {
        Text = "'hello world'",
        To = "'zh-CHS'"
    });
    www.Headers.Add("Authorization", AUTH_TOKEN);            
    var str = www.GetString();            
    Console.WriteLine();
    Console.WriteLine(GetText(str));
}
```

### 更新日志 
#### [1.5.4.9] 2017.01.16
* 新增控制`Query`参数中数组和对象的名称解析方式的枚举`ArrayEncodeMode`和`ObjectEncodeMode`

#### [1.5.4.8] 2017.01.11
* 修复一个逻辑上的错误,多个`Httpdoer`不在共享一个日志对象,而是共享全局侦听器
* 更新依赖组件

#### [1.5.4.6] 2016.11.30
* 修复无法获取`IHttpBodyParser`的问题

#### [1.5.4.4] 2016.11.24
* 更新nuget下载没有dll的问题
* 更新ioc组件版本

#### [1.5.4.2] 2016.11.17
* 修复bug

#### [1.5.4.1] 2016.11.15
* 修复同步模式下返回值有可能被截断的bug
* 修复`ResponseRaw`有时候会抛出异常的bug

#### [1.5.4] 2016.11.14
* 优化默认头信息的插入时间
* 优化`HttpMethod`,`Content-Type`等参数的默认值计算方式
* 优化`Delete`,`Put`,`Patch`方式提交对`Content-Type`影响

#### [1.5.3.1] 2016.11.11
* 修复`Body.ToString()`在某种情况下返回`null`的问题

#### [1.5.3] 2016.11.10
* 修复`HttpContentType.ChangeCharset`方法返回值错误的问题
* 优化当响应头的`Content-Type`中不存在`charset`属性时,从`Content-Encoding`中获取

#### [1.5.2] 2016.11.04
* 新增一些静态方法`Httpdoer.Get`等
* 修复请求没有Path的Url时,会在结尾处加上`/`的问题
* 优化当domain,path,query,对query参数合并的处理逻辑
* 新增`ToString('q')`方法可以返回带参数的请求地址

#### [1.5.1] 2016.10.27
* 修复提交json数据时格式错误的问题

#### [1.5.0-beta] 2016.10.21
* 新增请求支持设置 `IWebProxy`

#### [1.4.1-beta] 2016.10.07
* 修复访问某些网站或者API时返回`分析URI“xxx”的 Cookie 标头时出错`的问题

#### [1.4.0-beta] 2016.10.07
* 使用IOC优化自定义`ContentType`的处理方式
* 更新`blqw.IOC`

#### [1.3.2] 2016.10.05
* 优化自定义ContentType和Body时的编码方式
* 优化日志输出
* 优化代码格式
* 完善注释

#### [1.3.1.1] 2016.09.29 
* 修复请求中出现302跳转时,中间页面产生Cookie的处理逻辑

#### [1.3.1] 2016.09.29 
* 修复`Response.Headers`部分数据无法获取的问题

#### [1.3.0] 2016.09.09(1.3.0)
* 优化某些情况下的异常信息,更详细
* 修复异步模式下.302跳转中的Cookie丢失的问题
* 增加是否使用Cookie全局缓存可以设置
* 增加`AutoRedirect`属性,可控制302行为

#### 2016.07.26
* 修复不设置Method的情况下报错的问题

#### 2016.07.18
* 修复问题若干,发布正式版1.2正式版  
* 加入演示项目buibuiapi  

#### 2016.07.07
* 增加自定义Method的功能  
* 增加 Response.Headers 赋值
* 修复当Cookie不存在时读取Cookie报错的问题
* 优化默认Header的插入行为  
* 修复其他小问题若干  

#### 2016.07.05
* 更新IOC组件  

#### 2016.07.04
* 增加ContentType可以直接设定解析器  
* 增加Stream解析器  
* 增加`HttpBody.Wirte(byte[])`方法
* 增加HttpRequest拓展方法
* 优化部分逻辑

#### 2016.06.30
* 输出注释文件
* IHttpRequest增加一个属性FullUrl
* 现在可以绑定多个日志记录器和跟踪器

#### 2016.04.15
* 更新MEF  

#### 2016.04.14
* 新增同步版本,基于原有的HttpWebRequest实现  
* 优化异步版本,基于HttpClient重新实现  

#### 2016.04.11
* 增加一个连接池管理长时间无法结束的请求  
  
#### 2016.03.16  
* 修复EMF插件加载的一个bug  
* 修复复杂对象使用Json方式调试时的一个bug  
* 增加单元测试  
  
#### 2016.02.21  
* 修复在某些情况下初始化会出现错误的问题  
* 支持提交Json正文  

```csharp
static void Main(string[] args)
{
    var www = new HttpRequest("www.x.com");
    www.Path = "yyy/zzz";
    www.FormBody += new { id = 1, name = "blqw" };
    www.FormBody.ContentType = ContentType.ApplicationJson;
    www.GetString();
}
```
