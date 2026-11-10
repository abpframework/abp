(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else if (typeof module === "object" && module.exports) {
		module.exports = factory( require( "jquery" ) );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: AR (Arabic; العربية)
 */
$.extend( $.validator.messages, {
	required: "هذا الحقل إلزامي",
	remote: "يرجى تصحيح هذا الحقل للمتابعة",
	email: "رجاء إدخال عنوان بريد إلكتروني صحيح",
	url: "رجاء إدخال عنوان موقع إلكتروني صحيح",
	date: "رجاء إدخال تاريخ صحيح",
	dateISO: "رجاء إدخال تاريخ صحيح (ISO)",
	number: "رجاء إدخال عدد بطريقة صحيحة",
	digits: "رجاء إدخال أرقام فقط",
	creditcard: "رجاء إدخال رقم بطاقة ائتمان صحيح",
	equalTo: "رجاء إدخال نفس القيمة",
	extension: "رجاء إدخال ملف بامتداد موافق عليه",
	maxlength: $.validator.format( "الحد الأقصى لعدد الحروف هو {0}" ),
	minlength: $.validator.format( "الحد الأدنى لعدد الحروف هو {0}" ),
	rangelength: $.validator.format( "عدد الحروف يجب أن يكون بين {0} و {1}" ),
	range: $.validator.format( "رجاء إدخال عدد قيمته بين {0} و {1}" ),
	max: $.validator.format( "رجاء إدخال عدد أقل من أو يساوي {0}" ),
	min: $.validator.format( "رجاء إدخال عدد أكبر من أو يساوي {0}" ),
	step: $.validator.format( "يرجى تقديم قيمة من مضاعفات {0}" ),
	maxWords: $.validator.format( "يرجى تقديم ما لا يزيد عن {0} كلمات" ),
	minWords: $.validator.format( "يرجى تقديم {0} كلمات على الأقل" ),
	rangeWords: $.validator.format( "يرجى تقديم ما بين {0} و{1} كلمة" ),
	letterswithbasicpunc: "يرجى تقديم الحروف وعلامات الترقيم فقط",
	alphanumeric: "يرجى تقديم الحروف والأرقام والمسافات والتسطير فقط",
	lettersonly: "يرجى تقديم الحروف فقط",
	nowhitespace: "من فضلك لا تدخل المساحات البيضاء",
	ziprange: "يرجى تقديم الرمز البريدي بين 902xx-xxxx و905-xx-xxxx",
	integer: "يرجى تقديم رقم غير عشري موجب أو سالب",
	vinUS: "يرجى تقديم رقم تعريف المركبة (VIN)",
	dateITA: "يرجى تقديم تاريخ صالح",
	time: "يرجى تقديم وقت صالح بين 00:00 و23:59",
	phoneUS: "الرجاء تقديم رقم هاتف صالح",
	phoneUK: "الرجاء تقديم رقم هاتف صالح",
	mobileUK: "يرجى تقديم رقم هاتف محمول صالح",
	strippedminlength: $.validator.format( "يرجى تقديم {0} حرفًا على الأقل" ),
	email2: "يرجى تقديم عنوان بريد إلكتروني صالح",
	url2: "يرجى إدخال عنوان بريد إلكتروني صحيح",
	creditcardtypes: "يرجى تقديم رقم بطاقة ائتمان صالح",
	currency: "يرجى تقديم عملة صالحة",
	ipv4: "يرجى تقديم عنوان IP v4 صالح",
	ipv6: "يرجى تقديم عنوان IP v6 صالح",
	require_from_group: $.validator.format( "يرجى تقديم ما لا يقل عن {0} من هذه الحقول" ),
	nifES: "يرجى تقديم رقم TIN صالح",
	nieES: "يرجى تقديم رقم NIE صالح",
	cifES: "يرجى تقديم رقم CIF صالح",
	postalCodeCA: "يرجى تقديم رمز بريدي صالح",
	pattern: "التنسيق غير صالح"
} );
return $;
}));