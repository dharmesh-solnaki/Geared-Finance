import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberWithPrecisionPipe',
})
export class NumberWithPrecisionPipe implements PipeTransform {
  transform(value: string | number): string {
    if (!value) {
      return '0.00';
    }
    console.log(value);
    let stringValue = value.toString().replace(/[^0-9.]/g, '');
    const parts = stringValue.split('.');

    // Ensure only one '.' exists
    if (parts.length > 2) {
      stringValue = `${parts[0]}.${parts.slice(1).join('')}`;
    }

    // Restrict to two decimal places
    if (parts[1]) {
      parts[1] = parts[1].slice(0, 2);
      stringValue = `${parts[0]}.${parts[1]}`;
    }

    // Ensure exactly two decimal places

    const decimalLength = parts[1]?.length || 0;

    if (decimalLength == 0) {
      stringValue = stringValue + '.00';
    }
    if (decimalLength < 2 && decimalLength != 0) {
      parts[1] = (parts[1] || '0').padEnd(2, '0');
    }

    stringValue = parseFloat(stringValue).toFixed(2);

    return stringValue;
  }
}
