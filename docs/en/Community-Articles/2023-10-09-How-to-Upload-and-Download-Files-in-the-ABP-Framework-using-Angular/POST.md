# How to Upload and Download Files in the ABP Framework using Angular
In this article, I will describe how to upload and download files with the ABP framework using Angular as the UI template, most of the code is compatible with all template types. In this article, I just gather some information in one post. Nothing is new. I Googled how to manage files in ABP and I didn't find anything. So I decided write a simple article as an answer to this question. 

### Creating App Service.

An empty AppService that uses `IRemoteStreamContent` was created. ABP describes the IRemoteStreamContent as:

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

When you want to upload a file, app service param must be IRemoteStreamContent or RemoteStreamContent. You should be able to access a file data with the getStream method in the AppService. After that, There is no ABP spesific code. It's a c# spesific class. You can save a file system, move somewhere or serilize as base64 etc. 

when you want to download a file, A method should return IRemoteStreamContent or RemoteStreamContent. 
RemoteStreamContent gets a required parameter. The parameter type must be Stream. (FileStream, MemoryStream, Custom etc...)

For more information please read the topic in the Documentation: https://docs.abp.io/en/abp/latest/Application-Services#working-with-streams

### Creating Angular proxy services

ABP creates proxy files with the `abp generate-proxy -t ng` command. let's check the code.

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
The function name is a little bit weird but let's focus the first parameter. The type of file param is `FormData`. FormData is a native object in JavaSript Web API. See the details. https://developer.mozilla.org/en-US/docs/Web/API/FormData . 

How to use the `uploadFileByFile` function.

```javascript
const myFormData = new FormData();
myFormData.append('file', inputFile); // file must match variable name in AppService
storageService.uploadFileByFile(myFormData).subscribe()
```
The inputFile type is File. In most cases it comes from the `<input type="File">`, File belongs to the Javacsript Web Api. see the details https://developer.mozilla.org/en-US/docs/Web/API/File


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

The return type of function is Blob. Blob is another javacript object. See the details: https://developer.mozilla.org/en-US/docs/Web/API/Blob.

Now our code is not ABP Spesific. It is just javascript code. If you don't want to save the blob, here I asked my best AI friend ChatGPT. `Hello, chat! The programming lang is javascript. My variable type is Blob. How do I save file to client's machine?`   

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

