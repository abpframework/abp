## Abp 每日构建

 本文档将向你展示如何使用Abp框架每日构建包

有两个URL,一个是使用NuGet 2(Visual Studio 2012+),另一个URL用于NuGet 3(Visual Studio 2015+).

- NuGet v3 URL: <https://www.myget.org/F/abp-nightly/api/v3/index.json>
- NuGet v2 URL: <https://www.myget.org/F/abp-nightly/api/v2/>

在VS中打开: `工具 > 选项 > NuGet 包管理器 > 程序包源`

打开窗口后:

1. 单击绿色的`+`图标
2. 在底部输入名称(abp-nightly)和并粘贴URL到源上.
3. 单击`更新`按钮
4. 点击`确定`按钮

 然后打开 **程序包管理器控制台** 并从`程序包源`下拉菜单中选择添加的新程序包源. 然后选择`abp-nightly`同时选中`包括预发行版`复选框.
 