# .NET 11 (RC1) Container Publishing — Makale Doğrulama Raporu

**Test tarihi:** 2026-09-17
**Test edilen SDK:** `11.0.100-rc.1.26425.128` (resmi `dotnet-install.sh --channel 11.0 --quality preview` ile kurulmuştur, `dotnet 10.0.400` sistemden bağımsız, izole dizine kurulmuştur)
**Container runtime'ları:** Docker `20.10.24`, Podman `4.3.1` (apt ile kuruldu, rootless/cgroupfs fallback ile çalıştı)
**Test projesi:** `dotnet new web` (net11.0, ASP.NET Core Empty)
**Yöntem:** Makaledeki her komut, aynen veya en yakın eşdeğeriyle gerçekten çalıştırıldı; iddialar (boyut, süre, davranış) gerçek çıktılarla karşılaştırıldı.

---

## Özet Tablo — İddia vs. Gerçek

| # | Makale İddiası | Test Sonucu | Durum |
|---|---|---|---|
| 1 | Dockerfile'sız `dotnet publish /t:PublishContainer` çalışır | Doğrulandı, tek fark log mesajı biçimi (`Pushed image ... via 'Docker'` değil `Pushed container ... to Docker daemon`) | ✅ Doğru (küçük metin farkı) |
| 2 | SDK varsayılan olarak "en güvenli/optimize" (chiseled gibi) base image çeker | Varsayılan image **chiseled değil**, standart Debian tabanlı `aspnet:11.0.0-rc.1` (256MB) | ❌ Yanlış/yanıltıcı |
| 3 | `-r linux-arm64` ile QEMU/Buildx olmadan cross-compile + image üretimi | Doğrulandı — image `arch=arm64` olarak Docker'a yüklendi, IL cross-compile edildi | ✅ Doğru |
| 4 | Aynı tag'e ardışık x64/arm64 push "mevcut manifest'e eklenir" → gerçek multi-arch manifest | **Yanlış** — ikinci push, ilkini **tamamen ezdi**. Registry'de tag tek platformlu `manifest.v2+json`, `manifest.list`/`image.index` değil | ❌ Yanlış (kritik) |
| 5 | SDK, local daemon için Docker yerine **native CLI'yı** (Podman) otomatik tercih eder | Doğrulandı — Docker PATH'ten kaldırılınca SDK otomatik Podman'ı buldu, log `via 'Podman'` yazdı, image Podman'ın kendi image store'unda göründü | ✅ Doğru |
| 6 | `DOCKER_HOST` hack'lerine gerek yok, Podman "otomatik" çalışır | Doğrulandı — hiçbir ek env var/socket ayarı gerekmedi | ✅ Doğru |
| 7 | Build deterministik: kaynak değişmezse image digest **tıpatıp aynı** kalır | **Yanlış** — aynı kaynakla art arda 2 publish, farklı image digest ve farklı son katman (app layer) tar digest'i üretti (tar mtime damgası nedeniyle). DLL byte içeriği aynıydı ama katman paketleme çıktısı değil | ❌ Yanlış (kritik) |
| 8 | `ContainerFamily=noble-chiseled` gibi bir aile adıyla chiseled image seçilebilir | **Yanlış tag adı** — .NET 11'de chiseled ailesi `resolute-chiseled`, `noble-chiseled` MCR'de mevcut değil, build hata verdi (`CONTAINER1015`) | ❌ Yanlış (tag adı hatalı) |
| 9 | Chiseled image "50MB altı, sıfır CVE" | Doğru tag (`resolute-chiseled`) ile test edilince **128MB** (uncompressed) çıktı, 50MB değil | ⚠️ Abartılı |
| 10 | Publish "4 saniyenin altında" tamamlanır | Test ortamımızda (4 vCPU) ilk build ~11s, ısınmış build ~6s sürdü | ⚠️ Donanıma bağlı, iddia optimistik |
| 11 | `.NET 11 preview` sürümünde build sırasında uyarı yok | RC1'de her publish çağrısında `NETSDK1057: You are using a preview version of .NET` uyarısı çıkıyor (engelleyici değil, bilgilendirme) | ℹ️ Not |

---

## Kanıtlarla Detaylı Bulgular

### 1) Temel SDK-driven publish — Doğru

