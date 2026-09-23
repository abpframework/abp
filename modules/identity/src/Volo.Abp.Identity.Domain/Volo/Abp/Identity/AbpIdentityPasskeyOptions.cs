using System;
using System.Collections.Generic;

namespace Volo.Abp.Identity;

public class AbpIdentityPasskeyOptions
{
    /// <summary>
    /// Maps authenticator AAGUIDs to display names.
    /// The default entries are sourced from the community-maintained passkey-authenticator-aaguids list.
    /// </summary>
    public Dictionary<Guid, string> KnownAuthenticators { get; }

    public AbpIdentityPasskeyOptions()
    {
        KnownAuthenticators = new Dictionary<Guid, string>
        {
            [new Guid("a11a5faa-9f32-4b8c-8c5d-2f7d13e8c942")] = "AliasVault",
            [new Guid("ea9b8d66-4d01-1d21-3ce4-b6b48cb575d4")] = "Google Password Manager",
            [new Guid("adce0002-35bc-c60a-648b-0b25f1f05503")] = "Chrome on Mac",
            [new Guid("08987058-cadc-4b81-b6e1-30de50dcbe96")] = "Windows Hello",
            [new Guid("9ddd1817-af5a-4672-a2b9-3e3dd95000a9")] = "Windows Hello",
            [new Guid("6028b017-b1d4-4c02-b4b3-afcdafc96bb2")] = "Windows Hello",
            [new Guid("dd4ec289-e01d-41c9-bb89-70fa845d4bf2")] = "iCloud Keychain (Managed)",
            [new Guid("531126d6-e717-415c-9320-3d9aa6981239")] = "Dashlane",
            [new Guid("bada5566-a7aa-401f-bd96-45619a55120d")] = "1Password",
            [new Guid("b84e4048-15dc-4dd0-8640-f4f60813c8af")] = "NordPass",
            [new Guid("0ea242b4-43c4-4a1b-8b17-dd6d0b6baec6")] = "Keeper",
            [new Guid("891494da-2c90-4d31-a9cd-4eab0aed1309")] = "Sésame",
            [new Guid("f3809540-7f14-49c1-a8b3-8f813b225541")] = "Enpass",
            [new Guid("b5397666-4885-aa6b-cebf-e52262a439a2")] = "Chromium Browser",
            [new Guid("771b48fd-d3d4-4f74-9232-fc157ab0507a")] = "Edge on Mac",
            [new Guid("39a5647e-1853-446c-a1f6-a79bae9f5bc7")] = "IDmelon",
            [new Guid("d548826e-79b4-db40-a3d8-11116f7e8349")] = "Bitwarden",
            [new Guid("fbfc3007-154e-4ecc-8c0b-6e020557d7bd")] = "Apple Passwords",
            [new Guid("53414d53-554e-4700-0000-000000000000")] = "Samsung Pass",
            [new Guid("66a0ccb3-bd6a-191f-ee06-e375c50b9846")] = "Thales Bio iOS SDK",
            [new Guid("8836336a-f590-0921-301d-46427531eee6")] = "Thales Bio Android SDK",
            [new Guid("cd69adb5-3c7a-deb9-3177-6800ea6cb72a")] = "Thales PIN Android SDK",
            [new Guid("17290f1e-c212-34d0-1423-365d729f09d9")] = "Thales PIN iOS SDK",
            [new Guid("50726f74-6f6e-5061-7373-50726f746f6e")] = "Proton Pass",
            [new Guid("fdb141b2-5d84-443e-8a35-4698c205a502")] = "KeePassXC",
            [new Guid("eaecdef2-1c31-5634-8639-f1cbd9c00a08")] = "KeePassDX",
            [new Guid("9addb28c-b46f-4402-808f-019651441ff3")] = "KeePassPasskey",
            [new Guid("cc45f64e-52a2-451b-831a-4edd8022a202")] = "ToothPic Passkey Provider",
            [new Guid("bfc748bb-3429-4faa-b9f9-7cfa9f3b76d0")] = "iPasswords",
            [new Guid("b35a26b2-8f6e-4697-ab1d-d44db4da28c6")] = "Zoho Vault",
            [new Guid("b78a0a55-6ef8-d246-a042-ba0f6d55050c")] = "LastPass",
            [new Guid("de503f9c-21a4-4f76-b4b7-558eb55c6f89")] = "Devolutions",
            [new Guid("22248c4c-7a12-46e2-9a41-44291b373a4d")] = "LogMeOnce",
            [new Guid("a10c6dd9-465e-4226-8198-c7c44b91c555")] = "Kaspersky Password Manager",
            [new Guid("d350af52-0351-4ba2-acd3-dfeeadc3f764")] = "pwSafe",
            [new Guid("d3452668-01fd-4c12-926c-83a4204853aa")] = "Microsoft Password Manager",
            [new Guid("6d212b28-a2c1-4638-b375-5932070f62e9")] = "initial",
            [new Guid("d49b2120-b865-4191-8cea-be84a52b0485")] = "Heimlane Vault",
            [new Guid("e8b7f4a2-c3d5-e6f7-890a-b1c2d3e4f567")] = "Sherlocked",
            [new Guid("d9be9d39-e6a6-4c28-a581-32b044d986e4")] = "Sticky Password Manager",
            [new Guid("70617373-7761-6c6c-6669-646f32303236")] = "Passwall",
            [new Guid("c9cadfc9-89a9-489e-a25a-c7e86a4d5f15")] = "Burp Suite Navigation Recorder",
            [new Guid("fa37f553-f9b6-4adb-ac53-8bbb57ebdf0d")] = "Norton Password Manager",
            [new Guid("a4a2d88e-9796-4356-9164-e2a5a8bd019c")] = "Avast Password Manager",
            [new Guid("e7db2bd3-f2fe-4d71-ad78-7e7aa166cfd1")] = "Avira Password Manager",
            [new Guid("6bb49926-160a-4306-a100-4eb39ba6ac45")] = "AVG Password Manager",
            [new Guid("da583154-ce16-4cdf-9fe6-1dba788c0998")] = "Hey Be Safe",
            [new Guid("d2717a32-9851-48a8-9961-b264c97a411a")] = "Fenko Vault",
            [new Guid("65c97700-f5ef-4d5c-8a42-f30e45ac94b7")] = "Royal Vault",
            [new Guid("5ca471bb-a56d-46ad-a496-67e70e9ed9fb")] = "Parcel",
            [new Guid("45e3057e-b2f9-48ed-912f-9b901e153b16")] = "Uniqkey",
            [new Guid("87f5ec51-f721-4feb-9fe4-be18c4971894")] = "PassCard",
            [new Guid("477b05cd-7f78-4fe7-b629-27247f296138")] = "WALLIX Vault",
            [new Guid("53e7a7a5-e75f-4d3d-9483-12fc779cdf23")] = "Password Depot",
            [new Guid("cb6f6666-38ea-4873-9161-ff456a82d316")] = "iPass Secure Auth",
            [new Guid("9c1f2b6e-4d3a-4f7c-8b21-5e6a7d8c9f01")] = "Idira",
            [new Guid("9f8a3b2c-1d4e-4f6a-8b0c-2e1d3c4b5a69")] = "U2 Secured",
            [new Guid("69840def-9dcf-4632-bf87-6ac2e451b4f5")] = "Keyholm"
        };
    }
}
