# Tables

## Introduction

`abp-table` is the basic tag component for tables in abp.

Basic usage:

````html
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

See the [tables demo page](https://bootstrap-taghelpers.abp.io/Components/Tables) to see it in action.

## abp-table Attributes

- **responsive**: Used to create responsive tables up to a particular breakpoint. see [breakpoint specific](https://getbootstrap.com/docs/4.1/content/tables/#breakpoint-specific) for more information.
- **responsive-sm**: If not set to false, sets the table responsiveness for small screen devices.
- **responsive-md**: If not set to false, sets the table responsiveness for medium screen devices.
- **responsive-lg**: If not set to false, sets the table responsiveness for large screen devices.
- **responsive-xl**: If not set to false, sets the table responsiveness for extra large screen devices.
- **dark-theme**: If set to true, sets the table color theme to dark.
- **striped-rows**: If set to true, adds zebra-striping to table rows.
- **hoverable-rows**: If set to true, adds hover state to table rows.
- **border-style**: Sets the border style of the table. Should be one of the following values:
  - `Default` (default)
  - `Bordered`
  - `Borderless`
