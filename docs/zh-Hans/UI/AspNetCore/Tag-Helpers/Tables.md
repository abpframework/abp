# 表格

## 介绍

`ABP-table` 在ABP中用于表格的基本标签组件.

基本用法:

````csharp
<abp-table hoverable-rows="true" responsive-sm="true">
        <thead>
            <tr>
                <th scope="Column">#</th>
                <th scope="Column">First</th>
                <th scope="Column">Last</th>
                <th scope="Column">Handle</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th scope="Row">1</th>
                <td>Mark</td>
                <td>Otto</td>
                <td table-style="Danger">mdo</td>
            </tr>
            <tr table-style="Warning">
                <th scope="Row">2</th>
                <td>Jacob</td>
                <td>Thornton</td>
                <td>fat</td>
            </tr>
            <tr>
                <th scope="Row">3</th>
                <td table-style="Success">Larry</td>
                <td>the Bird</td>
                <td>twitter</td>
            </tr>
        </tbody>
    </abp-table>
````

## Demo

参阅[表格demo页面](https://bootstrap-taghelpers.abp.io/Components/Tables)查看示例.

## abp-table Attributes

- **responsive**: 用于创建直至特定断点的响应表. 请参阅[特定断点](https://getbootstrap.com/docs/4.1/content/tables/#breakpoint-specific)获取更多信息.
- **responsive-sm**: 如果没有设置为false,则为小屏幕设备设置表响应性.
- **responsive-md**: 如果未设置为false,则为中等屏幕设备设置表响应性.
- **responsive-lg**: 如果未设置为false,则为大屏幕设备设置表响应性.
- **responsive-xl**: 如果未设置为false,则为超大屏幕设备设置表响应性.
- **dark-theme**: 如果设置为true,则将表格颜色主题设置为黑暗.
- **striped-rows**: 如果设置为true,则将斑马条纹添加到表行中.
- **hoverable-rows**: 如果设置为true,则将悬停状态添加到表行.
- **border-style**: 设置表格的边框样式. 应为以下值之一:
  - `Default` (默认)
  - `Bordered`
  - `Borderless`
