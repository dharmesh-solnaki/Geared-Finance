import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilterParams } from 'src/app/Models/common-filter-params.model';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
} from 'src/app/Models/common-grid.model';
import { Vendor } from 'src/app/Models/common-models';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  LeadsDisplay,
  leadListGridSetting,
} from 'src/app/Models/lead-display.model';
import { ApplicationService } from 'src/app/Service/application.service';
import { SharedTemplateService } from 'src/app/Service/shared-template.service';
import { TokenService } from 'src/app/Service/token.service';
import { VendorService } from 'src/app/Service/vendor.service';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';
import {
  RoleEnum,
  applicationTypelist,
  leadTypeList,
  pipelineList,
  recordTypeList,
  selectMenu,
} from 'src/app/Shared/constants';

@Component({
  selector: 'app-leads',
  templateUrl: './leads.component.html',
  styles: [],
})
export class LeadsComponent {
  @ViewChild('LeadsHeader', { static: true })
  leadsHeader!: TemplateRef<HTMLElement>;
  pipelineList: selectMenu[] = [];
  leadTypeList: selectMenu[] = [];
  leadVisibleList: selectMenu[] = [];
  ownerList: selectMenu[] = [];
  vendorList: selectMenu[] = [];
  vendorUserList: selectMenu[] = [];
  recordTypeList: string[] = [];
  selectedRecordType: string = 'Last year';
  isShowRecordListMenu: boolean = false;
  isShowVendorUserMultiselect: boolean = false;
  filterForm: FormGroup = new FormGroup({});
  totalRecords: number = 0;
  LeadDataList: LeadsDisplay[] = [];
  leadGridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;
  searchingModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };
  isEnableLoader: boolean = false;
  isShowApplicationListType: boolean = false;
  isShowLeadType: boolean = false;

  @ViewChild('pipelineDropdown')
  pipelineDropdown!: CommonSelectmenuComponent;

  @ViewChild('leadTypeDropdown')
  leadTypeDropdown!: CommonSelectmenuComponent;

  @ViewChild('applicationListDropdown')
  applicationListDropdown!: CommonSelectmenuComponent;

  @ViewChild('ownerIdDropdown')
  ownerIdDropdown!: CommonSelectmenuComponent;

  @ViewChild('vendorIdDropdown')
  vendorIdDropdown!: CommonSelectmenuComponent;

  @ViewChild('vendorUserIdsDropdown')
  vendorUserIdsDropdown!: CommonSelectmenuComponent;

  constructor(
    private _templateService: SharedTemplateService,
    private _vendorService: VendorService,
    private _applicationService: ApplicationService,
    private _tokenService: TokenService,
    private _fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this._templateService.setTemplate(this.leadsHeader);
    this._templateService.isSearchRequired = true;
    this.pipelineList = pipelineList;
    this.leadTypeList = leadTypeList;
    this.leadVisibleList = applicationTypelist;
    this.recordTypeList = recordTypeList;
    this.leadGridSetting = leadListGridSetting;
    this.getVendorList();
    this.getOwnerList();
    this.intializeSearchForm();
    this.getLeadDataList();
    this.handleFormValueChanges();
    this.setUserStatus();
  }

  intializeSearchForm() {
    this.filterForm = this._fb.group({
      pipelineType: [0],
      leadType: [0],
      ownerIds: [[] as number[]],
      vendorId: [0],
      vendorSalesIds: [[] as number[]],
      recordType: [this.selectedRecordType],
      listType: [0],
    });
  }

  handleFormValueChanges() {
    this.filterForm.valueChanges.subscribe(() => {
      const formValues = this.filterForm.value;
      if (formValues.pipelineType == 1) {
        this.filterForm.patchValue({ vendorId: 0 }, { emitEvent: false });
      }
      if (formValues.leadType == 1 || formValues.listType == 2) {
        this.filterForm.get('ownerIds')?.setValue([], { emitEvent: false });
      }
      if (formValues.vendorId == 0) {
        this.filterForm.patchValue(
          { vendorSalesIds: [] },
          { emitEvent: false }
        );
      }
      this.searchingModel.pageNumber = 1;
      this.searchingModel.pageSize = 10;

      this.getLeadDataList();
    });
  }

  getVendorList() {
    let vendorSearch: CommonSearch = {
      pageNumber: 1,
      pageSize: Number.INT_MAX_VALUE,
    };
    this.vendorList = [];
    this._vendorService.getVendors(vendorSearch).subscribe((res: Vendor[]) => {
      if (res) {
        this.vendorList = res.map((x) => ({
          option: x.name,
          value: x.id,
        }));
      }
    });
  }
  getOwnerList() {
    this._applicationService.getOwnerList().subscribe((res) => {
      if (res) {
        this.ownerList = res.map((x) => ({
          option: x.name,
          value: x.id,
        }));
      }
    });
  }
  getVendorUserList(vendorId: number) {
    this.vendorUserList = [];
    vendorId != 0 &&
      this._applicationService.getVendorSalesRep(vendorId).subscribe((res) => {
        if (res) {
          this.isShowVendorUserMultiselect = true;
          this.vendorUserList = res.map((e) => ({
            option: e.name,
            value: e.id,
          }));
        } else {
          this.vendorUserList.push({ option: 'No data available', value: 0 });
        }
      });
  }

  handleOwnerListValue = (ev: number[]) =>
    this.filterForm.get('ownerIds')?.setValue(ev);

  handleVendoprUserList = (ev: number[]) =>
    this.filterForm.get('vendorSalesIds')?.setValue(ev);

  handleRecordTypeChange(item: string) {
    this.selectedRecordType = item;
    this.filterForm.get('recordType')?.setValue(item);
    this.isShowRecordListMenu = false;
  }

  getLeadDataList() {
    this.isEnableLoader = true;
    this._applicationService
      .getLeadList(this.filterForm.value, this.searchingModel)
      .subscribe(
        (res) => {
          if (res && res.responseData) {
            this.LeadDataList = res.responseData;
            this.totalRecords = res.totalRecords;
          }
          this.isEnableLoader = false;

          this.paginationSetter();
        },
        () => (this.isEnableLoader = false)
      );
  }

  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.totalRecords,
      currentPage: this.searchingModel.pageNumber,
      selectedPageSize: [`${this.searchingModel.pageSize} per page`],
    };
  }
  sortHandler(ev: SortConfiguration) {
    let { sort, sortOrder } = ev;
    this.searchingModel.sortBy = sort.trim();
    this.searchingModel.sortOrder = sortOrder;
    this.searchingModel.pageNumber = 1;
    this.getLeadDataList();
  }
  pageChangeEventHandler(page: number) {
    this.searchingModel.pageNumber = page;
    this.getLeadDataList();
  }
  pageSizeChangeHandler(pageSize: number) {
    this.searchingModel.pageNumber = 1;
    this.searchingModel.pageSize = pageSize;
    this.getLeadDataList();
  }
  checkedListEmitterHandler(ev: selectMenu, type: number) {
    const controlName = type === 1 ? 'ownerIds' : 'vendorSalesIds';
    const selectedIds = (
      this.filterForm.get(controlName)?.value as number[]
    ).filter((id) => id !== +ev.value);
    this.filterForm.get(controlName)?.setValue(selectedIds);
    this.searchingModel.pageNumber = 1;
    this.searchingModel.pageSize = 10;
  }

  clearFilters() {
    this.filterForm.reset({
      pipelineType: 0,
      leadType: 0,
      ownerIds: [] as number[],
      vendorId: 0,
      vendorSalesIds: [] as number[],
      recordType: this.selectedRecordType,
      listType: 0,
    });

    this.vendorUserIdsDropdown?.resetElement(),
      this.vendorIdDropdown?.handleListCheboxChange(true),
      this.ownerIdDropdown?.handleListCheboxChange(true),
      this.leadTypeDropdown?.resetElement(),
      this.pipelineDropdown?.resetElement(),
      this.applicationListDropdown?.resetElement();
  }
  ngOnDestroy(): void {
    this._templateService.setTemplate(null);
  }

  setUserStatus() {
    const token = this._tokenService.getDecryptedToken();

    this._applicationService.getUserStatus().subscribe((res) => {
      const { userRole } = token;
      if (
        [RoleEnum.GearedSuperAdmin, RoleEnum.VendorManager].includes(userRole)
      ) {
        this.isShowApplicationListType = true;
        this.isShowLeadType =
          userRole === RoleEnum.GearedSuperAdmin
            ? res.isIncludesInGAF
            : res.isIncludesInVSR;
      }
      if (
        [RoleEnum.GearedSalesRep, RoleEnum.VendorSalesRep].includes(userRole)
      ) {
        this.isShowLeadType = true;
      }
    });
  }
}
