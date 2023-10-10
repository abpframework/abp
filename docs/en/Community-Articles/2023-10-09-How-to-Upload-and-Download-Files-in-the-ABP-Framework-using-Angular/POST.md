# How to Upload and Download Files in the ABP Framework using Angular
In this article, I will describe how to upload and download files in the ABP framework. While I will use Angular as the UI template option, most of the code is compatible with all template types. In the article I just gather the some information in one post. Nothing is new. I searched How manage files in Abp in Search engine and I didn't find. So I decided write a simple article. 

### Creating App Service.

An empty AppService that uses `IRemoteStreamContent` was created. What is  IRemoteStreamContent, ABP Documentation described:

> ABP Framework provides a special type, IRemoteStreamContent to be used to get or return streams in the application services.

```csharp
public class StorageAppService: AbpFileUploadDownloadDemoAppService // <- a inherited from ApplicationService. `ProjectName`+'AppService'.
{
    public Guid UploadFile(IRemoteStreamContent file)
    {
        Stream fs = file.GetStream();
        
        //save your file with guid or implement your logic
        var id = Guid.NewGuid();
        var filePath = "Insert your file path here/" + id.ToString();
        using var stream = new FileStream(filePath, FileMode.Create);   
        fs.CopyTo(stream);
        return id;
    }
      public IRemoteStreamContent DownloadFile(Guid FileName)
    {
        //find your file with guid or implement your logic 
        var filePath = "Insert your file path here" ;
        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return new RemoteStreamContent(fs);
    }
}
```

When you want to upload a file, app service param must be IRemoteStreamContent or RemoteStreamContent. You be able to access a file data with getStream method in AppService. After that, There s no ABP spesific code. is a  c# spesific class. You can save a file system, move somewhere or serilize as base64 etc. 

when you want to download file, A method should return IRemoteStreamContent or RemoteStreamContent. 
RemoteStreamContent get a required parameter. The parameter type must be Stream. (FileStream,MemoryStream, Custom etc...)

More information please read the topic in the Documentation:  https://docs.abp.io/en/abp/latest/Application-Services#working-with-streams

### Creating Angular proxy services

ABP create proxy files `abp generate-proxy -t ng` command. let's check the code.

```javascript

 uploadFileByFile = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/storage/upload-file',
      body: file,
    },
    { apiName: this.apiName,...config });

```
Function name a little bit wierd but let's focus the first parameter. The type of file param is `FormData`. FormData is a native object in JavaSript Web API. See the detail.  https://developer.mozilla.org/en-US/docs/Web/API/FormData . 

How to use `uploadFileByFile` function.

```javascript
const myFormData = new FormData();
myFormData.append('file', inputFile); // file must match variable name in AppService
storageService.uploadFileByFile(myFormData).subscribe()
```
 inputFile type is File. Most case it come from `<input type="File">` File is belong to Javacsript Web Api. see the detail https://developer.mozilla.org/en-US/docs/Web/API/File


Let's continue with "download"

```javascript

  downloadFileByFileName = (FileName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'POST',
      responseType: 'blob',
      url: '/api/app/storage/download-file',
      params: { fileName: FileName },
    },
    { apiName: this.apiName,...config });

```

The return type of function is Blob. the Blob is another javacript object. See the detail: https://developer.mozilla.org/en-US/docs/Web/API/Blob.

Now our code is not ABP Spesific. It is just javascript code. If you don't want to save blob, here I asked my best Ai friend ChatGPT. `Hello, chat! The programming lang is javascript. My variable type is Blob. How do I save file to client's machine?`   

Our the gifted friend gives us the code
```javascript
function saveBlobToFile(blob, fileName) {
    // Create a blob URL
    const blobURL = window.URL.createObjectURL(blob);

    // Create an anchor element for the download
    const a = document.createElement("a");
    a.href = blobURL;
    a.download = fileName || 'download.dat'; // Provide a default file name if none is provided

    // Append the anchor to the document
    document.body.appendChild(a);

    // Simulate a click on the anchor to initiate the download
    a.click();

    // Clean up: remove the anchor and revoke the blob URL
    document.body.removeChild(a);
    window.URL.revokeObjectURL(blobURL);
}

// Usage example
const blob = new Blob(['Hello, World!'], { type: 'text/plain' });
saveBlobToFile(blob, 'hello.txt');
``` 

