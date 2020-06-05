# Domain Driven Design

## Co je DDD?

ABP framework poskytuje **infrastrukturu**, která zjednodušuje implementaci vývoje založeného na **DDD**. DDD je [definován ve Wikipedii](https://en.wikipedia.org/wiki/Domain-driven_design) takto:

> **Domain-driven design** (**DDD**) je přístup k vývoji softwaru pro komplexní potřeby propojením implementace s vyvíjejícím se modelem. Předpoklad DDD je následující:
>
> - Primární zaměření projektu je na jádře domény a doménové logice;
> - Zakládání komplexních návrhů na modelu domény;
> - Iniciování tvůrčí spolupráce mezi technickými a doménovými odborníky s cílem iterativně zdokonalit koncepční model, který řeší konkrétní problémy v doméně.

### Vrstvy

ABP dodržuje principy a vzorce DDD pro dosažení vrstveného aplikačního modelu, který se skládá ze čtyř základních vrstev:

- **Prezentační vrstva**: Poskytuje uživateli rozhraní. Používá *Aplikační vrstvu* k dosažení uživatelských interakcí.
- **Aplikační vrstva**: Prostředník mezi prezentační a doménovou vrstvou. Instrumentuje business objekty k provádění specifických úloh aplikace. Implementuje případy použití jako logiku aplikace.
- **Doménová vrstva**: Zahrnuje business objekty a jejich business pravidla. Je jádrem aplikace.
- **Vrstva infrastruktury**: Poskytuje obecné technické možnosti, které podporují vyšší vrstvy většinou pomocí knihoven třetích stran.

## Obsah

* **Doménová vrstva**
  * [Entity & agregované kořeny](Entities.md)
  * Hodnotové objekty
  * [Repozitáře](Repositories.md)
  * Doménové služby
  * Specifikace
* **Aplikační vrstva**
  * [Aplikační služby](Application-Services.md)
  * [Objekty přenosu dat (DTOs)](Data-Transfer-Objects.md)
  * Jednotka práce