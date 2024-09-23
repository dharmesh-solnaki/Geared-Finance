import { DecimalPipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
} from 'src/app/Models/common-grid.model';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  CommonTransfer,
  SubCategory,
} from 'src/app/Models/common-transfer.model';
import { documentGridSetting } from 'src/app/Models/document.model';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { FunderService } from 'src/app/Service/funder.service';
import { validateDocType } from 'src/app/Shared/common-functions';
import { CommonTransferComponent } from 'src/app/Shared/common-transfer/common-transfer.component';
import {
  FunderModuleConstants,
  alertResponses,
  ckEditorConfig,
} from 'src/app/Shared/constants';
import { Document } from 'src/app/Models/document.model';

@Component({
  selector: 'app-funder-product-guide',
  templateUrl: './funder-product-guide.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class FunderProductGuideComponent {
  funderGuideForm: FormGroup = new FormGroup({});
  editorConfig: CKEDITOR.config = ckEditorConfig;
  activeFunderTemplate: TemplateRef<HTMLElement> | null = null;
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  activeFunderTab: string = FunderModuleConstants.FUNDER_OVERVIEW;
  chosenFundingTitle: string = FunderModuleConstants.CHOSEN_FUNDING_TITLE;
  documentList: Document[] = [];
  @ViewChild('funderOverview', { static: true })
  funderOverview!: TemplateRef<HTMLElement>;
  @ViewChild('funderDocuments', { static: true })
  funderDocuments!: TemplateRef<HTMLElement>;
  @ViewChild('LtoRTransfer', { static: false })
  LtoRTransfer!: CommonTransferComponent;
  @ViewChild('RtoLTransfer', { static: false })
  RtoLTransfer!: CommonTransferComponent;
  funderId: number = 0;
  totalRecords: number = 0;
  searchingModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };
  docGridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;

  @Output() isFunderGuideDirty = new EventEmitter<boolean>();
  selectedDocName: string = String.Empty;
  documentUrlSrc: string = String.Empty;

  constructor(
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _decimalPipe: DecimalPipe,
    private _toaster: ToastrService,
    private _funderService: FunderService,
    private _route: ActivatedRoute
  ) {
    const id = this._route.snapshot.params['id'];

    if (id) {
      this.funderId = id;
      this._funderService.getFunderGuide(id).subscribe((res) => {
        res && this.funderGuideForm.patchValue(res);
        let tempData = this.funderGuideForm.get('selectedFundings')?.value;
        if (tempData) {
          this.existingListSetter(tempData);
        }
      });
    }
  }

  ngOnInit(): void {
    this.setActiveFunderTab(this.activeFunderTab, this.funderOverview);
    this.initializeFunderOverviewForm();
    this.equipmentDataSetter();
    this.docGridSetting = documentGridSetting;
  }

  initializeFunderOverviewForm() {
    this.funderGuideForm = this._fb.group({
      financeType: [String.Empty, [Validators.required]],
      rates: [],
      isBrokerageCapped: [false],
      isApplyRitcFee: [false],
      ritcFee: [],
      isApplyAccountKeepingFee: [false],
      accountKeepingFee: [],
      isApplyDocumentFee: [false],
      funderDocFee: [],
      matrixNotes: [],
      generalNotes: [],
      eotNotes: [],
      cutoff: [],
      craa: [],
      selectedFundings: [],
      funderId: [this.funderId],
      id: [0],
    });
  }
  setActiveFunderTab(tab: string, template: TemplateRef<any>) {
    if (tab == 'funderDocuments') {
      this.getDocuments();
    }
    this.activeFunderTab = tab;
    this.activeFunderTemplate = template;
  }
  funderGuideFormSubmit() {
    this.checkValidityFunderGuide();
    if (this.funderGuideForm.invalid) {
      this.funderGuideForm.markAllAsTouched();
      let errorMsg = alertResponses.ON_FORM_INVALID;
      const invalidFields = Object.keys(this.funderGuideForm.controls)
        .filter((field) => this.funderGuideForm.get(field)?.invalid)
        .map((field) => `<br> - ${field}`);
      this._toaster.error(`${errorMsg} ${invalidFields}`, String.Empty, {
        enableHtml: true,
      });
      return;
    }
    const funderFormType = this.funderGuideForm.value;
    const id = this.funderGuideForm.get('id')?.value;
    this._funderService.upsertFunderGuide(funderFormType).subscribe(
      (res) => {
        this._toaster.success(
          id ? alertResponses.UPDATE_RECORD : alertResponses.ADD_RECORD
        );
        this.funderGuideForm.get('id')?.setValue(res);
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
  }
  checkValidityFunderGuide() {
    const funderGuideForm = this.funderGuideForm;
    const selctedFundings = funderGuideForm.get('selectedFundings')?.value;

    if (!selctedFundings || selctedFundings?.length <= 0) {
      funderGuideForm
        .get('selectedFundings')
        ?.setValidators(Validators.required);
    } else {
      funderGuideForm.get('selectedFundings')?.clearValidators();
    }
    funderGuideForm.get('selectedFundings')?.updateValueAndValidity();

    if (funderGuideForm.get('isApplyRitcFee')?.value) {
      if (!funderGuideForm.get('ritcFee')?.value) {
        funderGuideForm.get('ritcFee')?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('ritcFee')?.clearValidators();
    }
    funderGuideForm.get('ritcFee')?.updateValueAndValidity();

    if (funderGuideForm.get('isApplyAccountKeepingFee')?.value) {
      const monthlyFee = funderGuideForm.get('accountKeepingFee')?.value;
      if (!monthlyFee) {
        funderGuideForm
          .get('accountKeepingFee')
          ?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('accountKeepingFee')?.clearValidators();
    }
    funderGuideForm.get('accountKeepingFee')?.updateValueAndValidity();
    if (funderGuideForm.get('isApplyDocumentFee')?.value) {
      const monthlyFee = funderGuideForm.get('funderDocFee')?.value;
      if (!monthlyFee) {
        funderGuideForm.get('funderDocFee')?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('funderDocFee')?.clearValidators();
    }
    funderGuideForm.get('funderDocFee')?.updateValueAndValidity();
  }
  equipmentDataSetter() {
    const searchModel: CommonSearch = {
      pageNumber: 1,
      pageSize: Number.INT_MAX_VALUE,
    };
    this._equipmentService.getEquipmentTypes(searchModel).subscribe((res) => {
      if (res && res.responseData) {
        const groupedData: { [key: number]: CommonTransfer } = {};

        res.responseData.forEach((item) => {
          const categoryId = item.category.id;
          if (!groupedData[categoryId]) {
            groupedData[categoryId] = new CommonTransfer(
              categoryId,
              item.category.name,
              []
            );
          }
          groupedData[categoryId].subCategory.push(
            new SubCategory(item.id, item.name)
          );
        });
        this.availableFunding = Object.values(groupedData);
        this.filterAvailableFunding();
      }
    });
  }
  existingListSetter(list: any) {
    const groupedData: { [key: number]: CommonTransfer } = {};

    list.forEach((item: any) => {
      const categoryId = item.id;
      if (!groupedData[categoryId]) {
        groupedData[categoryId] = new CommonTransfer(categoryId, item.name, []);
      }
      item.subCategory.forEach((subCat: any) => {
        groupedData[categoryId].subCategory.push(
          new SubCategory(subCat.id, subCat.name)
        );
      });
    });
    this.existedFundings = Object.values(groupedData);
  }
  handleInputChange(field: string, type: number) {
    const formField = this.funderGuideForm.get(field);
    if (!formField?.value) {
      formField?.setValue('0.00');
    } else {
      let cleanedInput = formField?.value.replace(/[^\d.]/g, String.Empty);
      const dotIndex = cleanedInput.indexOf('.');
      if (dotIndex !== -1) {
        cleanedInput =
          cleanedInput.substring(0, dotIndex + 1) +
          cleanedInput.substring(dotIndex + 1).replace(/\./g, String.Empty);
      }
      let result2 =
        type == 1
          ? this._decimalPipe.transform(cleanedInput, '1.2-2') || '0.00'
          : this._decimalPipe.transform(cleanedInput, '1.0-2');

      formField?.setValue(result2);
    }
  }
  handleCheckoxChange(type: number) {
    const financeType = this.funderGuideForm.get('financeType');
    const financeTypeValue = financeType?.value || String.Empty;
    const selectedType = type === 0 ? 'Chattel mortgage' : 'Rental';
    const hasType = financeTypeValue.includes(selectedType);

    let updatedValue: string;
    if (hasType) {
      updatedValue = financeTypeValue
        .replace(selectedType, String.Empty)
        .replace(/,\s*,/g, ',')
        .replace(/^\s*,|,\s*$/g, String.Empty)
        .trim();
    } else {
      updatedValue = financeTypeValue
        ? `${financeTypeValue}, ${selectedType}`.trim()
        : selectedType;
    }
    financeType?.setValue(updatedValue);
  }
  addToSelectedList() {
    if (!this.LtoRTransfer.isTempListEmpty()) {
      this.LtoRTransfer.addToSelectedList();

      // Update 'existedFundings' with new subcategories
      this.LtoRTransfer.tempList.forEach((selectedItem) => {
        // Check if the category already exists in 'existedFundings'
        const existingCategory = this.existedFundings.find(
          (category) => category.id === selectedItem.id
        );

        if (existingCategory) {
          // Add missing subcategories only
          selectedItem.subCategory.forEach((newSubCat) => {
            const subCatExists = existingCategory.subCategory.some(
              (existingSubCat) => existingSubCat.id === newSubCat.id
            );
            if (!subCatExists) {
              existingCategory.subCategory.push(newSubCat);
            }
          });
        } else {
          // If the category doesn't exist, add it completely
          this.existedFundings.push(selectedItem);
        }
      });

      // Update 'availableFunding' to remove transferred subcategories
      this.LtoRTransfer.tempList.forEach((selectedItem) => {
        const availableCategory = this.availableFunding.find(
          (category) => category.id === selectedItem.id
        );

        if (availableCategory) {
          // Remove selected subcategories from the available list
          availableCategory.subCategory = availableCategory.subCategory.filter(
            (subCat) =>
              !selectedItem.subCategory.some(
                (selectedSubCat) => selectedSubCat.id === subCat.id
              )
          );

          // If all subcategories are removed, remove the category
          if (availableCategory.subCategory.length === 0) {
            this.availableFunding = this.availableFunding.filter(
              (category) => category.id !== availableCategory.id
            );
          }
        }
      });

      // Update the form field with the selected fundings
      this.funderGuideForm
        .get('selectedFundings')
        ?.setValue(this.existedFundings);

      // Clear the temporary list
      this.LtoRTransfer.clearTheTempList();
    }
  }

  removeFromSelectedList() {
    if (!this.RtoLTransfer.isTempListEmpty()) {
      this.RtoLTransfer.addToSelectedList();
      // Add the removed items back to the available list
      this.availableFunding = [
        ...this.availableFunding,
        ...this.RtoLTransfer.tempList.filter(
          (item) =>
            !this.availableFunding.some((existing) => existing.id === item.id)
        ),
      ];

      // Ensure that subcategories are correctly merged and avoid duplications
      this.RtoLTransfer.tempList.forEach((itemToAdd) => {
        const existingItem = this.availableFunding.find(
          (item) => item.id === itemToAdd.id
        );
        if (existingItem) {
          itemToAdd.subCategory.forEach((subCat) => {
            const subCatExists = existingItem.subCategory.some(
              (existingSubCat) => existingSubCat.id === subCat.id
            );
            if (!subCatExists) {
              existingItem.subCategory.push(subCat);
            }
          });
        } else {
          this.availableFunding.push(itemToAdd);
        }
      });

      // Update the selected funding list
      this.existedFundings = [...this.RtoLTransfer.availableDivDisplayList];
      this.funderGuideForm
        .get('selectedFundings')
        ?.setValue(this.existedFundings);

      // Clear the temporary list after processing
      this.RtoLTransfer.clearTheTempList();
    }
  }
  filterAvailableFunding() {
    this.availableFunding = this.availableFunding
      .map((funding) => {
        // Find the corresponding funding item in existedFundings
        const existingFunding = this.existedFundings.find(
          (ef) => ef.id === funding.id
        );

        if (existingFunding) {
          // Filter out the subCategories present in existedFunding
          const filteredSubCategories = funding.subCategory.filter(
            (subCat) =>
              !existingFunding.subCategory.some(
                (existingSubCat) => existingSubCat.id === subCat.id
              )
          );

          // Return a new CommonTransfer object with the filtered subCategories
          return new CommonTransfer(
            funding.id,
            funding.name,
            filteredSubCategories
          );
        }

        // If no matching existingFunding found, return the funding item as is
        return funding;
      })
      .filter((funding) => funding.subCategory.length > 0);
  }
  formChangeHandler() {
    this.isFunderGuideDirty.emit(this.funderGuideForm.dirty);
  }
  handleFileUpload(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const isValidFormat = validateDocType(file.type, 'pdf');
      if (isValidFormat) {
        const formData = new FormData();
        formData.append('document', file);
        this._funderService.uploadDocument(formData, this.funderId).subscribe(
          () => {
            this.getDocuments();
            this._toaster.success(alertResponses.DOC_UPLOAD_SUCCESS);
          },
          () => this._toaster.error(alertResponses.ERROR)
        );
      } else {
        this._toaster.error(alertResponses.INVALID_DOC_TYPE_PDF);
      }
    }
  }
  getDocuments() {
    this.documentList = [];
    this._funderService
      .getDocList(this.searchingModel, this.funderId)
      .subscribe((res) => {
        if (res && res.responseData) {
          (this.totalRecords = res.totalRecords),
            (this.documentList = res.responseData);
        }
        this.paginationSetter();
      });
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
    this.getDocuments();
  }
  pageChangeEventHandler(page: number) {
    this.searchingModel.pageNumber = page;
    this.getDocuments();
  }
  pageSizeChangeHandler(pageSize: number) {
    if (this.totalRecords > pageSize) {
      this.searchingModel.pageNumber = 1;
      this.searchingModel.pageSize = pageSize;
      this.getDocuments();
    }
  }

  docEventHandler(event: { fileName: string; type: number }) {
    const { fileName, type } = event;
    this.selectedDocName = fileName;
    this._funderService.getDocument(fileName).subscribe((res) => {
      let url = URL.createObjectURL(res);
      if (type == 1) {
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
      } else if (type == 2) {
        this.selectedDocName = fileName;
        this.documentUrlSrc = url;
        document.getElementById('pdfViewerButton')?.click();
      } else {
        document.getElementById('deleteConfirmationBtn')?.click();
      }
    });
  }
  deleteOkHandler() {
    this._funderService.deleteDocument(+this.selectedDocName).subscribe(
      () => {
        document.getElementById('deleteModalCancelBtn')?.click();
        this._toaster.success(alertResponses.DOC_DELETE_SUCCESS);
        this.documentList = this.documentList.filter(
          (x) => x.id != +this.selectedDocName
        );
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
  }
}
