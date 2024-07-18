import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
  SortOrder,
} from '../../Models/common-grid.model';
import { recordsPerPage, selectMenu } from '../../Models/constants.model';
import { CommonSelectmenuComponent } from '../common-selectmenu/common-selectmenu.component';

@Component({
  selector: 'app-common-grid',
  templateUrl: './common-grid.component.html',
  styleUrls: ['./common-grid.component.css'],
})
export class CommonGridComponent {
  public _gridSettings: IGridSettings;
  public paginationSetting!: PaginationSetting;
  public showPagination: boolean = true;
  public pageSize!: number;
  public displayData: any = [];
  public pageSizeOptions: selectMenu[] = [];
  pageNumbers: number[] = [];
  defaultSettings: IGridSettings = {
    columns: [],
    showPagination: false,
    pageSizeValues: [
      { pageNo: 25, text: '25 per page' },
      { pageNo: 50, text: '50 per page' },
      { pageNo: 100, text: '100 per page' },
    ],
  };
  defaultPaginationSetting: PaginationSetting = {
    currentPage: 1,
    totalRecords: 25,
    selectedPageSize: ['25 per page'],
  };

  @ViewChild('pageSizerChild') pageSizerChild!:CommonSelectmenuComponent

  
  constructor() {
    this._gridSettings = this.defaultSettings;
  }

  @Input() data: any[] = [];
  @Input() isEditable:boolean=false;
  @Input() public set gridSettings(value: IGridSettings) {
    if (value) {
      this._gridSettings = value;

    } else {
      this._gridSettings = Object.assign(this.defaultSettings);
    }

    if (value && value.showPagination && !value.showPagination) {
      this.showPagination = value.showPagination;
    }
  }

  @Input() public set paginationSettings(value: PaginationSetting) {
    if (value) {
      this.paginationSetting = {
        currentPage: value.currentPage,
        totalRecords: value.totalRecords,
        selectedPageSize: [value.selectedPageSize![0] + ' per page'],
      };
    } else {
      this.paginationSetting = this.defaultPaginationSetting;
    }
    this.pageSize = +this.paginationSetting.selectedPageSize![0].split(' ')[0];
    this.updateDisplayedData();
  }

  @Output() onSortEvent = new EventEmitter<SortConfiguration>();
  @Output() onEditEvent =new EventEmitter<number>();
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.this.

    this.updateDisplayedData();
    // this.pagSizeSetter();
  
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.updateDisplayedData();
    this.pagSizeSetter();
    this.updatePageNumbers();
  }

  onSetEdit(record:number){
    if(this.isEditable){
      this.onEditEvent.emit(record)
    }
  }
  pageSizeChangeHandler(ev:number|string){
   this.pageSize=+ev
   this.paginationSetting.currentPage=1
   this.updateDisplayedData();
    this.updatePageNumbers();
   
  }
  // -----------Sorting----------------
  public sortOrder: SortOrder = SortOrder.ASC;
  public previousSort: string = '';

  sort(col: string, sort: boolean) {
    if (sort) {
      this.sortOrder = (col === this.previousSort && this.sortOrder === SortOrder.ASC)
        ? SortOrder.DESC
        : SortOrder.ASC;

      this.previousSort = col;

      const sortDetails: SortConfiguration = { sort: col, sortOrder: this.sortOrder };
      this.onSortEvent.emit(sortDetails);
      this.updateDisplayedData();
   
     if(  this.isShowPagination() ){
      this.paginationSettings.currentPage=1;
     }   

    }
  }
 

  ///-----------------pagination

  isShowPagination() {
    return (
      this._gridSettings.showPagination &&
      this.displayData.length > 0 &&
      this.paginationSetting.totalRecords > this.pageSize
    );
  }

  updateDisplayedData() {
  this.displayData = this.data
  }

  goToPreviousPage() {
    if (this.paginationSetting.currentPage > 1) {
      this.paginationSetting.currentPage--;
      this.updateDisplayedData();
    }
  }

  goToNextPage() {
    if (!this.isLastPage()) {
      this.paginationSetting.currentPage++;
      this.updateDisplayedData();
    }
  }

  isLastPage(): boolean {
    const lastPage = Math.ceil(
      this.paginationSetting.totalRecords / +this.pageSize
    );
    return this.paginationSetting.currentPage >= lastPage;
  }

  goToPage(page: number) {
    this.paginationSetting.currentPage = page;
    this.updateDisplayedData();
  }

  updatePageNumbers() {
    const totalPages = Math.ceil(
      this.paginationSetting.totalRecords / +this.pageSize
    );
    this.pageNumbers = Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  pagSizeSetter() {
    if (this._gridSettings && this._gridSettings.pageSizeValues) {
      this.pageSizeOptions.splice(0)
      this._gridSettings.pageSizeValues.forEach((item) => {
   
        this.pageSizeOptions.push({ option: item.text, value: item.pageNo });
      });
    }
    this.pageSize = +this.pageSizeOptions[0].value

  }
}
