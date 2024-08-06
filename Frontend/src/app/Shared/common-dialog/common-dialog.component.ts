import { Component, EventEmitter, Input, Output, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-common-dialog',
  templateUrl: './common-dialog.component.html',
})
export class CommonDialogComponent {
  @Input() modalTitle:string=''
  @Input() bodyData:any
  @Input() modalClass:string=''
 @Input() modalId:string=''
  @Output() onModalCloseEventEmitter = new EventEmitter();

 get isTemplate():boolean{
  return this.bodyData instanceof TemplateRef
 }
}
