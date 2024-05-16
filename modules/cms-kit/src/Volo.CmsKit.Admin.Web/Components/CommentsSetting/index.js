
//$(document).ready(function () {
//    var service = volo.cmsKit.admin.comments.commentAdmin;
//    service.getSettings().done(function (response) {
//        var isChecked = response.requireApprovement
//        $('#checkbox').prop('checked', isChecked);
//        console.log('CheckBox durumu başarıyla güncellendi:', response.requireApprovement);
//    }).fail(function (error) {
//        console.error('CheckBox durumu alınırken bir hata oluştu:', error);
//    });

//    $('#save').click(function () {
//        var isChecked = $('#checkbox').prop('checked');

//        service.setSettings({ RequireApprovement: isChecked }).done(function (response) {
//            console.log('CheckBox durumu başarıyla güncellendi:', response);
//            alert('CheckBox durumu başarıyla kaydedildi.');
//        }).fail(function (error) {
//            console.error('CheckBox durumu güncellenirken bir hata oluştu:', error);
//            alert('CheckBox durumu kaydedilirken bir hata oluştu.');
//        });
//    });
//});
var service = volo.cmsKit.admin.comments.commentAdmin;

$(document).ready(function () {
    service.getSettings().done(function (response) {
        var isChecked = response.requireApprovement
        $('#checkbox').prop('checked', isChecked);
        console.log('CheckBox durumu başarıyla güncellendi:', response.requireApprovement);
    }).fail(function (error) {
        console.error('CheckBox durumu alınırken bir hata oluştu:', error);
    });

    $('#save').click(function () {
        var isChecked = $('#checkbox').prop('checked');

        service.setSettings({ RequireApprovement: isChecked }).done(function (response) {
            console.log('CheckBox durumu başarıyla güncellendi:', response);
            alert('CheckBox durumu başarıyla kaydedildi.');
        }).fail(function (error) {
            console.error('CheckBox durumu güncellenirken bir hata oluştu:', error);
            alert('CheckBox durumu kaydedilirken bir hata oluştu.');
        });
    });
});
