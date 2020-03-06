# Buttons

ABP framework has a special Tag Helper to create bootstrap button easily.

`<abp-button>`

## Attributes

`<abp-button>` has 7 different attribute.

* [`button-type`](#button-type)
* [`size`](#`size`)
* [`busy-text`](#`busy-text`)
* [`text`](#`text`)
* [`icon`](#`icon`)
* [`disabled`](#`disabled`)
* [`icon-type`](#`icon-type`)


### `button-type`

`button-type` is a selectable parameter. It's default value is `Default`.

`<abp-button button-type="Primary">Button</abp-button>`

You can choose one of the button type listed below.

* `Default`
* `Primary`
* `Secondary`
* `Success`
* `Danger`
* `Warning`
* `Info`
* `Light`
* `Dark`
* `Outline_Primary`
* `Outline_Secondary`
* `Outline_Success`
* `Outline_Danger`
* `Outline_Warning`
* `Outline_Info`
* `Outline_Light`
* `Outline_Dark`
* `Link`

### `size`

`size` is a selectable parameter. It's default value is `Default`.

`<abp-button size="Default">Button</abp-button>`

You can choose one of the size type listed below.

* `Default`
* `Small`
* `Medium`
* `Large`
* `Block`
* `Block_Small`
* `Block_Medium`
* `Block_Large`

### `busy-text`

`busy-text` is a string parameter. IT shows the text while the button is busy.

### `text`

`text` is a string parameter that displaying on button.

### `icon`

`icon` is a string parameter. It is depending to [`icon-type`](#`icon-type`). For default, we use [Font Awesome](https://fontawesome.com/) for icons. To use it, you need to set `icon` parameter as a icon name. 

##### Example

[fa-address-card](https://fontawesome.com/icons/address-card): ![fa-address-card](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAMAAABHPGVmAAABiVBMVEUAAADWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHXWJHWiK1XyAAAAgnRSTlMAAQIDBAUGBwgJCwwNDg8QERITFBUXGBkaHR4iIyQlJicqKy4vMDEyMzQ1Njg5Ozw9PkBBQkNGSUtNTlBUVVZXWFlcXV5hY2dpbXBxc3R4fH5/gIKIj5KdnqOlpqiqq62vsLS3ur7Aw8XIys7R09XX2dze4OLk5ujp6+3v8fX3+fv9Y33KfgAAAppJREFUaN7t2flT00AUB/BNmtBCBY9iq7UetVVEqQIeVRRBQY2C94V41oK3EApSrFzdv1wSmjHXZtfZfc44s98fu7v9TEny3mZBSEZGRub/TOxmAwPlVaZl7FnEgBm3jc5VDJobFvIFAyeLUBHawFWEnoIjOI5MeGQ/asIjRxG8gQsSEYU07h3eoWnJ3EQdDNkcjzkFVL2yDoMsd7vr9M4FCORHwtsNdFM8stHt7zlda8KR0WBnK4tGfqpBRPFdFmNvipz0EzpyJ6xJj3mm1KI7ukpHDoStS3umzFL2DXSkPWyZLhhRqev+DWJGGwodSYSt07xzhjuT5HTdpSOZMGSX4OdkLAy5JBiZD0M+iS4rp4NGUXjtqscDl31BfKmf9d3FSgWiaVV1z+94HZhwKvIxuUpFpq8NvsC1/J81uTlcGRyZcu8Dv/E98Q/t65E38Uy/XcMSfRW81GNXr0lRZcW5sZQL1iZlqWZtWy47V6jYFIIMuf6wJ55vvR+tTZdcN8GAM2+Oo3bN+JuPv1a+dGaWYyo5mhGF5GgvsRn+W7hOf1We50YMOjLCjWTpSIob0eiI0uREvrKcX7znRJ6xIPe35w606eTEr5ORWyzIMGftKrMgfZxlpZ8FKXIivSzIEU7kOAty0J66qkZO6iAj51iQ3tbtftsgZ2KRjDxgQQzO52RFYUBM3rJynm6c5K5d67tpRvIX/5aocSjayC4L2XdVh3oKhBy7+FaeEklEIhKhIhvwSB59h0f2oUekIXtfXGL6lsdWs9PekIY1lCOMfA45GiIl7W7+gUxtjb0jvMn/BZKypxYIo1YjbKvBImftwcRHQGSz5DTVMx+AkJVJ93Gm2u4/fGuNJlmyvZNS/R936PK/0TIyMjLR+Q10nub4kPoc5wAAAABJRU5ErkJggg== "Address Card")

`<abp-button icon="address-card" text="Address" />`

> Don't forget: You dont need to write prefix! It will add automatically "fa" prefix for [Font Awesome](https://fontawesome.com/) icons while you did not change `icon-type`.

### `disabled`

`disabled` is a boolean parameter. If you set it `true`, your button will be disabled.

### `icon-type`

`icon-type` is a selectable parameter. It's default value is `FontAwesome`. You can create your own icon type provider and change it.

You can choose one of the size type listed below.

* `FontAwesome`
* `Other`