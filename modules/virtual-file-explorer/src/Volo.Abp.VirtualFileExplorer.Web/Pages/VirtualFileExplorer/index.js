var _fileContentModal = new abp.ModalManager(
    abp.appPath + 'VirtualFileExplorer/FileContentModal'
);

function showContent(filePath) {
    _fileContentModal.open({
        filePath: filePath,
    });
}
