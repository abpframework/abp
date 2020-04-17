## Průvodce pro přispěvatele

ABP je [open source](https://github.com/abpframework) a komunitně řízený projekt. Tento průvodce má za cíl pomoci každému kdo chce do projektu nějak přispět.

### Příspěvek kódu

Vždy můžete zaslat pull request do Github repositáře.

- Naklonujte [ABP repozitář](https://github.com/abpframework/abp/) z Githubu.
- Učiňte potřebné změny.
- Zašlete pull request.

Než budete dělat nějaké změny, diskutujte o nich prosím na [Github problémy](https://github.com/abpframework/abp/issues). Díky tomu nebude žádný jiný vývojář pracovat na stejném problému a Váš PR má lepší šanci na to být přijat.

#### Opravy chyb a vylepšení

Pokud chcete opravit známou chybu nebo pracovat na plánovaném vylepšení podívejte se na [seznam problémů](https://github.com/abpframework/abp/issues) na Githubu.

#### Požadavky na funkce

Pokud máte nápad na funkci pro framework nebo modul [vytvořte problém](https://github.com/abpframework/abp/issues/new) na Githubu nebo se připojte ke stávající diskuzi. V případě přijetí komunitou ho pak můžete implementovat.

### Překlad dokumentů

Pokud chcete přeložit celou [dokumentaci](https://abp.io/documents/) (včetně této stránky) do Vašeho rodného jazyka, následujte tyto kroky:

* Naklonujte [ABP repozitář](https://github.com/abpframework/abp/) z Githubu.
* K přidání nového jazyka vytvořte novou složku v [docs](https://github.com/abpframework/abp/tree/master/docs). Název složky musí být "en", "es", "fr", "tr" atd. v závislosti na jazyku (navštivte [všechny jazykové kódy](https://msdn.microsoft.com/en-us/library/hh441729.aspx)).
* Pro referenci použijte ["en" složku](https://github.com/abpframework/abp/tree/master/docs/en) a její názvy souborů a strom složek. Při překladu této dokumentace zachovejte prosím tyto názvy stejné.
* Zašlete pull request (PR) po překladu jakéhokoliv dokumentu klidně i po jednom. Nečekejte až budete mít překlad všech dokumentů.

Existuje několik základních dokumentů, které je třeba přeložit než bude jazyk uveřejněn na [stránkách ABP dokumentace](https://docs.abp.io)

* Začínáme dokumenty
* Tutoriály
* CLI

Nový jazyk je publikován jakmile jsou minimálně tyto překlady dokončeny.

### Lokalizace zdrojů

ABP framework má flexibilní [lokalizační systém](../Localization.md). Můžete tak vytvořit lokalizované uživatelské prostředí pro svou vlastní aplikaci.

K tomu mají framework a vestavěné moduly již lokalizované texty. Například [lokalizační texty pro Volo.Abp.UI balík](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json). Můžete vytvořit nový soubor ve [stejné složce](https://github.com/abpframework/abp/tree/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi) k přidání překladu.

* Naklonujte [ABP repozitář](https://github.com/abpframework/abp/) z Githubu.
* Vytvořte nový soubor pro cílový jazyk pro lokalizační text v (json) souboru (u souboru en.json).
* Zkopírujte veškerý text ze souboru en.json.
* Přeložte texty.
* Zašlete pull request na Githubu.

ABP je modulářní framework, proto je zde mnoho zdrojů lokalizačních textů, jeden pro každý modul. K najití všech .json souborů, vyhledejte po naklonování repozitáře soubory "en.json". Můžete se taky podívat na [tento seznam](Localization-Text-Files.md) souborů lokalizačních textů.

### Příspevky do blogu a návody

Pokud se rozhodnete pro ABP vytvořit nějaké návody nebo příspěvky do blogu, dejte nám vědět (prostřednictvím [Github problémy](https://github.com/abpframework/abp/issues)), ať můžeme přidat odkaz na Váš návod/příspěvek v oficiální dokumentaci a oznámit na našem [Twitter účtu](https://twitter.com/abpframework).

### Zpráva o chybě

Pokud najdete chybu, [vytvořte prosím problém v Github repozitáři](https://github.com/abpframework/abp/issues/new).
