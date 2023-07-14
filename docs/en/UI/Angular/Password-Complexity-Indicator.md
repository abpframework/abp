# Password Complexity Indicator
`PasswordComplexityIndicatorService` is for Calculating Password Complexity.

# Service
### password-complextiy-indicator.service.ts
```ts
import { Injectable } from "@angular/core";
import { ProgressBarStats } from '../models/password-complexity';

@Injectable({providedIn: 'root'})
  colors: string[] = [
    '#B0284B',
    '#F2A34F',
    '#FFE500',
    '#A4BD6E',
    '#6EBD70',
  ];

  requirements: RegexRequirementsModel = {
    minLengthRegex: /(?=.{6,})/,                                        // Default min length 6
    numberRegex: /(?=.*[0-9])/,                                         // Default isContain number
    lowercaseRegex: /(?=.*[a-z])/,                                      // Default isContainLowercase
    uppercaseRegex: /(?=.*[A-Z])/,                                      // Default isContainUppercase
    specialCharacterRegex: /^(?=.*[!@#$%^&*()\-_=+[\]{};:'"<>/?])\S+$/, // Default isContainSpecialCharacter
  };

  validatePassword(password: string): ProgressBarStats {
    let passedCounter = 0;

    Object.values(this.requirements).forEach((reg:RegExp)=>{
      const isValid = reg.test(password);
      
      if(isValid){
        passedCounter++;
      }
    })
    return { bgColors:this.colors, passedTestCount: passedCounter};
  }
```
- As you can see from the code above we set default regular expressions requirements and colors. 
- You can change tests and colors as you wish
- **but remember if you change the requirements, it's length must be equal with the colors array**
# Component
```ts
import { Component, Input } from  '@angular/core';
import { ProgressBarModel } from  '../../models/password-complexity';

@Component({
  selector: 'abp-password-complexity-indicator',
  template: `
    <div [style.opacity]="progressBar?.passedTestCount > 0 ? 1 : 0" class="d-flex justify-content-between mx-1 transition" style="height: 8px;">
      <div *ngFor="let color of progressBar?.bgColors; let i=index;" class="progress w-100 me-1" >
        <div class="progress-bar transition" [style.background]="progressBar.passedTestCount > i ? color : 'var(--lpx-border-color)'" [style.width]="progressBar.width < 20 ? '0%' : '100%'"></div>
      </div>
    </div>
  `,
  styles: [`
      .transition { transition: all 0.2s ease-out; }
      .progress {backgroundColor: var(--lpx-border-color); height: 8px; border-radius:2px}
    `
  ]
})
export  class  PasswordComplexityIndicatorComponent{
  @Input({required:true}) progressBar? : ProgressBarStats;
}
```
- abp-password-complexity-indicator component is **takes only one required input which type is ProgressBarStats**
- ```ts
  interface ProgressBarStats{
    bgColor:string, width: number
  })
  ``` 



# How To Use
İt's easy, imagine you have password input that you want to add complexity indicator under it.

```ts
@Component({
  selector: 'myComponent',
  templateUrl: `
    <form [formGroup]="form">
      <input inputId="new-password" formControlName="password" (keyup)="validatePassword()">
      <div class="mt-3">
        <abp-password-complexity-indicator [progressBar]="passwordProgressBarStats"></abp-password-complexity-indicator>
      </div>
    </form>
  `,
})
export class myComponent{
  this.form = this.fb.group({
    password: [''],
  });
  passwordProgressBarStats: ProgressBarStats;
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

- give the password to `PasswordComplexityIndicatorService`'s `validatePassword` method, and equalize returned value with the `this.passwordProgressBarStats`
- thats it you can start typing to password input :)
