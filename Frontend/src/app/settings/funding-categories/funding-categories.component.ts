import { Component, ElementRef, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IGridSettings, PaginationSetting } from 'src/app/Models/common-grid.model';
import { FundingEquipmentResponse, FundingEquipmentType } from 'src/app/Models/common-models';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { FundingEquipmentTypeGridSetting } from 'src/app/Models/equipmentTypes.model';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';
import {
  alertResponses,
  modalTitles,
  selectMenu,
} from 'src/app/Shared/constants';

@Component({
  selector: 'app-funding-categories',
  templateUrl: './funding-categories.component.html',
})
export class FundingCategoriesComponent {
  fundingCategoties: selectMenu[] = [];
  equipmentData:FundingEquipmentResponse[]=[]
  equipmentTypeForm: FormGroup = new FormGroup({});
  modalTitle: string = modalTitles.ADDEQUIPMENTTYPE;
  @ViewChild('fundingSearchInput') fundingSearchInput!: ElementRef;
  @ViewChild('selectMenuFundingCategories')
  selectMenuFundingCategories!: CommonSelectmenuComponent;
  gridSetting!:IGridSettings
  paginationSetting!:PaginationSetting
  totalRecords:number= 0
  searchModel:CommonSearch={
    pageNumber:1,
    pageSize:10
  }

  constructor(
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _toaster: ToastrService
  ) {
    this.gridSetting = FundingEquipmentTypeGridSetting
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
    if (this.equipmentTypeForm.get('category')?.value == 0) {
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
        document.getElementById('closeBtnModal')?.click();
      },
      (err) => this._toaster.error(alertResponses.ERROR)
    );
  }

  equipmentDataSetter(){
   this.equipmentData=[]
   this._equipmentService.getEquipmentTypes(this.searchModel).subscribe(res=>{

      if(res && res.responseData){
        this.totalRecords = res.totalRecords
        this.equipmentData=res.responseData
        this.equipmentData.map(e=> e.categoryName= e.category.name)
      
      }

      this.paginationSetter()
    })
  }
  onEmptyInputField(search:string){
    if(search==null ||search==''){
      this.searchModel = {
        pageNumber:1,
        pageSize:10, 
      }
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
      pageNumber:1,
      pageSize:10, 
    }
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

}
