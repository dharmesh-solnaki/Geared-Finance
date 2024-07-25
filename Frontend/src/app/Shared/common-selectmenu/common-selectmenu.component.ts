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
} from '@angular/core';
import { selectMenu } from '../../Models/constants.model';
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
  @Input() defaultValue: string | number = '';
  @Input() needsSearching: boolean = false;
  @Output() valueChangeEmitter = new EventEmitter<number | string>();
  selectedValue: string | number = '';
  selectedOption: string | number = '';
  isMenuOpen: boolean = false;
  @Input() workingOptionData: selectMenu[] = [];
  isFieldRequired: boolean = false;

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
    // this.resetElement()
   const currentValue = changes['optionData']?.currentValue;

  if (
    currentValue == null ||
    (typeof currentValue === 'string' && currentValue.trim() === '') ||
    (Array.isArray(currentValue) && currentValue.length === 0) ||
    Object.keys(currentValue).length===0
  ) {
    this.resetElement()
    this.optionData = [{ option: 'No data', value: 0 }];
  } else {    
    this.optionData = currentValue;
  }
  // this.optionData = changes['optionData'].currentValue
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
  setDefaults(item: selectMenu){
    console.log(item,'fromsad')
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
}
