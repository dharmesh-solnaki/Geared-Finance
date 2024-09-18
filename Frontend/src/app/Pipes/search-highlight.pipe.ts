import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'searchHighlight',
})
export class SearchHighlightPipe implements PipeTransform {
  transform(value: string, search: string): string {
    if (!search) {
      return value;
    }
    const matchedRegex = new RegExp(`(${search})`, 'gi');
    return value.replace(matchedRegex, `<strong>$1</strong>`);
  }
}
