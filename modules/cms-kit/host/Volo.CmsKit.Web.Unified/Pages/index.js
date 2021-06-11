$(function(){
    var fileUploadUri = "/api/cms-kit-admin/media";
    var fileUriPrefix = "/api/cms-kit/media/"
    
    var editor = $('#content-editor');

    function getUppyHeaders() {
        var headers = {};
        headers[abp.security.antiForgery.tokenHeaderName] = abp.security.antiForgery.getToken();

        return headers;
    }
    
    var uploadImage = function (blob, callback, source) {
        var UPPY_OPTIONS = {
            endpoint: fileUploadUri,
            formData: true,
            fieldName: "file",
            method: "post",
            headers: getUppyHeaders()
        };

        var UPPY = Uppy.Core().use(Uppy.XHRUpload, UPPY_OPTIONS);

        UPPY.reset();

        UPPY.addFile({
            id: "content-file",
            name: blob.name, 
            type: blob.type, 
            data: blob, 
        });
        
        UPPY.upload().then((result) => {
            if (result.failed.length > 0) {
                abp.message.error("File upload failed");
            } else {
                var mediaDto = result.successful[0].response.body;
                var fileUrl = (fileUriPrefix + mediaDto.id);
                
                callback(fileUrl, mediaDto.name);
            }
        });
    };

    new toastui.Editor({
        el: editor[0],
        usageStatistics: false,
        useCommandShortcut: true,
        initialEditType: 'wysiwyg',
        previewStyle: 'tab',
        height: "25em",
        minHeight: "25em",
        language: abp.localization.currentCulture.cultureName, 
        hooks: {
            addImageBlobHook: uploadImage,
       },
    });
});