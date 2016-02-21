# blqw.HttpRequest
简化http请求操作

### Demo
```csharp
static void Main(string[] args)
{
    var www = new HttpRequest("https://api.datamarket.azure.com");
    www.Method = HttpRequestMethod.GET;
    www.Path = "Bing/MicrosoftTranslator/v1/Translate";
    www.QueryString += new {
        Text = "'hello world'",
        To = "'zh-CHS'"
    };
    www.Headers.Add("Authorization", AUTH_TOKEN);            
    var str = www.GetString().Result;            
    Console.WriteLine();
    Console.WriteLine(GetText(str));
}
```

### 更新日志
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