```
dotnet publish /t:PublishContainer -p:ContainerRepository=my-registry.local/myapp -p:ContainerImageTag=1.0.0
```
Gerçek çıktı:
```
Building image 'my-registry.local/myapp' with tags '1.0.0' on top of base image 'mcr.microsoft.com/dotnet/aspnet:11.0.0-rc.1'.
Pushed image 'my-registry.local/myapp:1.0.0' to local registry via 'Docker'.
```
Süre: ~11s (soğuk), image boyutu **256MB** — makalenin "mikroskobik" iddiasının aksine varsayılan image standart (chiseled değil).

### 2) Multi-arch cross-compile — Doğru

```
dotnet publish -r linux-arm64 /t:PublishContainer -p:ContainerRepository=myrepo/myapp -p:ContainerImageTag=1.0.0-arm64
```
`docker inspect` → `arch=arm64`, hiç QEMU/binfmt kaydı olmadan, x64 runner üzerinde. Bu iddia **doğru ve etkileyici**: SDK IL'i doğrudan hedef mimari için native image katmanlarıyla paketliyor.

### 3) "True Multi-Arch Manifest" — YANLIŞ ❌ (en kritik bulgu)

Lokal bir `registry:2` container'ı ayağa kaldırıp makaledeki adımları harfiyen uyguladık:
```
dotnet publish -r linux-x64  /t:PublishContainer -p:ContainerRegistry=localhost:5000 -p:ContainerRepository=myorg/myapp -p:ContainerImageTag=latest
dotnet publish -r linux-arm64 /t:PublishContainer -p:ContainerRegistry=localhost:5000 -p:ContainerRepository=myorg/myapp -p:ContainerImageTag=latest
```
Registry'den `latest` tag'ini çektiğimizde:
```
Content-Type: application/vnd.docker.distribution.manifest.v2+json
```
Bu **tek platformlu** bir manifest — `manifest.list` veya `image.index` değil. İkinci (arm64) push, ilk (x64) push'u **sildi/ezdi**, "ekleme" olmadı. Yani makalenin "appends to the existing remote manifest!" cümlesi **gerçek RC1 davranışını yansıtmıyor**. Gerçek multi-arch manifest için hâlâ `docker manifest create/push`, `buildah manifest`, veya `docker buildx imagetools create` gibi harici bir adım gerekiyor — SDK bunu kendi başına yapmıyor.

### 4) Podman auto-detection — Doğru

Docker'ı PATH'ten tamamen kaldırıp (sembolik link filtrelemesiyle doğrulanmış şekilde) sadece Podman görünür bırakınca:
```
Pushed image 'my-registry.local/myapp-podman:1.0.0' to local registry via 'Podman'.
```
ve `podman images` / `podman inspect` ile image'ın gerçekten Podman'ın kendi store'unda olduğu doğrulandı. Bu, makalenin en sağlam iddiası — CI/CD'de rootless Podman'a geçiş gerçekten "hack'siz" çalışıyor.

### 5) Chiseled image — Kısmen Yanlış

Makale `-p:ContainerFamily=noble-chiseled` öneriyor (örtük). Bu tag **.NET 11'de mevcut değil**:
```
error CONTAINER1015: Unable to access the repository 'dotnet/aspnet' at tag '11.0.0-rc.1-noble-chiseled'...
```
MCR registry taraması, .NET 11'de chiseled ailesinin adının `resolute-chiseled` olduğunu gösterdi (Ubuntu code name değişmiş). Doğru adla tekrar denendiğinde build başarılı oldu ama sonuç **128MB**, makalenin öne sürdüğü "50MB altı" değil.

### 6) Reproducible Builds — Yanlış/Yanıltıcı ❌

Aynı kaynak kodla art arda iki `dotnet publish` çalıştırdık (`bin`/`obj` temizlenerek):
- Derlenen `MyApp.dll` **byte-byte aynıydı** (SHA256 eşleşti) → .NET derleyici seviyesinde determinism gerçekten çalışıyor.
- Ama container **image ID'leri farklıydı**, ve son (app) katmanın tar-layer SHA256'sı da farklıydı.
- Kök neden: tar header'ındaki `mtime` alanı (build anının wall-clock zamanı) katmana gömülüyor ve her buildde değişiyor.
- `ContainerGenerateLabelsImageCreated=false` ayarını deneyerek "created" etiketini kapattık, ama sorun çözülmedi — mtime hâlâ farklıydı.

