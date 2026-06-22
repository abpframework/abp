import { Component, ElementRef, AfterViewInit, ViewChild, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { EditorState } from '@codemirror/state';
import { EditorView, lineNumbers } from '@codemirror/view';
import { basicSetup } from 'codemirror';

@Component({
  selector: 'abp-codemirror-editor',
  template: `<div #cmHost class="codemirror-container"></div>`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CodeMirrorEditorComponent),
      multi: true,
    },
  ],
})
export class CodeMirrorEditorComponent implements AfterViewInit, ControlValueAccessor {
  @ViewChild('cmHost', { static: true })
  host!: ElementRef;

  private view!: EditorView;
  private value = '';

  private onChange = (value: string) => {};
  private onTouched = () => {};

  writeValue(value: string): void {
    this.value = value || '';

    if (this.view) {
      this.view.dispatch({
        changes: {
          from: 0,
          to: this.view.state.doc.length,
          insert: this.value,
        },
      });
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  ngAfterViewInit(): void {
    const startState = EditorState.create({
      doc: this.value,
      extensions: [
        basicSetup,
        EditorView.updateListener.of(update => {
          if (update.docChanged) {
            const text = update.state.doc.toString();
            this.onChange(text);
          }
        }),
        lineNumbers(),
      ],
    });

    this.view = new EditorView({
      state: startState,
      parent: this.host.nativeElement,
    });
  }
}
