# Password Complexity Indicator

The `PasswordComplexityIndicatorService` is for calculating the password complexity.

- Set default values in `PasswordComplexityIndicatorService`:
  - colors
  - texts
  - requirements
- **How we set default values**
  - ```ts
    colors: string[] = ['#B0284B', '#F2A34F', '#5588A4', '#3E5CF6', '#6EBD70'];
  
    texts: string[] = ['Weak', 'Fair', 'Normal', 'Good', 'Strong'];
  
    requirements: RegexRequirementsModel = {
      minLengthRegex: /(?=.{6,})/,                                        // Default min length 6
      numberRegex: /(?=.*[0-9])/,                                         // Default isContain number
      lowercaseRegex: /(?=.*[a-z ])/,                                     // Default isContainLowercase
      uppercaseRegex: /(?=.*[A-Z])/,                                      // Default isContainUppercase
      specialCharacterRegex: /[^a-zA-Z0-9 ]+/,                            // Default isContainSpecialCharacter
    };
    ```
- Make sure that the lengths of these values are equal (In our service we have **5** tests/colors/texts).
- The `PasswordComplexityIndicatorService` has only one method `validatePassword` that passes the password as an argument and returns the properties of the bar.

- The `validatePassword` method returns an object **of the type ProgressBarStats**.
- ```ts
  interface ProgressBarStats{
    bgColor: string,
    text?: string,
    width: number
  })
  ``` 
- Use this object to modify the `password complexity bar`
  - ***bgColor:*** decides the color of the bar.
  - ***text:*** explains the meaning of the bar to the user. 
  - ***width:*** decides how full the bar will be.


# How To Use

It's easy, imagine you have a password input that you want to add the complexity indicator under. Put this component under the input

```ts
  <abp-password-complexity-indicator [progressBar]="ProgressBarStatsObject"></abp-password-complexity-indicator>
```

- Pass the password to the `validatePassword` method of the `PasswordComplexityIndicatorService`, and bind return the value to the `progressBar` property of the `abp-password-complexity-indicator` 
- We suggest localization instead of using the text value directly.
- en.json
  ```json
    ....
    "Strength": "Strength",
    "Weak": "Weak!",
    "Fair": "Fair.",
    "Normal": "Normal.",
    "Good": "Good.",
    "Strong": "Strong!"
  ``` 

# How To Customize

- If you want to change the test count, make sure that the lengths of the arrays of the `colors,texts,regex` in the `PasswordComplexityIndicatorService` are equal. Otherwise, it won't work.
- If you change any of the texts, you must change the localization file.
- That's it, you can start typing the password input!