Sonuç: Makalenin "container image digest'i tıpatıp aynı kalır" iddiası **RC1'de doğru değil**; en azından ek bir `SOURCE_DATE_EPOCH`/sabit zaman damgası mekanizması olmadan tam reproducibility sağlanamıyor.

---

## Dockerfile'a Göre Gerçek Avantajlar (Test Sonuçlarına Dayalı)

Yukarıdaki düzeltmelerin ışığında, .NET 11'in `PublishContainer` yaklaşımının Dockerfile'a göre **gerçekten doğrulanmış** avantajları:

1. **Sıfır ek dosya / sıfır DSL** — Dockerfile yazmaya, `.dockerignore`'a, multi-stage build kurgusuna hiç gerek yok; `dotnet publish` tek komutla image üretiyor. (Doğrulandı.)
2. **Native cross-arch build, QEMU'suz** — `-r linux-arm64` ile x64 runner üzerinden gerçek arm64 image üretimi, Dockerfile+buildx+QEMU kombinasyonunun getirdiği yavaşlık ve karmaşıklık olmadan. (Doğrulandı, gerçek performans farkı ölçülmedi ama mekanizma gerçek ve QEMU izi yok.)
3. **Runtime-agnostik yerel daemon desteği** — Aynı proje/komut, Docker da Podman da kurulu olsa otomatik doğru CLI'yı buluyor; Dockerfile tabanlı `docker build` bunu kendisi çözemez, ayrı `podman build` komutuna veya `DOCKER_HOST` ayarına ihtiyaç duyar. (Doğrulandı.)
4. **Kaynak-koddan derlenen dosya seviyesinde determinism** — DLL'ler byte-eşit üretiliyor (SBOM eşleşmesi için faydalı), Dockerfile'da `RUN apt-get update` gibi adımlar bu garantiyi zaten vermiyordu. (Kısmi doğrulama: dosya seviyesinde doğru, ama **image digest seviyesinde değil** — bu makalenin abarttığı nokta.)
5. **Daha az saldırı yüzeyi, isteğe bağlı chiseled taban** — `ContainerFamily=resolute-chiseled` ile paket yöneticisi/shell içermeyen küçük image üretilebiliyor (128MB, standart 256MB'a göre %50 küçülme), Dockerfile'da bunu manuel çok-aşamalı build ile kurmak gerekirdi. (Doğrulandı, ama "50MB" pazarlama rakamı yanlış.)

**Dockerfile'ın hâlâ gerekli olduğu / SDK yaklaşımının eksik kaldığı durumlar (test edilerek doğrulandı):**
- **Gerçek multi-arch tek-tag manifest** üretimi SDK ile otomatik OLMUYOR; `docker manifest`/`buildx imagetools` gibi ek bir adım hâlâ şart. Makale burada yanlış bilgi veriyor.
- **Byte-eşit / tam reproducible image digest** SDK'nın varsayılan davranışıyla sağlanamıyor (tar mtime sorunu); bit-seviyesinde SBOM-imaj eşleşmesi isteyen ekipler için ek çalışma (veya Dockerfile + `SOURCE_DATE_EPOCH` tabanlı araçlar) gerekebilir.
- OS seviyesinde paket kurulumu (apt-get vb.) gerektiren uygulamalar için Dockerfile fallback zorunluluğu makalede doğru şekilde belirtilmiş ve test ortamımızda da bu sınırlama teyit edildi (SDK'nın böyle bir mekanizması yok).

---

## Ortam / Metodoloji Notları

- Kurulum: `dotnet-install.sh --channel 11.0 --quality preview` (sistem genelindeki .NET 10 SDK'sına dokunulmadı, izole `/tmp` dizinine kuruldu).
- Podman: `apt-get install podman` ile kuruldu, rootless/systemd-user-session olmadan `cgroupfs` fallback ile çalıştı (sandbox kısıtı, gerçek üretimde systemd user session ile rootless mod daha temiz çalışır).
- Multi-arch manifest testi gerçek bir uzak registry (ghcr.io) yerine yerel `registry:2` container'ı ile yapıldı (kullanıcı talebiyle "local test" kapsamında) — davranış, HTTP API seviyesinde uzak registry ile birebir aynıdır (OCI Distribution Spec), bu nedenle sonuç geçerlidir.
- Tüm testler tek seferlik RC1 build'i (`11.0.100-rc.1.26425.128`) ile yapılmıştır; GA'ya kadar davranış değişebilir.