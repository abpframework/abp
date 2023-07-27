# Password Complexity Indicator
`PasswordComplexityIndicatorService` is for calculating the password complexity.

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
    return { bgColor:this.colors[passedCounter - 1], text:this.texts[passedCounter - 1], width: (100 / this.colors.length) * passedCounter };
  }
}
```
- Set the default values for the complexity indicator bar with the:
  - regex
  - colors
  - texts
- Make sure that the lengths of these values are equal (In our service we have **5** tests/colors/texts).
- `PasswordComplexityIndicatorService` has only one method `validatePassword` that passes the password as an argument and returns the properties of the bar.
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
- The abp-password-complexity-indicator component **takes only one required input which type is ProgressBarStats**.
- ```ts
  interface ProgressBarStats{
    bgColor: string,
    text?: string,
    width: number
  })
  ``` 
- as you can see from the interface above, the progressBar input has:
  - ***bgColor:*** decides the color of the bar.
  - ***text:*** explains the meaning of the bar to the user. 
  - ***width:*** decides how full the bar will be.


# How To Use
İt's easy, imagine you have a password input that you want to add the complexity indicator under.

```ts
@Component({
  selector: 'myComponent',
  templateUrl: `
    <form [formGroup]="form">
      <label class="form-label" for="input-password">
        Password
        <ng-container *ngIf="progressBar?.width > 0">
          Strength
          <span [style.color]="progressBar?.bgColor">{{progressBar?.text}}</span>
        </ng-container>
      </label>  
      <input id="input-password" type="password" class="form-control" formControlName="password" (keyup)="validatePassword()"/>
      <abp-password-complexity-indicator [progressBar]="progressBar"></abp-password-complexity-indicator>
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
    this.progressBar = this.passwordComplexityService.validatePassword(this.password);
  }
}
```

- Pass the password to the `validatePassword` method of `PasswordComplexityIndicatorService`, and equalize the returned value with the `this.progressBar`.
- In our component we used the color and text value in the template for a better look.
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
- That's it you can start typing the password input!
