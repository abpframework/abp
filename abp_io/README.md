## ABP Platform Websites Localization

This is the localization project of [abp.io platform](https://abp.io).
All *.abp.io websites are built on top of ABP Framework, and it uses ABP Framework's localization system.
You can correct a wrong localization text, or you can translate it into your own language.
By doing so, [abp.io](https://abp.io) websites will be translated into a new language and it will help to expand the ABP Community.



## How to Translate abp.io Into Your Language:

1. Install [ABP CLI](https://abp.io/docs/latest/cli) command line tool.

2. Run the following command to generate the localization file. 
   For example, for translating from English to French `fr`: 
   
   ```bash
   abp translate -c fr
   ```
3. After you fill in the empty localization keys, run the following command to apply it.
   ```bash
   abp translate -a
   ```
4. Send your PR to the team; after the review process, we wil merge it.

---



## References:
* [ABP CLI Translate Command](https://abp.io/docs/latest/contribution#using-the-abp-translate-command)
* [Contribution Guide](https://github.com/abpframework/abp/blob/dev/docs/en/Contribution/Index.md)
