# Password Complexity Indicator
`PasswordComplexityIndicatorService` is for Calculating Password Complexity.

# Service
### password-complextiy-indicator.service.ts
```ts
@Injectable({providedIn: 'root'})
export class PasswordComplexityIndicatorService{
  colors: string[] = ['#B0284B', '#F2A34F', '#5588A4', '#3E5CF6', '#6EBD70'];
  texts: string[] = ['Weak', 'Fair', 'Normal', 'Good', 'Strong']

  requirements: RegexRequirementsModel = {
    minLengthRegex: /(?=.{6,})/,                                        // Default min length 6
    numberRegex: /(?=.*[0-9])/,                                         // Default isContain number
    lowercaseRegex: /(?=.*[a-z])/,                                      // Default isContainLowercase
    uppercaseRegex: /(?=.*[A-Z])/,                                      // Default isContainUppercase
    specialCharacterRegex: /^(?=.*[!@#$%^&*()\-_=+[\]{};:'"<>/?])\S+$/, // Default isContainSpecialCharacter
  };

  validatePassword(password: string): ProgressBarStats {
    let passedCounter = 0;
    Object.values(this.requirements).forEach((reg:RegExp) => {
      const isValid = reg.test(password);
      
      if(isValid){
        passedCounter++;
      }
    })
    return { bgColor:this.colors[passedCounter - 1], text:this.texts[passedCounter - 1], width: (100 / this.texts.length) * passedCounter };
  }
}
```
- In PasswordComplexityIndicatorService we set default values for complexity indicator bar. These are;
  - regex
  - colors
  - texts
- Make sure these values length are equal (In our example we have **5** tests/colors/texts).
- We have only one method validatePassword which we pass password as an argument. At the end this method will return the properties of the complexity bar.
# Component

```ts
@Component({
  selector: 'abp-password-complexity-indicator',
  template: `
    <div [style.opacity]="progressBar?.width > 0 ? 1 : 0" [style.backgroundColor]="'var(--lpx-border-color)'" class="progress transition mx-3">
      <div class="progress-bar transition" [style.width]="progressBar?.width + '%'" [style.backgroundColor]="progressBar?.bgColor"></div>
    </div>
  `,
  styles: [`
      .transition { transition: all 0.3s ease-out; }
      .progress { backgroundColor: var(--lpx-border-color); height: 4px; border-radius:3px 3px 0 0; margin-top:-4px; z-index:1; position:relative}
      :host-context{ order:1 }
    `
  ]
})
export class PasswordComplexityIndicatorComponent{
  @Input({required:true}) progressBar? : ProgressBarStats;
}
```
- abp-password-complexity-indicator component is **takes only one required input which type is ProgressBarStats**.
- ```ts
  interface ProgressBarStats{
    bgColor: string,
    text: string,
    width: number
  })
  ``` 
- as you can see from interface above, progressBar input has;
  - ***bgColor:*** decides color of the bar.
  - ***text:*** () // TODO bu belki optional olabilir düzelt
  - ***width:*** decides how full the bar will be


# How To Use
İt's easy, imagine you have password input that you want to add complexity indicator under it.

```ts
@Component({
  selector: 'myComponent',
  templateUrl: `
    <form [formGroup]="form">
      <label for="input-password">
        {{ 'AbpAccount::Password' | abpLocalization }}
        <ng-container *ngIf="progressBar?.width > 0">
          {{'AbpAccount::Strength' | abpLocalization}}
          <span [style.color]="progressBar?.bgColor">{{'AbpAccount::' + progressBar?.text | abpLocalization}}</span>
        </ng-container>
      </label>  
      <input id="input-password" type="password" class="form-control" formControlName="password" (keyup)="validatePassword()"/>
      <div class="mt-3">
        <abp-password-complexity-indicator [progressBar]="progressBar"></abp-password-complexity-indicator>
      </div>
    </form>
  `,
})
export class myComponent{
  this.form = this.fb.group({
    password: [''],
  });
  progressBar: ProgressBarStats;
  private readonly passwordComplexityService: PasswordComplexityIndicatorService = inject(PasswordComplexityIndicatorService);
  private readonly fb: UntypedFormBuilder = inject(UntypedFormBuilder);

  get password(): string{
    return this.form.get('password').value;
  }

  validatePassword(){
    this.passwordProgressBarStats = this.passwordComplexityService.validatePassword(this.password);
  }
}
```

- Give the password to `PasswordComplexityIndicatorService`'s `validatePassword` method, and equalize returned value with the `this.progressBar`.
- In our component we used color and text value in template for better looking.

# How To Customize
- If you want to change the test count be sure that in `PasswordComplexityIndicatorService` `colors,texts,regex` arrays length are equal.
- Instead of using text value directly make it localizable.
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
- If you change texts, you must change the localization file.
- Thats it you can start typing to password input :)
