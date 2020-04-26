## Katlı Kılavuzu

ABP [açık kaynak kodlu](https://github.com/abpframework) ve topluluk yürürlüğünde ilerleyen bir projedir. Bu kılavuz, herkesin projeye katkıda bulunmak istemesine yardımcı olmayı amaçlamaktadır.

### Kod Katkısı

Github deposuna her zaman pull request gönderebilirsiniz.

- Github dan [ABP reposu](https://github.com/abpframework/abp/) nu klonlayın
- Gerekli değişiklikleri yapın.
- Pull request isteğini gönderin.

Herhangi bir değişiklik yapmadan önce, lütfen [Github issues](https://github.com/abpframework/abp/issues) de tartışın. Bu şekilde, başka hiçbir geliştirici aynı konu üzerinde çalışmaz ve PR isteğinizin kabul edilme şansı artar.

#### Hata Çözümü & Geliştirmeler

Bilinen bir hatayı düzeltmek veya planlanan bir geliştirme üzerinde çalışmak isteyebilirsiniz. Github daki [issue listesi](https://github.com/abpframework/abp/issues) ne bakınız.

#### Özellikle Talepleri

Framework veya modüller için bir özellik fikriniz varsa, Girhub da [issue oluşturunuz](https://github.com/abpframework/abp/issues/new) veya varolan bir tartışmaya katılınız. Sonra topluluk tarafından benimsenirse uygulayabilirsiniz

### Döküman Çevirileri

[Dökümantasyonu](https://abp.io/documents/) kendi ana dilinize çevirmek isteyebilirsiniz. Öyleyse, aşşağıda ki adımları izleyiniz:

* Github dan [ABP reposunu](https://github.com/abpframework/abp/) klonlayınız.
* Yeni dil eklemek için, [docs](https://github.com/abpframework/abp/tree/master/docs) klasöründe yeni klasör oluşturun. Klasörün adı dile göre "en", "es", "fr", "tr" olabilir. Dil kodları için  [tüm dil kodları](https://msdn.microsoft.com/en-us/library/hh441729.aspx) bakabilirsiniz.
* Dosya adları ve klasör yapılandırması için ["en" klasörünü](https://github.com/abpframework/abp/tree/master/docs/en) referans alabilirsiniz. Eğer aynı dökümanı çeviri yapıyorsanız, dosya adını aynı şekilde tutunuz.
* Herhangi bir döküman çevirisi yaptıktan sonra pull request (PR) 'i gönderiniz. Lütfen döküman çevirisi yapınız ve PR lerinizi bir bir gönderiniz. Tüm dökümanları çevirmeyi beklemeyiniz.

Çevirinin [ABP döküman sitesinde](https://docs.abp.io) yayınlanmasından önce bazı çevirilerin olması gerekiyor. 
* Başlangıç dökümanları
* Öğreticiler
* CLI

Yeni dil minumum gereksinimleri tamamlandıktan sonra yanınlanabilinir.

### Kaynak Yerelleştirmeleri

ABP frameworkü esnek bir [yerelleştirme sistemi](../Localization.md)ne sahiptir. Siz kendi uygulamanıza göre yerelleştirme oluşturabilirsiniz.

Buna ek olarak, frameworkün ve önceden oluşturulmuş modüllerin zaten metni yerelleştirdi. Örnek olarak, [Volo.Abp.UI paketinin yerelleştirme metinlerine](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json) bakabilirsiniz. Çeviri yapmak için [aynı klasörde](https://github.com/abpframework/abp/tree/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi) dosya oluşturabilirsiniz.

* Github dan [ABP reposu](https://github.com/abpframework/abp/) nu klonlayın
* Hedef dil için yerelleştirme metni (json) dosyası (en.json dosyasına yakın) oluşturunuz.
* en.json daki tüm metinleri kopyalayınız.
* Metinlerin çevirilerini yapınız.
* Github dan pull request gönderiniz.

ABP bir moduler framework dür. Böylece bir çok yerelleştirme kaynağı her modül için bulunuyor. Bütün .json dosyalarını bulmak için, klonlama işlemini yaptıktan sonra "en.json" dosyalarını aratabilirsiniz. Farklı olarak [bu liste](Localization-Text-Files.md)ye de yerelleştirme dosyaları için bakabilirsiniz.

### Blog Yazıları & Öğreticiler

Eğer öğretici oluşturmak yada abp de blog yazısı oluşturmak isterseniz, lütfen bize ([Github issue](https://github.com/abpframework/abp/issues) oluşturarak) bilgi veriniz. böylece biz de dökümanlarımızda sizin öğretici/yazınız için link ekleyebilir ve bizim [Twitter hesabımız](https://twitter.com/abpframework) da yayınlayabiliriz.

### Hata Bildirimi

Eğer bir hata bulursanız, lütfen [Github reposunda bir issue oluşturunuz](https://github.com/abpframework/abp/issues/new)
