import {
  Component,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
} from '@angular/core';

@Component({
  selector: 'app-common-dialog',
  templateUrl: './common-dialog.component.html',
})
export class CommonDialogComponent {
  @Input() modalTitle: string = '';
  @Input() bodyData: any;
  @Input() modalClass: string = '';
  @Input() modalId: string = '';
  @Input() isShowSaveBtn: boolean = false;
  @Input() isShowAddBtn: boolean = false;
  @Input() isDisableSaveBtn: boolean = false;
  @Input() isDisableAddBtn: boolean = false;

  @Output() onModalCloseEventEmitter = new EventEmitter();
  @Output() onSaveEventEmitter = new EventEmitter();
  @Output() onAddEventEmitter = new EventEmitter();
  @Output() onSaveEmitter = new EventEmitter();
  @Output() onCloseEmitter = new EventEmitter();

  get isTemplate(): boolean {
    return this.bodyData instanceof TemplateRef;
  }
}
