import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appRemoveNondigits]',
})
export class RemoveNondigitsDirective {
  constructor(private el: ElementRef) {}

  @HostListener('input', ['$event']) onInput(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    input.value = input.value.replace(/\D/g, String.Empty);
  }
}
