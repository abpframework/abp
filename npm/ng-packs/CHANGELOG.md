# [2.0.0](https://github.com/abpframework/abp/compare/1.1.2...2.0.0) (2020-01-13)


### Bug Fixes

* **core:** fix flattedRoutes manipulation ([2b2e0be](https://github.com/abpframework/abp/commit/2b2e0be51d9fb37188348f390b56df286de7a6bc))
* **core:** fix nullable control in the config state ([756e858](https://github.com/abpframework/abp/commit/756e8588f3dd2d1035dbc22db3773653e28b1542))
* **core:** update flattedRoutes when patchRoute action dispatched ([15a8279](https://github.com/abpframework/abp/commit/15a82794ac5173bd080d3aec46efb43bfc0614ae))
* **permission-management:** fix twice executing problem of visible setter([ae16916](https://github.com/abpframework/abp/commit/ae16916ae2b03dfc25d9c4c5ceeb559dee34bde2))
* **theme-shared:** fix remember me functionality [(#2610)](https://github.com/abpframework/abp/pull/2610) ([189a77f](https://github.com/abpframework/abp/commit/189a77f4c421398b841e67280f6f19c4ef56649d)), closes [(#2602)](https://github.com/abpframework/abp/issues/2602)


### Code Refactoring

* remove deprecated functions, outputs, inputs etc. ([f1d0eba](https://github.com/abpframework/abp/commit/f1d0ebae856406213023af11632ed72849928b3e)), closes [#2476](https://github.com/abpframework/abp/issues/2476)


### Features

* add state actions to related state services ([#2431](https://github.com/abpframework/abp/pull/2431)), closes ([#2430](https://github.com/abpframework/abp/pull/2430))
* make some components replaceable ([#2522](https://github.com/abpframework/abp/pull/2522)), closes ([#2404](https://github.com/abpframework/abp/issues/2404))
* **account:** add autocomplete attiributes for chromium based browsers ([343c847](https://github.com/abpframework/abp/commit/343c847fb70d634218de1b156b3a15557e27acda))
* **account:** add the enableLocalLogin condition to auth-wrapper ([#2537](https://github.com/abpframework/abp/pull/2573)) ([0fb7ba4](https://github.com/abpframework/abp/commit/0fb7ba4614ebb64a00f19a9bef277a0fb9bee764))
* **core:** add init directive ([5ae1071](https://github.com/abpframework/abp/commit/5ae10717d73b1d76711dcd68201ed6b0609c4112))
* **core:** add ExtractFromGeneric type to ABP namespace ([1fe3b35](https://github.com/abpframework/abp/commit/1fe3b35076c5daddf9edc2a8396b62288cf105da))
* **core:** add init directive to emit an output when the element initialized ([31c224c](https://github.com/abpframework/abp/commit/31c224c9f9c015e94877d1eba382d1cd415704c7))
* **core:** fill the parentName property in flattedRoutes for child rotues ([17dd0a2](https://github.com/abpframework/abp/commit/17dd0a25a7dc2fa6268ce68f4f2f5d78f78a1f23))
* **core:** create AddRoute action to add new elements to top navigation ([#2425](https://github.com/abpframework/abp/pull/2425)), closes ([#2186](https://github.com/abpframework/abp/pull/2186))
* **theme-shared:** trigger unload confirmation in the modal component when form is dirty ([f59a294](https://github.com/abpframework/abp/commit/f59a2945b223a19750afd46da7e33deff2583328))
* **theme-shared:** create new toast and confirmation components ([#2606](https://github.com/abpframework/abp/pull/2606)), closes ([#2537](https://github.com/abpframework/abp/issues/2537))
* **theme-shared:** create table, pagination, loading components  ([#2605](https://github.com/abpframework/abp/pull/2605)), related ([#2537](https://github.com/abpframework/abp/issues/2537))


### BREAKING CHANGES

* Deprecated functions, outputs, inputs addressed in issue #2476 are removed.


# [1.1.0](https://github.com/abpframework/abp/compare/1.0.2...1.1.0) (2019-12-06)


### Bug Fixes

* **account:** add header parameter to fetchTokenUsingPasswordFlow method ([3c92489](https://github.com/abpframework/abp/commit/3c924899ca6e5b797bd6536420f75efb61981787))
* **account:** add tenant header to token request ([dce9777](https://github.com/abpframework/abp/commit/dce97779e5c5a343a1a4bb1b2599bb9a0b192d9a))
* **core:** fix add view clear before creating view ([95563de](https://github.com/abpframework/abp/commit/95563de2a414f2b96a52e63e85cf7d256dcccbaa))
* **core:** fix change default insert position in the lazy-load.service ([13a10c2](https://github.com/abpframework/abp/commit/13a10c2cc5dd35ae120492f95de18b182be85eb1))
* **core:** fix abpfor directive filtering bug ([4f4075f](https://github.com/abpframework/abp/commit/4f4075fb2b6f9ec371fbfa85ed61ac1a5af725ef))
* **core:** fix sort pipe key error ([2f53454](https://github.com/abpframework/abp/commit/2f534545bac1d45d8f5e5fd99fee982765db6a4d))
* **core:** fix sort pipe null data problem ([4764750](https://github.com/abpframework/abp/commit/4764750a8c4c38c670d05858e88407b91c5dbe60))
* **core:** fix visibility directive bug ([c1a1bfe](https://github.com/abpframework/abp/commit/c1a1bfe7ee16e3c2e9f71779c4928340c0278a9a))
* **identity:** fix validation display error ([335c74d](https://github.com/abpframework/abp/commit/335c74d80097de5a97913e97230f423768f751ea))
* **module-template:** organize home page [#2265](https://github.com/abpframework/abp/issues/2265) ([1b54da2](https://github.com/abpframework/abp/commit/1b54da232ff86891b54219a58e2ee616a46fbe5f))
* **module-template:** update environments ([b5a13a2](https://github.com/abpframework/abp/commit/b5a13a26580210ab035b344ce30366f1595ae9d3))
* **template:** fix lazyLoadService parameters in app.component ([6f024a9](https://github.com/abpframework/abp/commit/6f024a94df6b882385f8653a17071d4b0eb638c3))
* **tenant-management:** add modalBusy controls ([e75bf30](https://github.com/abpframework/abp/commit/e75bf30aada713697f34feb0c6a49ca25bc3e709)), closes [#2042](https://github.com/abpframework/abp/issues/2042)
* **tenant-management:** add type error control ([f21917a](https://github.com/abpframework/abp/commit/f21917ab2bb5f5dce51dde1a5e9fbbef36bc9f90))
* **theme-basic:** fix layout animations ([66fcb0e](https://github.com/abpframework/abp/commit/66fcb0e0a5bdb6f6ec25171cb0a7e06cb333c9cd))
* **theme-shared:** fix breadcrumb type error ([d7c9a60](https://github.com/abpframework/abp/commit/d7c9a6044c32b7b20232a76c5e98b9d874c79cd7))
* **theme-shared:** fix confirmation yes text localization ([f55862a](https://github.com/abpframework/abp/commit/f55862adca463b9fbd6e3af36738874ed210742e))
* **theme-shared:** fix creating custom error component problem ([2c891d8](https://github.com/abpframework/abp/commit/2c891d84e904f619cc4ac6de834e0dbc666837ee))
* **theme-shared:** fix modal animation and backdrop height ([383c4f6](https://github.com/abpframework/abp/commit/383c4f6a29d2412e9225283b0c2f8d0e8db50936))
* **theme-shared:** fix wrong folder name ([1f8c447](https://github.com/abpframework/abp/commit/1f8c4477cf79d1d891d159580728ce8ea76d6934))
* add finalize operator to set false the modalBusy ([208a5e3](https://github.com/abpframework/abp/commit/208a5e3ab52a828e8a4e37156dd2f927c48b152e)), closes [#2093](https://github.com/abpframework/abp/issues/2093)
* add finalize operator to set false the modalBusy ([3ec86f1](https://github.com/abpframework/abp/commit/3ec86f150e5ef1f9728137eff0aa8210248cb015)), closes [#2093](https://github.com/abpframework/abp/issues/2093)
* fix mistyped config file and selector name ([73fbe7d](https://github.com/abpframework/abp/commit/73fbe7ddf1326f5faa812bae0e79c0726d740d5d))
* move table-sort.directive to theme-shared ([ae31b57](https://github.com/abpframework/abp/commit/ae31b57aa91a45d0b828ae64a7151f7d0b483661)), closes [#2067](https://github.com/abpframework/abp/issues/2067)
* remove console warn ([d2dc069](https://github.com/abpframework/abp/commit/d2dc06982b62f4f7c54848130cdf87df6dab521e))
* update NgxValidate blueprints ([22b1c56](https://github.com/abpframework/abp/commit/22b1c569dd1241a9b74be3c7caf128ce101c4f39))


### Code Refactoring

* **theme-shared:** rename error.component ([d17d6b1](https://github.com/abpframework/abp/commit/d17d6b1c16dca21c682b116d2d3847f10e4856bd))


### Features

* **theme-shared:** add date parser formatter for ng-datepicker ([4769843](https://github.com/abpframework/abp/commit/4769843e305badd5da64efccfb90d1552c576b6f))
* **account:** implement dynamic password rules ([0bde323](https://github.com/abpframework/abp/commit/0bde3233186177662f03e7a2a55236e8d2b2fa69))
* **account:** implement dynamic password validation to register page ([60ed67d](https://github.com/abpframework/abp/commit/60ed67d004acb78041efa9e2a2d20cd83611e803)), closes [#2039](https://github.com/abpframework/abp/issues/2039)
* **account:** implement manage-profile html design ([746c7ce](https://github.com/abpframework/abp/commit/746c7ceda9674f6093a4e833c756eda3ae8c8bdd))
* **account:** implement new login and register html design ([f44773c](https://github.com/abpframework/abp/commit/f44773cca74c22f742f82b64b04d390bec9094c7))
* **core:** add condition check to getGrantedPolicy ([76b7559](https://github.com/abpframework/abp/commit/76b755957598b4a21e1f270538c6137ae1c537ae))
* **core:** add stop-propagation directive test ([d4b5bd9](https://github.com/abpframework/abp/commit/d4b5bd93deadd06d45593ab1f0015f55abedf603))
* **core:** add template ref to permission directive ([eedab03](https://github.com/abpframework/abp/commit/eedab03ad1634097bebf21542071933768b9e5e5))
* **core:** add toLocalISOString function to Date prototype ([9be9401](https://github.com/abpframework/abp/commit/9be940181e2d68bd9bb7af44487b81996f4431ac))
* **core:** add undefined and empty string params control to rest service ([6b78dbf](https://github.com/abpframework/abp/commit/6b78dbff863e92f6c41358fceb25210763571388))
* **core:** support to find requiredPolicy from child route ([a3d751f](https://github.com/abpframework/abp/commit/a3d751f482d4ca4ab9b8a23896b789479376e820))
* **feature-management:** add empty data message ([6e759b8](https://github.com/abpframework/abp/commit/6e759b8829649a4a80a5c82700af9f9baea3763d))
* **identity:** add badges to role list ([a191661](https://github.com/abpframework/abp/commit/a1916615354155a91410da65ff726e762a158887))
* **identity:** implement dynamic password validation to users page ([7d026e5](https://github.com/abpframework/abp/commit/7d026e5e9176511864947ebc8d8c452384e72ee9)), closes [#2039](https://github.com/abpframework/abp/issues/2039)
* **permissin-guard:** add a handling permission condition ([c5fc902](https://github.com/abpframework/abp/commit/c5fc902fb3aa45422d7efb5728d12b0e31b0ff8c))
* **permission-management:** add hideBadges input ([b9d2e5f](https://github.com/abpframework/abp/commit/b9d2e5f6565e95b7b6e73c457d9a6f857277379e))
* **setting-management:** add a pointer class ([80e362a](https://github.com/abpframework/abp/commit/80e362a06d05da9f9ac214e95a46510c713ad5b3))
* **settings-management:** add requiredPolicy to settings page ([04bcc4f](https://github.com/abpframework/abp/commit/04bcc4f4a92a213f737e9104b417fd986d803e69))
* **template:** add insert position parameter in load method of lazyLoadScript ([77e6515](https://github.com/abpframework/abp/commit/77e6515aadd7a9d5fa049237f0d09f6e227aafdf))
* **theme-basic:** add icon to default link template ([0e765bd](https://github.com/abpframework/abp/commit/0e765bd1d73342bb2e7dda17a3e4db00a958b45e))
* **theme-basic:** add two icons ([22051fe](https://github.com/abpframework/abp/commit/22051fe7eb416d1d768bac480031518bde621553))
* **theme-basic:** copy new styles ([6c2df8d](https://github.com/abpframework/abp/commit/6c2df8dfdf1c77a414a7049461ed5006e164c3b3))
* **theme-basic:** implement collapse animation to the user info nav element ([87bad9b](https://github.com/abpframework/abp/commit/87bad9b567e3d10b2ae576c4ab514b3591faeb42))
* **theme-basic:** implement collapseWithMargin animation to the application layout main navigation ([6fa1922](https://github.com/abpframework/abp/commit/6fa19229bbd54a79cf98c06b9cbfe3222c0cd91c))
* **theme-shared:** add collapseLinearWithMargin animation ([91f10ee](https://github.com/abpframework/abp/commit/91f10eec66e8930f144d91bb236776296b624fcc))
* **theme-shared:** add custom component features to error.component ([257cc8f](https://github.com/abpframework/abp/commit/257cc8f4597cef6d0fd530e7a15015a088585ade)), closes [#2097](https://github.com/abpframework/abp/issues/2097)
* **theme-shared:** add destroy subject to custom error component ([707c37c](https://github.com/abpframework/abp/commit/707c37c04d602d743289724198a4ffbffde8fc29)), closes [#2097](https://github.com/abpframework/abp/issues/2097)
* **theme-shared:** add expand classes and implement to layout ([d6c049d](https://github.com/abpframework/abp/commit/d6c049d3bfe5945e9d81e3077124a6f993c9bba6))
* **theme-shared:** add hideCloseIcon property to http error config token ([8365934](https://github.com/abpframework/abp/commit/8365934032c144927ae9ac636dbf1a89db94113c))
* **theme-shared:** add http error config token ([5809d1b](https://github.com/abpframework/abp/commit/5809d1ba01438d3d5bc26c7a67a871c5459d2069))
* **theme-shared:** add id input to button.component ([c7c1efe](https://github.com/abpframework/abp/commit/c7c1efe378cd345a40f6af894c58c648fc7618e8))
* **theme-shared:** add overflow-hidden class ([e0902a1](https://github.com/abpframework/abp/commit/e0902a1a20227e4e8e695303357b29b42b9fd8c8))
* **theme-shared:** add table scroll styles ([fe2d731](https://github.com/abpframework/abp/commit/fe2d73102dd00c73dff4cf21559209fa6297a414))
* add abp-collapsed-height and remove abp-collapsed class ([c0fef92](https://github.com/abpframework/abp/commit/c0fef926621a81962c2670dc1337d8a0c3d1a93f))
* add collapseWithMargin animation ([6f94e73](https://github.com/abpframework/abp/commit/6f94e736e3bcaf70b78fbfcf17e0426c63c80f49))
* add jest.config files ([c116a2d](https://github.com/abpframework/abp/commit/c116a2d21396fc696adfd0a393e0420f62333504))
* implement new home page ([01c8bf9](https://github.com/abpframework/abp/commit/01c8bf9f592e834516204a09ae26e661fde46553))
* update ui design ([7ebb7a0](https://github.com/abpframework/abp/commit/7ebb7a0097e8cc1f84c41078a802c596135e3dcf)), closes [#2024](https://github.com/abpframework/abp/issues/2024)


### BREAKING CHANGES

* **theme-shared:** renamed error.component to http-error-wrapper.component



## [1.0.2](https://github.com/abpframework/abp/compare/1.0.0...1.0.2) (2019-10-22)


### Bug Fixes

* fix manage profile animations ([d41e2e4](https://github.com/abpframework/abp/commit/d41e2e483f4a201ba90709a5a898fe3b8535682e))



# [1.0.0](https://github.com/abpframework/abp/compare/0.22.0...1.0.0) (2019-10-21)


### Bug Fixes

* fix width of table columns ([e182861](https://github.com/abpframework/abp/commit/e1828614702d646feab3e4d73696d894d63bbc55))


### Features

* **setting-management:** create state ([8f358bb](https://github.com/abpframework/abp/commit/8f358bbe0179feaacb80bb9dde79682b69b7eb2f))
* **theme-shared:** add new inputs to button component ([d6efc85](https://github.com/abpframework/abp/commit/d6efc85b8ed88bd7b99025662b5220567d9551cc))
* upgrade font awesome ([e4957fe](https://github.com/abpframework/abp/commit/e4957fe45548b7910996aab2dc6b0f9f425e7c04))



# [0.22.0](https://github.com/abpframework/abp/compare/0.21.0...0.22.0) (2019-10-14)


### Bug Fixes

* **core:** change pipe inputs ([4fa105b](https://github.com/abpframework/abp/commit/4fa105bb77b123ec2861b8f9033bb6f986b960e4))
* **core:** fix localizationInitialize function state selector error ([08d013e](https://github.com/abpframework/abp/commit/08d013ebc419366a272fc2d8d4e5a4c55ed22107))
* **core:** fix state selecting errors ([8967bb4](https://github.com/abpframework/abp/commit/8967bb431001d2cad8abf0200103a9809d6ab815))
* **tenant-management-config:** fix exports ([cecdbfc](https://github.com/abpframework/abp/commit/cecdbfc44ae9186db99cf38ae24d01b15ae4af46))
* **theme-shared:** change color-white css class ([1abe70f](https://github.com/abpframework/abp/commit/1abe70f7f1edef8fb198047ce980ba1cfa360b36))
* **theme-shared:** fix modal styles and animations ([093e62e](https://github.com/abpframework/abp/commit/093e62eb64c5446d13fd78ec68dd4e9d4ebfe684))
* **theme-shared:** remove unnecessary brackets ([d330342](https://github.com/abpframework/abp/commit/d3303420e0485ccfbff02840101b9f686c7cb328))
* **theme-shared:** fix timing bug ([8ddf13e](https://github.com/abpframework/abp/commit/8ddf13ef6546957d7c0679a4df93c26c5076c1b9))
* fix some bugs ([efb6c45](https://github.com/abpframework/abp/commit/efb6c456f6453f715d65876722ad636bb024d12e))


### Code Refactoring

* **core:** convert localization pipe to pure ([bbfc2dc](https://github.com/abpframework/abp/commit/bbfc2dc688ea91662dd54adcdb621c26686dea15))


### Features

* **module-template:** add settings component ([d7713ee](https://github.com/abpframework/abp/commit/d7713ee30836cf667ae96fd8bd8a584f377cab82))
* **core:** add default option to localization ([a92b48d](https://github.com/abpframework/abp/commit/a92b48d6b3f635f0f80fc1d6cc81b30f039b4e40))
* **core:** add sort method to sort.pipe ([6b7f2ae](https://github.com/abpframework/abp/commit/6b7f2ae2ca47001c7f8bd977ea9827c9b273a6ea))
* **core:** add table-sort.directive ([35420bc](https://github.com/abpframework/abp/commit/35420bc4d48d5b2109f5f2c943613cd550fb8255))
* **core:** lazy load service supports url array ([220d411](https://github.com/abpframework/abp/commit/220d41149d4824c3341ee5e946a898f159e45a63))
* **module-template:** add identity, tenant management, setting-management modules ([5c1bcbb](https://github.com/abpframework/abp/commit/5c1bcbb256c0b1b8bb948d256ee5e4c8162d12c3)), closes [#1652](https://github.com/abpframework/abp/issues/1652)
* **templates:** add setting management module ([5c29d1f](https://github.com/abpframework/abp/commit/5c29d1f69f9f94f5ecd473a40b0a049ecc51c4eb)), closes [#1652](https://github.com/abpframework/abp/issues/1652)
* create dev-app ([2c2cd7c](https://github.com/abpframework/abp/commit/2c2cd7c1aa0aee2c63a68dbdad9810a730810810))
* create libraries in the module-template [#1652](https://github.com/abpframework/abp/issues/1652) ([7856b22](https://github.com/abpframework/abp/commit/7856b223533aaedbb9b068ab8116daffa985b70c))
* create module template main app [#1652](https://github.com/abpframework/abp/issues/1652) ([a99496f](https://github.com/abpframework/abp/commit/a99496f2efd229b9250334f7eb9c0371b276fa33))
* seperate 3th party css files from initial bundle ([ac71b2f](https://github.com/abpframework/abp/commit/ac71b2fc6f7ae6e6b7d4e632104af330ab8de484))


# [0.21.0](https://github.com/abpframework/abp/compare/0.20.1...0.21.0) (2019-09-23)



## [0.20.1](https://github.com/abpframework/abp/compare/0.20.0...0.20.1) (2019-09-23)


### Bug Fixes

* **account:** comment to remember me logic ([00b9d01](https://github.com/abpframework/abp/commit/00b9d015cb63c25df85705802a53434fabb1d26c))
* **account:** rename appName ([d5035c4](https://github.com/abpframework/abp/commit/d5035c4400cb1f99d32b79b42d05ffa0d60f26fd))
* **core:** add ng zone to route navigating ([19a1abb](https://github.com/abpframework/abp/commit/19a1abba0cc1d42bdaeb8b654737379223ee61bf))
* **core:** auth guard redirect ([7c14c73](https://github.com/abpframework/abp/commit/7c14c73a392c591b382be34128d9674616284b6f))
* **core:** build error ([0f1424a](https://github.com/abpframework/abp/commit/0f1424a0749debcbd95ccc351fcea17dcb294bdd))
* **core:** change getGrantedPolicy ([148c16b](https://github.com/abpframework/abp/commit/148c16b80b2da07447e089626a75add60ac210ca))
* **core:** localization pipe sync value problem ([efa0a4e](https://github.com/abpframework/abp/commit/efa0a4e1bd5184afc9bebd670aaa8447ca841ce5))
* **feature-management:** npm version ([d4bd43b](https://github.com/abpframework/abp/commit/d4bd43b0f9a9387daeed26853d358f87248ae8f8))
* **feature-management:** remove deleteDestPath from ng-package.json ([e35b801](https://github.com/abpframework/abp/commit/e35b8010ed38a5c72794dc3decffb9d252540bbb))
* **identity:** change actions column minimum width ([8086c1c](https://github.com/abpframework/abp/commit/8086c1c97cd18335c71eb3ae9bf217f8c32fca01))
* **identity:** change template ([3bef365](https://github.com/abpframework/abp/commit/3bef3650f8cbfcfc47894abdf490f12a6dc1fbb5))
* **permission-management:** fix type error ([779e161](https://github.com/abpframework/abp/commit/779e161aa8b9a379295525458fc60b97d2b33629))
* **setting-management:** fix permisson problem ([143a34e](https://github.com/abpframework/abp/commit/143a34e2440dc222158a8d5de8b831c13c2625af))
* **tenant:** add validation to connection string #resolves 1641 ([96386ef](https://github.com/abpframework/abp/commit/96386ef0e6d8c8f7f99b2c9758d6500e36ad409a))
* **tenant-management:** change actions column minimum width ([cb7df88](https://github.com/abpframework/abp/commit/cb7df881e14b563d4721f35b710c2852381438ef))
* add feature-management to angular.json ([03ae5b5](https://github.com/abpframework/abp/commit/03ae5b5ee48da4b7f42384283decf36bd1a22d2d))
* **tenant-management:** change checkbox html element ([97af208](https://github.com/abpframework/abp/commit/97af208f9a9f9d359d73f516ca58759b50254cce))
* **tenant-management:** fix default connection string modal validation issue [#1641](https://github.com/abpframework/abp/issues/1641) ([adf32c1](https://github.com/abpframework/abp/commit/adf32c1cb265c2d2e06d158cf4c0453f041c0f75))
* **theme-basic:** remove unused router ([015e94d](https://github.com/abpframework/abp/commit/015e94daf663ce81b8229a3c019d4f980f76fed9))
* **theme-shared:** change abpButton class from fa-spin to fa-pulse ([ef79664](https://github.com/abpframework/abp/commit/ef796645d96bc6968565497995049314ce2039e8))
* **theme-shared:** change button text colors to white ([9638ef1](https://github.com/abpframework/abp/commit/9638ef107baa7258b2b35e5eafbded4bcbd61b18))
* **theme-shared:** change fade animation name ([683337f](https://github.com/abpframework/abp/commit/683337f6b45442fd8d2edb19fd978aa1bd47170f))
* **theme-shared:** loader bar turning back problem ([38f5f32](https://github.com/abpframework/abp/commit/38f5f320147dcaf781a905b0d67d8634cd89173d))
* **theme-shared:** move chart.js importing in the appendScript function ([1b30f90](https://github.com/abpframework/abp/commit/1b30f90f79b4ac0187baebee1ced73aa1d8dd601))
* **theme-shared:** remove unnecessary css style ([70ec0b3](https://github.com/abpframework/abp/commit/70ec0b3a63c001f94e7eb5de4b1be6fefb625869))


### Reverts

* Revert "fix docs projects combobox link bug" ([0965eb4](https://github.com/abpframework/abp/commit/0965eb43bde4fce27037761cf2150126142a8c86))



# [0.19.0](https://github.com/abpframework/abp/compare/0.18.1...0.19.0) (2019-08-16)


### Bug Fixes

* **account:** tenant-box selection ([26c6901](https://github.com/abpframework/abp/commit/26c69017d9655d82a9d50b08f2acdc9da0d3895a))
* **angular-tutorial:** add some modules in shared.module ([5154e80](https://github.com/abpframework/abp/commit/5154e80add02833627eb44b69f31ac94b49afce6))
* **core:** check if localization key exists ([c80ba38](https://github.com/abpframework/abp/commit/c80ba38d3de0dc2446f32a043cefd70c51c7b513))
* **core:** localization empty return problem ([1afaed3](https://github.com/abpframework/abp/commit/1afaed318d4ef7c27b1106115fe1426b76bea4f4))
* **core:** redirect user to child routes instead of module root route ([298ad36](https://github.com/abpframework/abp/commit/298ad367a30a75be387199d9aca30a9e0dfbe83a))
* remove unnecessary paths ([23b50c6](https://github.com/abpframework/abp/commit/23b50c6eb1e4c6fa9fe228306d24cd19a21104b4))
* **core:** rename directives ([5e4be2a](https://github.com/abpframework/abp/commit/5e4be2ab4fec3e23461986318c7424076bb31e90))
* **core:** store tenant instead of tenant id ([b1c30fd](https://github.com/abpframework/abp/commit/b1c30fdcd15e6b9471d44fcbd2cb65290566ab60))
* **identity:** add localization key to search input placeholder ([c079dbc](https://github.com/abpframework/abp/commit/c079dbcf4f07d73e225d94f8292a837d34b991e2))
* **identity:** change request url ([e23b337](https://github.com/abpframework/abp/commit/e23b3372cdaa60269f8b7bc0ec5a2f04b19a8cb7))
* **identity:** disable input when editing a static role ([c1ab80a](https://github.com/abpframework/abp/commit/c1ab80a3efdd1c6b47fe5ede103b09049c8c6f3e))
* **identity:** hide password field of modal when editing an existing user ([c5508d2](https://github.com/abpframework/abp/commit/c5508d2f25613cb34d7df027a1a98ce7c41625aa))
* **multi-tenancy:** correctly set TenantId on create in AppService ([2a4c319](https://github.com/abpframework/abp/commit/2a4c319081a0f95e8f437a932fb84299d2fcfc9b)), closes [#1360](https://github.com/abpframework/abp/issues/1360)
* **ng-core:** add type error control ([5f24fb8](https://github.com/abpframework/abp/commit/5f24fb86c7704e1c9ea76eb11a575b70c973802a))
* **permission-management:** disabled checkbox bug in permission modal ([774c338](https://github.com/abpframework/abp/commit/774c33886bb1658f494972f7958a36f54e2a9db1))
* **permission-management:** disabled checkboxes ([23aa91d](https://github.com/abpframework/abp/commit/23aa91da5481556c92b48d562afe3736c66c16c6))
* **scripts:** add missing import ([a5048f1](https://github.com/abpframework/abp/commit/a5048f12e4ad7c6ffbb51fb89c439855dcb0e395))
* **tenant-management:** change request urls ([f869794](https://github.com/abpframework/abp/commit/f869794e163ffd3558ab5bae19bfef40cc94e3fc))
* **tenant-management:** fix request search value of abp-button and handle form submit ([0047492](https://github.com/abpframework/abp/commit/00474927dda8a2555104693e3ab4f8f33ec34b60))
* **tenant-management:** prod build error ([0dc1e1f](https://github.com/abpframework/abp/commit/0dc1e1fa87d289ea9f775ef5ad85fb646bed736d))
* **tenant-management:** remove parentName in child route ([c1cefc7](https://github.com/abpframework/abp/commit/c1cefc777ea41a38e2481e7d2b13b1551d9217c4))
* **tenant-management:** rename GetTenant action to GetTenants ([ed7cbc1](https://github.com/abpframework/abp/commit/ed7cbc18487cac5336b089f7efd94d209d192de7))
* **theme-basic:** add pointer class to nav elements ([e1a4a12](https://github.com/abpframework/abp/commit/e1a4a12d611b365d0493880c611fc84866021e59))
* **theme-basic:** change-password and profile modals ([5b6abc5](https://github.com/abpframework/abp/commit/5b6abc517f82af18c3a53a2b070ae4a4526aa9c4))
* **theme-basic:** close navbar dropdowns on window resize ([d49de5f](https://github.com/abpframework/abp/commit/d49de5f88b93ab83f7e969d7adb0a3d668709325))
* **theme-basic:** fix navigation dropdowns ([35cd51a](https://github.com/abpframework/abp/commit/35cd51a92cb6be8b06779ef32b660847639c9664))
* **theme-basic:** navigation dropdown visibility and alignment ([7451adc](https://github.com/abpframework/abp/commit/7451adcada8424621ab39f433524b6bc3d713394))
* **theme-basic:** rename EmptyLayout to EmptyLayoutComponent ([84f5132](https://github.com/abpframework/abp/commit/84f51324153e441a02f9ab45f480f5e6a5bd09ea))
* **theme-basic:** translation keys ([7311801](https://github.com/abpframework/abp/commit/73118018913f0b737ac0e649a1dc5d764daa9c63))
* **theme-shared:** button code quality ([9f9ce58](https://github.com/abpframework/abp/commit/9f9ce5810ef892806f3e31f92ecaacb5e9db7fbc))
* **theme-shared:** button-component disabled attribute ([7272912](https://github.com/abpframework/abp/commit/72729125c1add763537604779964a3630e34af06))
* **theme-shared:** fix accessibility issue with loading state of button component ([60c64fb](https://github.com/abpframework/abp/commit/60c64fb93b250e654c4d51c96e7a9a5b4ba8d585))
* **theme-shared:** improve code quality of error.handler ([2809ec8](https://github.com/abpframework/abp/commit/2809ec8d4e17fdc05e67254346d0dea7fbd08677))
* **theme-shared:** modal component ([9676ea1](https://github.com/abpframework/abp/commit/9676ea1bed44f1399190fe18c56c4110da7fa4e3))
* **theme-shared:** modal component click listening ([ae5c398](https://github.com/abpframework/abp/commit/ae5c398b8ae655d9a6e1d8a08d5b2a827dd73556))
* **theme-shared:** remove constructor function ([eed7807](https://github.com/abpframework/abp/commit/eed7807b8d0b3a9462d85673ca648cc41a4ea5c0))
* **theme-shared:** undo keyup changes ([be16db3](https://github.com/abpframework/abp/commit/be16db3898eef1207a1a0558f1f458cfd2f9d696))
* **theme-shared:** validation-error component ([700d768](https://github.com/abpframework/abp/commit/700d768807d317b68e764a563732ad717dadaae2))
* **user, permission:** bug fix ([0004677](https://github.com/abpframework/abp/commit/0004677796022b0c3b0a2c621aebf56ce0e5280f))
* change peerDependencies to dependencies in tenant-management ([161edb6](https://github.com/abpframework/abp/commit/161edb6f81174b526cb287682ecf81db7c16e382))
* child dropdown width and ellipsis width ([602d24b](https://github.com/abpframework/abp/commit/602d24bbda1c076e4a1c74766e94304fd91e915a))
* localizations ([e5c1f2e](https://github.com/abpframework/abp/commit/e5c1f2e00eb769adda9bf7a8c05995512688f8cd))
* pr feedbacks ([a1b7c6d](https://github.com/abpframework/abp/commit/a1b7c6d730c794c8626f61e1df4647325f646486))
* remove comment lines ([12bd324](https://github.com/abpframework/abp/commit/12bd3244d91637ada5e427f17b0c3891095c5d6c))
* translation keys ([ad092ca](https://github.com/abpframework/abp/commit/ad092ca11d37cf293ec452f6de0e2446b3a27895))
* update commit message sync.js ([6738b26](https://github.com/abpframework/abp/commit/6738b2694ea946f22182d3bb3c0face930497a94))


### Features

* **theme-basic:** change modal ([902731f](https://github.com/abpframework/abp/commit/902731f01d9d30dce0b79808dd85ec433a1ed95c))
* **theme-shared:** change modal style and logic ([ae93dc4](https://github.com/abpframework/abp/commit/ae93dc432ced052c5e88ec486bbe47363b84e5ee))


### Performance Improvements

* **permission-management:** handle attr.disabled while rendering to DOM ([4b09f02](https://github.com/abpframework/abp/commit/4b09f02e096fb39fdb0ca8e752ba25209a035c09))

