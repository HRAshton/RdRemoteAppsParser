# RdRemoteAppsParser

RemoteApps web page parser

### Features:

 * Parse RDWA RemoteApps web pages to models with App names, .rdp and .png urls.

## Installation

```
Install-Package RdRemoteAppsParser
```

## Usage

```csharp
const string domain = "https://remoteapps.example.com";
const string user = "user005";
const string pass = "god";

await client.Connect(domain, user, pass);
var folderModel = await client.GetAllRemoteElements();
var firstUrl = folderModel.Apps.FirstOrDefault()?.RdpFileUrl;
// firstUrl = "/RDWeb/Pages/rdp/cpub-chrome-CmsRdsh.rdp"
```

### Options:

You can configure a proxy server using constructor's optional argument.

## License

(The MIT License)

Copyright (c) 2020 HRAshton &lt;HRAshton@yandex.com&gt;

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
'Software'), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
