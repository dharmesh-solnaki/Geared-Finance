import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appPhoneMasking]',
})
export class PhoneMaskingDirective {
  constructor(private el: ElementRef<HTMLInputElement>) {}

  @HostListener('input', ['$event']) onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const trimmedValue = input.value.replace(/\D/g, '');
    const formattedValue = this.formatPhoneNumber(trimmedValue);
    input.value = formattedValue;
  }

  formatPhoneNumber(value: string): string {
    const firstPart = value.slice(0, 4);
    const secondPart = value.slice(4, 7);
    const thirdPart = value.slice(7, 10);

    let formattedValue = '';
    if (firstPart) {
      formattedValue += firstPart;
    }
    if (secondPart) {
      formattedValue += ' ' + secondPart;
    }
    if (thirdPart) {
      formattedValue += ' ' + thirdPart;
    }
    return formattedValue;
  }
}
