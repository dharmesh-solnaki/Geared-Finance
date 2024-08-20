import { Component, ElementRef, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
} from 'src/app/Models/common-grid.model';
import {
  FundingEquipmentResponse,
  FundingEquipmentType,
} from 'src/app/Models/equipmentTypes.model';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { FundingEquipmentTypeGridSetting } from 'src/app/Models/equipmentTypes.model';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';
import {
  alertResponses,
  modalTitles,
  selectMenu,
} from 'src/app/Shared/constants';
import { csvMaker } from 'src/app/Shared/common-functions';

@Component({
  selector: 'app-funding-categories',
  templateUrl: './funding-categories.component.html',
})
export class FundingCategoriesComponent {
  fundingCategoties: selectMenu[] = [];
  equipmentData: FundingEquipmentResponse[] = [];
  equipmentTypeForm: FormGroup = new FormGroup({});
  equipmentId: number = 0;
  equipmentType: string = '';
  equipmentCategoryId: number = 0;
  deletionEquipmentId: number = 0;
  isEquipmentTypeEditable: boolean = false;
  modalTitle: string = modalTitles.ADDEQUIPMENTTYPE;
  deleteModalTitle: string = modalTitles.DELETEEQUIPMENTTYPE;
  @ViewChild('fundingSearchInput') fundingSearchInput!: ElementRef;
  @ViewChild('selectMenuFundingCategories')
  selectMenuFundingCategories!: CommonSelectmenuComponent;
  gridSetting!: IGridSettings;
  paginationSetting!: PaginationSetting;
  totalRecords: number = 0;
  searchModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };

  constructor(
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _toaster: ToastrService
  ) {
    this.gridSetting = FundingEquipmentTypeGridSetting;
  }
  ngOnInit(): void {
    this.equipmentTypeForm = this._fb.group({
      category: new FormControl(null, Validators.required),
      name: new FormControl('', Validators.required),
    });
    this.fundingCategorySetter();
    this.equipmentDataSetter();
  }

  formSubmitHandler() {
    if (!this.equipmentTypeForm.get('category')?.value) {
      this.equipmentTypeForm.get('category')?.setValue(null);
    }
    if (this.equipmentTypeForm.invalid) {
      this.equipmentTypeForm.markAllAsTouched();
      return;
    }

    const formValues = this.equipmentTypeForm.value;
    const euqipmentModel: FundingEquipmentType = {
      name: formValues.name,
      categoryId: formValues.category,
    };
    this._equipmentService.upsertEquipmentType(euqipmentModel).subscribe(
      () => {
        this._toaster.success(alertResponses.ADD_RECORD);
        this.equipmentDataSetter();
      },
      (err) => this._toaster.error(alertResponses.ERROR)
    );
    document.getElementById('closeBtnModal')?.click();
  }

  equipmentDataSetter() {
    this.equipmentData = [];
    this._equipmentService
      .getEquipmentTypes(this.searchModel)
      .subscribe((res) => {
        if (res && res.responseData) {
          this.totalRecords = res.totalRecords;
          this.equipmentData = res.responseData;
          this.equipmentData.map((e) => (e.categoryName = e.category.name));
        }
        this.paginationSetter();
      });
  }
  onEmptyInputField(search: string) {
    if (!search) {
      this.searchModel = {
        pageNumber: 1,
        pageSize: 10,
      };
      this.equipmentDataSetter();
    }
  }

  paginationSetter() {
    this.paginationSetting = {
      totalRecords: this.totalRecords,
      currentPage: this.searchModel.pageNumber,
      selectedPageSize: [`${this.searchModel.pageSize} per page`],
    };
  }

  searchHandler() {
    this.searchModel = {
      name: this.fundingSearchInput.nativeElement?.value,
      pageNumber: 1,
      pageSize: 10,
    };
    this.equipmentDataSetter();
  }
  sortHandler(ev: SortConfiguration) {
    const { sort, sortOrder } = ev;
    this.searchModel.sortBy = sort.trim();
    this.searchModel.sortOrder = sortOrder;
    this.searchModel.pageNumber = 1;
    this.equipmentDataSetter();
  }
  pageChangeEventHandler(page: number) {
    this.searchModel.pageNumber = page;
    this.equipmentDataSetter();
  }
  pageSizeChangeHandler(pageSize: number) {
    this.searchModel.pageNumber = 1;
    this.searchModel.pageSize = pageSize;
    this.equipmentDataSetter();
  }

  formCancelingHandler() {
    this.selectMenuFundingCategories.resetElement();
    this.selectMenuFundingCategories.filterHandler('');
    this.equipmentTypeForm.reset();
  }
  fundingCategorySetter() {
    this._equipmentService.getFundingCategories().subscribe(
      (res) => {
        this.fundingCategoties = [];
        res &&
          res.map((e) => {
            this.fundingCategoties.push({ option: e.name, value: e.id });
          });
      },
      (err) => (this.fundingCategoties = [])
    );
  }

  onEditEquipment(ev: FundingEquipmentResponse) {
    this.isEquipmentTypeEditable = true;
    (this.equipmentId = ev.id),
      (this.equipmentType = ev.name),
      (this.equipmentCategoryId = ev.category.id);
  }
  onSaveEquipment(ev: string) {
    const euqipmentModel: FundingEquipmentType = {
      name: ev,
      categoryId: this.equipmentCategoryId,
      id: this.equipmentId,
    };
    this._equipmentService.upsertEquipmentType(euqipmentModel).subscribe(
      () => {
        this._toaster.success(alertResponses.UPDATE_RECORD);
        this.equipmentDataSetter();
      },
      (err) => this._toaster.error(alertResponses.ERROR)
    );
    this.isEquipmentTypeEditable = false;
    (this.equipmentId = 0),
      (this.equipmentType = ''),
      (this.equipmentCategoryId = 0);
  }

  onDeleteEquipment(id: number) {
    this.deletionEquipmentId = id;
    document.getElementById('deleteConfirmationBtn')?.click();
  }
  deleteOkHandler() {
    this._equipmentService
      .deleteEquipmentType(this.deletionEquipmentId)
      .subscribe(
        () => {
          this._toaster.success(alertResponses.DELETE_RECORD);
          this.equipmentDataSetter();
        },
        (err) => {
          this._toaster.error(alertResponses.ERROR);
        }
      );
    document.getElementById('deleteModalCancelBtn')?.click();
  }

  downloadClickHandler() {
    let downloadData: { Equipment: string; EquipmentCategory: string }[] = [];
    const csvTitle = `EquipmentType ${Date.now().toString()}`;

    if (this.equipmentData.length == this.totalRecords) {
      this.equipmentData.forEach((e) => {
        downloadData.push({
          Equipment: e.name,
          EquipmentCategory: e.category.name,
        });
      });
      csvMaker(downloadData, csvTitle);
      return;
    } else {
      const prevPage: number = this.searchModel.pageNumber;
      const prevPageSize: number = this.searchModel.pageSize;

      this.searchModel.pageNumber = 1;
      this.searchModel.pageSize = Number.INT_MAX_VALUE;
      downloadData = [];
      this._equipmentService
        .getEquipmentTypes(this.searchModel)
        .subscribe((res) => {
          if (res && res.responseData) {
            res.responseData.forEach((e) => {
              downloadData.push({
                Equipment: e.name,
                EquipmentCategory: e.category.name,
              });
            });
          }
          csvMaker(downloadData, csvTitle);
        });

      this.searchModel.pageNumber = prevPage;
      this.searchModel.pageSize = prevPageSize;
    }
  }
}
