# blqw.HttpRequest
简化http请求操作

### Demo
```csharp
static void Main(string[] args)
{
    var www = new HttpRequest("https://api.datamarket.azure.com");
    www.Method = HttpRequestMethod.GET;
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