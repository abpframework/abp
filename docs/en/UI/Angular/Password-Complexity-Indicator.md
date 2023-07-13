# Password Complexity Indicator
`PasswordComplexityIndicatorService` is for Calculating Password Complexity.

# Service
### password-complextiy-indicator.service.ts
```ts
import { Injectable } from  "@angular/core";
import { ProgressBarModel, RegexRequirementsModel } from  '../models/password-complexity';

@Injectable({providedIn: 'root'})
export  class  PasswordComplexityIndicatorService {

progressBar: ProgressBarModel;

stats: ProgressBarModel[] = [
  { bgColor: '', width: 0 },
  { bgColor: 'danger', width: 20 },
  { bgColor: 'warning', width: 40 },
  { bgColor: 'info', width: 60 },
  { bgColor: 'primary', width: 80 },
  { bgColor: 'success', width: 100 },
];

requirements: RegexRequirementsModel  = {
  minLengthRegex: /(?=.{6,})/, 						  // min length 6
  numberRegex: /(?=.*[0-9])/,  						  // isContain number
  lowercaseRegex: /(?=.*[a-z])/,                                	  // isContainLowercase
  uppercaseRegex: /(?=.*[A-Z])/, 					  // isContainUppercase
  specialCharacterRegex: /^(?=.*[!@#$%^&*()\-_=+[\]{};:'"<>/?])\S+$/,     // isContainSpecialCharacter
};

validatePassword(password: string): ProgressBarModel {
  let  passedCounter  =  0;

  Object.values(this.requirements).forEach((reg:RegExp)=>{
    const  isValid  =  reg.test(password);

    if(isValid){
      passedCounter++;
    }
  })

  return this.stats[passedCounter];
}}
```
- As you can see from the code above we set default regular expressions tests and width values. 
- You can change tests and bg-color as you wish, ( **These colors must be bootstrap colors**)
- if you change the tests, don't forget to change the width values
# Component
```ts
import { Component, Input } from  '@angular/core';
import { ProgressBarModel } from  '../../models/password-complexity';

@Component({
  selector: 'abp-password-complexity-indicator',
  template: `
    <div *ngIf="progressBar.width  !=  0" class="progress" attr.aria-valuenow="{{progressBar.width}}"  aria-valuemin="0" aria-valuemax="100">
      <div class="progress-bar bg-{{progressBar.bgColor}}" [style.width]="progressBar.width  +  '%'"></div>
    </div>
  `
})
export  class  PasswordComplexityIndicatorComponent{
  @Input({required:true}) progressBar! : ProgressBarModel;
}
```
- abp-password-complexity-indicator component is **takes only one required input which type is ProgressBarModel** ( { bgColor:string, width: number } )



# How To Use
İt's easy, imagine you have password input that you want to add complexity indicator under it.

```ts
@Component({
  selector: 'myComponent',
  templateUrl: `
    <form [formGroup]="form">
      <input inputId="new-password" formControlName="password" (keyup)="validatePassword()">
      <div  class="mt-3">
        <abp-password-complexity-indicator [progressBar]="progressBar"></abp-password-complexity-indicator>
      </div>
    </form>
  `,
})
export class myComponent{
  this.form = this.fb.group({
    password: [''],
  });
  progressBar: ProgressBarModel = { bgColor: '', width: 0 }
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

- give the password to passwordComplexityService's validatePassword method, and equalize returned value with the this.progressBar
- thats it you can start typing :)
