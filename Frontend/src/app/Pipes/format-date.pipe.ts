import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate',
})
export class FormatDatePipe implements PipeTransform {
  transform(value: string | number, type?: string): string {
    if (!value) {
      return String.Empty;
    }

    if (type == 'date') {
      return new Date(value).toLocaleString('en-AU', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        timeZone: 'Australia/Sydney',
      });
    }

    return new Date(+value)
      .toLocaleString('en-AU', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
        timeZone: 'Australia/Sydney',
      })
      .replace(',', '');
  }
}
