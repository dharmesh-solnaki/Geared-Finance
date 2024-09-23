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
import { selectMenu } from '../constants';
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
  public pageSizeOptions: selectMenu[] = [{ option: '10 per page', value: 10 }];
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

  @ViewChild('pageSizerChild') pageSizerChild!: CommonSelectmenuComponent;

  constructor() {
    this._gridSettings = this.defaultSettings;
    this.paginationSetting = this.defaultPaginationSetting;
  }

  @Input() data: any[] = [];
  @Input() isEditable: boolean = false;
  @Input() isEquipmentTypeEditable: boolean = false;
  @Input() selectedId: number = 0;
  @Input() equipmentType: string = String.Empty;
  @Input() public set gridSettings(value: IGridSettings) {
    this._gridSettings = value || this.defaultSettings;
    this.showPagination = this._gridSettings.showPagination || false;
    this.initializePageSize();
  }

  @Input() public set paginationSettings(value: PaginationSetting) {
    this.paginationSetting = value || this.defaultPaginationSetting;
    this.pageSize = +this.paginationSetting.selectedPageSize![0].split(' ')[0];
    this.updateDisplayedData();
  }

  @Output() onSortEvent = new EventEmitter<SortConfiguration>();
  @Output() onEditEvent = new EventEmitter<number>();
  @Output() onPageChange = new EventEmitter<number>();
  @Output() onpageSizeChange = new EventEmitter<number>();
  @Output() onEquipmentEdit = new EventEmitter();
  @Output() onEquipmentSave = new EventEmitter<string>();
  @Output() onEquipmentDelete = new EventEmitter<number>();
  @Output() onDocumentEvent = new EventEmitter<{
    fileName: string;
    type: number;
  }>();
  @Output() onEditNote = new EventEmitter();
  @Output() onDeleteNote = new EventEmitter();
  @Output() onNoteEditChange = new EventEmitter<string>();

  ngOnInit(): void {
    this.updateDisplayedData();
    this.pagSizeSetter();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] || changes['paginationSettings']) {
      this.updateDisplayedData();
    }
  }

  updateDisplayedData() {
    this.displayData = this.data;
    this.updatePageNumbers();
  }

  onSetEdit(record: number) {
    if (this.isEditable) {
      this.onEditEvent.emit(record);
    }
  }
  pageSizeChangeHandler(ev: number | string) {
    this.pageSize = +ev;
    this.onpageSizeChange.emit(this.pageSize);
  }
  // -----------Sorting----------------
  public sortOrder: SortOrder = SortOrder.ASC;
  public previousSort: string = '';

  sort(col: string, sort: boolean) {
    if (sort) {
      this.sortOrder =
        col === this.previousSort && this.sortOrder === SortOrder.ASC
          ? SortOrder.DESC
          : SortOrder.ASC;

      this.previousSort = col;

      const sortDetails: SortConfiguration = {
        sort: col,
        sortOrder: this.sortOrder,
      };
      this.onSortEvent.emit(sortDetails);
      this.updateDisplayedData();
    }
  }

  ///-----------------pagination
  isShowPagination() {
    return this._gridSettings.showPagination && this.data.length > 0;
    // &&
    //   this.data.length < this.paginationSetting.totalRecords
  }

  goToPreviousPage() {
    if (this.paginationSetting.currentPage > 1) {
      this.paginationSetting.currentPage--;
      this.onPageChange.emit(this.paginationSetting.currentPage);
      this.updateDisplayedData();
    }
  }

  goToNextPage() {
    if (!this.isLastPage()) {
      this.paginationSetting.currentPage++;
      this.onPageChange.emit(this.paginationSetting.currentPage);
      this.updateDisplayedData();
    }
  }

  isLastPage(): boolean {
    const lastPage = Math.ceil(
      this.paginationSetting.totalRecords / this.pageSize
    );
    return this.paginationSetting.currentPage >= lastPage;
  }

  goToPage(page: number) {
    this.paginationSetting.currentPage = page;
    this.onPageChange.emit(page);
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
      this.pageSizeOptions = this._gridSettings.pageSizeValues.map((item) => ({
        option: item.text,
        value: item.pageNo,
      }));
    }
  }

  private initializePageSize() {
    if (
      this._gridSettings.pageSizeValues &&
      this._gridSettings.pageSizeValues.length > 0
    ) {
      this.pageSize = this._gridSettings.pageSizeValues[0].pageNo;
    }
  }

  isShowAction() {
    return (
      this._gridSettings.showEquipmentTypeDelete ||
      this._gridSettings.showEquipmentTypeEdit ||
      this._gridSettings.showEquipmentTypeSave ||
      this._gridSettings.showRolePermissionEdit ||
      this._gridSettings?.showDocument ||
      this._gridSettings?.showDownload ||
      this._gridSettings?.showDelete ||
      this._gridSettings?.showNoteEdit ||
      this._gridSettings?.showNoteDelete
    );
  }
}
