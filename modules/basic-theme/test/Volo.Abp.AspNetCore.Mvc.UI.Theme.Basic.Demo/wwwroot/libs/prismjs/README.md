# [Prism](https://prismjs.com/)

[![Build Status](https://travis-ci.org/PrismJS/prism.svg?branch=master)](https://travis-ci.org/PrismJS/prism)
[![npm](https://img.shields.io/npm/dw/prismjs.svg)](https://www.npmjs.com/package/prismjs)

Prism is a lightweight, robust, elegant syntax highlighting library. It's a spin-off project from [Dabblet](https://dabblet.com/).

You can learn more on [prismjs.com](https://prismjs.com/).

[Why another syntax highlighter?](https://lea.verou.me/2012/07/introducing-prism-an-awesome-new-syntax-highlighter/#more-1841)

[More themes for Prism!](https://github.com/PrismJS/prism-themes)

## Contribute to Prism!

Prism depends on community contributions to expand and cover a wider array of use cases. If you like it, considering giving back by sending a pull request. Here are a few tips:

- Read the [documentation](https://prismjs.com/extending.html). Prism was designed to be extensible.
- Do not edit `prism.js`, it’s just the version of Prism used by the Prism website and is built automatically. Limit your changes to the unminified files in the `components/` folder. `prism.js` and all minified files are also generated automatically by our build system.
- The build system uses [gulp](https://github.com/gulpjs/gulp) to minify the files and build `prism.js`. With all of Prism's dependencies installed, you just need to run the command `npm run build`.
- Please follow the code conventions used in the files already. For example, I use [tabs for indentation and spaces for alignment](http://lea.verou.me/2012/01/why-tabs-are-clearly-superior/). Opening braces are on the same line, closing braces on their own line regardless of construct. There is a space before the opening brace. etc etc.
- Please try to err towards more smaller PRs rather than few huge PRs. If a PR includes changes I want to merge and changes I don't, handling it becomes difficult.
- My time is very limited these days, so it might take a long time to review longer PRs (short ones are usually merged very quickly), especially those modifying the Prism Core. This doesn't mean your PR is rejected.
- If you contribute a new language definition, you will be responsible for handling bug reports about that language definition.
- If you [add a new language definition](https://prismjs.com/extending.html#creating-a-new-language-definition) or plugin, you need to add it to `components.json` as well and rebuild Prism by running `npm run build`, so that it becomes available to the download build page. For new languages, please also add a few [tests](https://prismjs.com/test-suite.html) and an example in the `examples/` folder.
- Go to [prism-themes](https://github.com/PrismJS/prism-themes) if you want to add a new theme.

Thank you so much for contributing!!

## Translations

* [![中文说明](http://awesomes.oss-cn-beijing.aliyuncs.com/readme.png)](https://www.awesomes.cn/repo/PrismJS/prism)
