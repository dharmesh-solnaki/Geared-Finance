import {
  Component,
  Input,
  OnInit,
  HostListener,
  ElementRef,
  forwardRef,
  SimpleChanges,
  EventEmitter,
  Output,
  ViewChild,
} from '@angular/core';
import { selectMenu } from '../constants';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
@Component({
  selector: 'app-common-selectmenu',
  templateUrl: './common-selectmenu.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CommonSelectmenuComponent),
      multi: true,
    },
  ],
})
export class CommonSelectmenuComponent implements OnInit, ControlValueAccessor {
  @Input() optionData: selectMenu[] = [];
  @Input() defaultOption: string = '';
  @Input() defaultValue: string | number | boolean = '';
  @Input() needsSearching: boolean = false;
  @Input() isShowMultipleSelect: boolean = false;
  @Output() valueChangeEmitter = new EventEmitter();
  @Output() checkListEmitter = new EventEmitter();
  @Output() checkListShiftEmitter = new EventEmitter();
  @ViewChild('searchFromSelectList') searchFromSelectList!: ElementRef;
  selectedValue: string | number | boolean = '';
  selectedOption: string | number = '';
  isMenuOpen: boolean = false;
  @Input() workingOptionData: selectMenu[] = [];
  // isFieldRequired: boolean = false;
  checkedList: selectMenu[] = [];
  constructor(private elementRef: ElementRef) {}
  onChange: any = () => {};
  onTouched: any = () => {};

  writeValue(value: any): void {
    if (value) {
      this.selectedValue = value;
      const selectedOption = this.optionData.find(
        (option) => option.value.toString() == value.toString()
      );
      if (selectedOption) {
        this.selectedOption = selectedOption.option;
      }
    }
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  updateTheOptionData(newData: selectMenu[]) {
    this.optionData = newData;
    this.workingOptionData = this.optionData;
  }

  ngOnChanges(changes: SimpleChanges): void {
    const currentValue = changes['optionData']?.currentValue;

    if (
      currentValue == null ||
      (typeof currentValue === 'string' && currentValue.trim() === '') ||
      (Array.isArray(currentValue) && currentValue.length === 0) ||
      Object.keys(currentValue).length === 0
    ) {
      this.resetElement();
      this.optionData = [{ option: 'No data', value: 0 }];
    } else {
      this.optionData = currentValue;
    }
    this.workingOptionData = this.optionData;
  }
  ngOnInit(): void {
    this.selectedValue = this.defaultValue;
    this.selectedOption = this.defaultOption;

    if (!this.optionData.isNotEmpty()) {
      this.optionData = [{ option: 'No data', value: 'Select' }];
    }
    this.workingOptionData = this.optionData;
  }

  valueChangeHadler(item: selectMenu) {
    this.selectedValue = item.value;
    this.selectedOption = item.option;
    this.isMenuOpen = false;
    this.onChange(this.selectedValue);
    this.onTouched();
    this.valueChangeEmitter.emit(item.value);
  }

  resetElement() {
    this.selectedValue = this.defaultValue;
    this.selectedOption = this.defaultOption;
    this.isMenuOpen = false;
    if (this.needsSearching) {
      if (
        this.searchFromSelectList &&
        this.searchFromSelectList.nativeElement
      ) {
        this.searchFromSelectList.nativeElement.value = '';
      }
    }
  }

  menuToggler() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  filterHandler(searchString: string) {
    if (searchString.trim() === '') {
      this.workingOptionData = this.optionData;
    } else {
      this.workingOptionData = this.optionData.filter((x) =>
        x.option.toLowerCase().includes(searchString.toLowerCase())
      );
    }
  }
  setDefaults(item: selectMenu) {
    this.selectedValue = item.value;
    this.selectedOption = item.option;
    this.isMenuOpen = false;
    this.onChange(this.selectedValue);
    this.onTouched();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isMenuOpen = false;
    }
  }
  handleOptionCheckboxChange(item: selectMenu) {
    if (!this.checkedList.some((x) => x == item)) {
      this.checkedList.push(item);
    } else {
      this.checkedList = this.checkedList.filter((x) => x != item);
    }
    const selectedChecks = this.checkedList.map((x) => +x.value);
    this.checkListEmitter.emit(selectedChecks);
  }
  handleListCheboxChange(isForRemove: boolean) {
    this.checkedList = [];
    if (!isForRemove) {
      this.checkedList = [...this.workingOptionData];
    } else {
      this.selectedOption = this.defaultOption;
    }

    const selectedChecks = this.checkedList.map((x) => +x.value);
    this.checkListEmitter.emit(selectedChecks);
  }
  removeFromTheCheckList() {
    let item = this.checkedList.shift();
    this.checkListShiftEmitter.emit(item);
  }
}
