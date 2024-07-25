import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneFormatting'
})
export class PhonePipe implements PipeTransform {
  // [value]="phoneFormatting | udpCurrency"
  transform(value: string|null ) {
    if(typeof(value)=="string"){
      const firstPart = value.slice(0, 4);
      const secondPart = value.slice(4, 7);
      const thirdPart = value.slice(7, 10);
      return `${firstPart} ${secondPart} ${thirdPart}`
    }else{
      return value;
    }
  }

}
