import { isPlatformBrowser } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  Component,
  AfterViewInit,
  ViewChild,
  ElementRef,
  forwardRef,
  inject,
  PLATFORM_ID,
  DestroyRef,
} from '@angular/core';
import Editor from '@toast-ui/editor';

import { AbpLocalStorageService } from '@abp/ng.core';
import { THEME_CHANGE_TOKEN } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-toastui-editor',
  template: `<div #editorContainer></div>`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ToastuiEditorComponent),
      multi: true,
    },
  ],
})
export class ToastuiEditorComponent implements AfterViewInit, ControlValueAccessor {
  @ViewChild('editorContainer', { static: true })
  editorContainer!: ElementRef<HTMLDivElement>;

  private editor!: Editor;
  private value = '';

  private platformId = inject(PLATFORM_ID);
  private localStorageService = inject(AbpLocalStorageService);
  private destroyRef = inject(DestroyRef);
  private themeChange$ = inject(THEME_CHANGE_TOKEN, { optional: true });

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value = value || '';
    if (this.editor) {
      this.editor.setMarkdown(this.value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  ngAfterViewInit(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.initializeEditor();
    if (this.themeChange$) {
      this.setupThemeListener();
    }
  }

  private getTheme(): string {
    return this.localStorageService.getItem('LPX_THEME') || 'light';
  }

  private initializeEditor(): void {
    const theme = this.getTheme();
    this.editor = new Editor({
      el: this.editorContainer.nativeElement,
      previewStyle: 'tab',
      height: '500px',
      theme: theme,
      initialValue: this.value,
    });

    this.editor.addHook('change', () => {
      const value = this.editor.getMarkdown();
      this.onChange(value);
    });
  }

  private setupThemeListener(): void {
    this.themeChange$!.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(style => {
      if (!this.editor) {
        return;
      }
      const wrapper =
        this.editorContainer.nativeElement.querySelector('.toastui-editor-defaultUI') ??
        this.editorContainer.nativeElement;

      const { styleName } = style;

      switch (styleName) {
        case 'dark':
          wrapper.classList.add('toastui-editor-dark');
          break;
        case 'system':
          const isSystemDark =
            typeof window !== 'undefined' &&
            window.matchMedia('(prefers-color-scheme: dark)').matches;
          if (!isSystemDark) {
            wrapper.classList.remove('toastui-editor-dark');
          } else {
            wrapper.classList.add('toastui-editor-dark');
          }
          break;
        default:
          wrapper.classList.remove('toastui-editor-dark');
      }
    });
  }
}
