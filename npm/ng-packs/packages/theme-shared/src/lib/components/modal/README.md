<h1> Abp Modal </h1>

Example Usage:

```html
<abp-modal size="md" [(visible)]="isModalShow" [centered]="false">
  <ng-template #abpHeader>
    <h3>Modal Title</h3>
  </ng-template>

  <ng-template #abpBody>
    <form [formGroup]="form">
      <input formControlName="name" />
      <input formControlName="surname" />
    </form>
  </ng-template>

  <ng-template #abpFooter>
    <button #abpClose type="button" class="btn btn-secondary">Close</button>
    <button type="button" class="btn btn-primary" (click)="saveChanges()">Save changes</button>
  </ng-template>
</abp-modal>

<button (click)="isModalShow = true">Click</button>
```
