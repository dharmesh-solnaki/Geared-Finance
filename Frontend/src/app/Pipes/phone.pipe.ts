import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneMasking'
})
export class PhonePipe implements PipeTransform {
  transform(value: string|null ) {
    if(typeof(value)=="string" && value.length==10){

        const firstPart = value.slice(0, 4);
      const secondPart = value.slice(4, 7);
      const thirdPart = value.slice(7, 10);
      return `${firstPart} ${secondPart} ${thirdPart}`
   
    }else{
      return value || '';
    }
  }

}
